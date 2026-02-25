const express = require('express');
const session = require('express-session');
const SqliteStore = require('connect-sqlite3')(session);
const bcrypt = require('bcryptjs');
const path = require('path');
const { db, resetData, HOUSE_NAMES, SCHOOL_EVENTS, DB_PATH } = require('./db');

const app = express();
const PORT = process.env.PORT || 3001;
const IS_PROD = process.env.NODE_ENV === 'production';

app.use(express.json());

// ── Sessions ──────────────────────────────────────────────────────────────────

app.use(session({
  store: new SqliteStore({ db: 'sessions.db', dir: path.dirname(DB_PATH) }),
  secret: process.env.SESSION_SECRET || 'sportsday-secret-change-in-prod',
  resave: false,
  saveUninitialized: false,
  cookie: {
    secure: IS_PROD,
    httpOnly: true,
    maxAge: 1000 * 60 * 60 * 12, // 12 hours
  }
}));

// ── Serve React build in production ───────────────────────────────────────────

if (IS_PROD) {
  const clientBuild = path.join(__dirname, '../client/dist');
  app.use(express.static(clientBuild));
}

// ── Auth middleware ───────────────────────────────────────────────────────────

function requireAuth(req, res, next) {
  if (req.session && req.session.userId) return next();
  res.status(401).json({ error: 'Unauthorised' });
}

// ── Auth routes ───────────────────────────────────────────────────────────────

app.post('/api/login', (req, res) => {
  const { username, password } = req.body;
  const user = db.prepare('SELECT * FROM users WHERE username = ?').get(username);
  if (!user || !bcrypt.compareSync(password, user.password_hash)) {
    return res.status(401).json({ error: 'Invalid credentials' });
  }
  req.session.userId = user.id;
  res.json({ ok: true });
});

app.post('/api/logout', (req, res) => {
  req.session.destroy(() => res.json({ ok: true }));
});

app.get('/api/me', (req, res) => {
  if (req.session && req.session.userId) return res.json({ loggedIn: true });
  res.json({ loggedIn: false });
});

// ── Room / class-events routes ─────────────────────────────────────────────────

app.get('/api/rooms', requireAuth, (req, res) => {
  const rooms = db.prepare('SELECT room_number FROM rooms ORDER BY room_number').all();
  res.json(rooms.map(r => r.room_number));
});

app.get('/api/rooms/:roomNumber', requireAuth, (req, res) => {
  const room = db.prepare('SELECT * FROM rooms WHERE room_number = ?').get(req.params.roomNumber);
  if (!room) return res.status(404).json({ error: 'Room not found' });

  const events = db.prepare(`
    SELECT he.id, he.event_number, he.name, he.is_saved FROM house_events he
    WHERE he.room_id = ? ORDER BY he.event_number
  `).all(room.id);

  for (const ev of events) {
    ev.is_saved = !!ev.is_saved;
    ev.score_cards = db.prepare(
      'SELECT house_name, athletic_points, spirit_points FROM score_cards WHERE house_event_id = ?'
    ).all(ev.id);
  }

  // Apply the rotation offset so houses face the right event
  let idx = room.event_order_offset;
  const rotated = [];
  while (rotated.length < events.length) {
    if (idx >= events.length) idx = 0;
    rotated.push(events[idx]);
    idx++;
  }

  res.json({ room_number: room.room_number, events: rotated });
});

app.put('/api/events/:eventId', requireAuth, (req, res) => {
  const { score_cards } = req.body;
  const eventId = req.params.eventId;

  db.transaction(() => {
    for (const card of score_cards) {
      db.prepare(`
        UPDATE score_cards SET athletic_points = ?, spirit_points = ?
        WHERE house_event_id = ? AND house_name = ?
      `).run(card.athletic_points, card.spirit_points, eventId, card.house_name);
    }
    db.prepare('UPDATE house_events SET is_saved = 1 WHERE id = ?').run(eventId);
  })();

  res.json({ ok: true });
});

// ── School events routes ───────────────────────────────────────────────────────

app.get('/api/school-events', requireAuth, (req, res) => {
  res.json(SCHOOL_EVENTS);
});

app.post('/api/school-events/scores', requireAuth, (req, res) => {
  const { event_name, score_cards } = req.body;
  // score_cards: [{ house_name, athletic_points, spirit_points }]

  db.transaction(() => {
    for (const card of score_cards) {
      if (card.athletic_points > 0) {
        db.prepare('UPDATE houses SET school_event_athletic_score = school_event_athletic_score + ? WHERE name = ?')
          .run(card.athletic_points, card.house_name);
      }
      if (card.spirit_points > 0) {
        db.prepare('UPDATE houses SET school_event_spirit_score = school_event_spirit_score + ? WHERE name = ?')
          .run(card.spirit_points, card.house_name);
      }
    }
  })();

  res.json({ ok: true });
});

// ── Spirit points routes ───────────────────────────────────────────────────────

app.get('/api/spirit', requireAuth, (req, res) => {
  const spirits = db.prepare('SELECT name, spirit_score FROM house_spirits').all();
  res.json(spirits);
});

app.post('/api/spirit', requireAuth, (req, res) => {
  const { scores } = req.body; // [{ house_name, points }]

  db.transaction(() => {
    for (const s of scores) {
      db.prepare('UPDATE house_spirits SET spirit_score = spirit_score + ? WHERE name = ?')
        .run(s.points, s.house_name);
    }
  })();

  res.json({ ok: true });
});

// ── Leaderboard route ─────────────────────────────────────────────────────────

app.get('/api/scores', requireAuth, (req, res) => {
  const houses = db.prepare('SELECT name, school_event_athletic_score, school_event_spirit_score FROM houses').all();
  const spirits = db.prepare('SELECT name, spirit_score FROM house_spirits').all();

  // Sum up all class event score cards per house
  const classCards = db.prepare(`
    SELECT sc.house_name, SUM(sc.athletic_points) as ath, SUM(sc.spirit_points) as spi
    FROM score_cards sc
    JOIN house_events he ON sc.house_event_id = he.id
    WHERE he.is_saved = 1
    GROUP BY sc.house_name
  `).all();

  const scores = houses.map(h => {
    const cls = classCards.find(c => c.house_name === h.name) || { ath: 0, spi: 0 };
    const spi = spirits.find(s => s.name === h.name) || { spirit_score: 0 };
    return {
      house_name: h.name,
      athletic_points: h.school_event_athletic_score + (cls.ath || 0),
      spirit_points: h.school_event_spirit_score + (cls.spi || 0) + spi.spirit_score,
    };
  });

  res.json(scores);
});

// ── Reset route ────────────────────────────────────────────────────────────────

app.post('/api/reset', requireAuth, (req, res) => {
  resetData();
  res.json({ ok: true });
});

// ── SPA fallback (production) ─────────────────────────────────────────────────

if (IS_PROD) {
  const clientBuild = path.join(__dirname, '../client/dist');
  app.get('*', (req, res) => {
    res.sendFile(path.join(clientBuild, 'index.html'));
  });
}

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});

