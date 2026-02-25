const Database = require('better-sqlite3');
const bcrypt = require('bcryptjs');
const path = require('path');

const DB_PATH = process.env.DB_PATH || (process.env.NODE_ENV === 'production' ? '/data/sportsday.db' : path.join(__dirname, 'sportsday.db'));
const db = new Database(DB_PATH);

db.pragma('journal_mode = WAL');
db.pragma('foreign_keys = ON');

// ── Schema ────────────────────────────────────────────────────────────────────

db.exec(`
  CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username TEXT UNIQUE NOT NULL,
    password_hash TEXT NOT NULL
  );

  CREATE TABLE IF NOT EXISTS houses (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT UNIQUE NOT NULL,
    school_event_athletic_score INTEGER NOT NULL DEFAULT 0,
    school_event_spirit_score   INTEGER NOT NULL DEFAULT 0
  );

  CREATE TABLE IF NOT EXISTS house_spirits (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT UNIQUE NOT NULL,
    spirit_score INTEGER NOT NULL DEFAULT 0
  );

  CREATE TABLE IF NOT EXISTS rooms (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    room_number INTEGER UNIQUE NOT NULL,
    event_order_offset INTEGER NOT NULL DEFAULT 0
  );

  CREATE TABLE IF NOT EXISTS house_events (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    event_number INTEGER NOT NULL,
    name TEXT NOT NULL,
    room_id INTEGER NOT NULL REFERENCES rooms(id),
    is_saved INTEGER NOT NULL DEFAULT 0
  );

  CREATE TABLE IF NOT EXISTS score_cards (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    house_event_id INTEGER NOT NULL REFERENCES house_events(id),
    house_name TEXT NOT NULL,
    athletic_points INTEGER NOT NULL DEFAULT 0,
    spirit_points   INTEGER NOT NULL DEFAULT 0
  );
`);

// ── Seed ──────────────────────────────────────────────────────────────────────

const HOUSE_NAMES   = ['Sturt', 'Wickham', 'Elliott', 'Leslie'];
const HOUSE_EVENTS  = ['Long Jump', 'Gaga Ball', 'Bombardment', 'Marathon', 'Hurdles', 'Nerf Javelin', 'Spoke Relay', 'Shot Put'];
const SCHOOL_EVENTS = ['Sprints', 'Baton Relay', 'Ribbon Relay', 'Tug of War', 'Team Chants'];
const ROOMS         = [
  { number: 5,  offset: 7 },
  { number: 6,  offset: 0 },
  { number: 7,  offset: 5 },
  { number: 8,  offset: 6 },
  { number: 12, offset: 1 },
  { number: 13, offset: 2 },
  { number: 14, offset: 3 },
  { number: 15, offset: 4 },
];

function seedInitialData() {
  const userExists = db.prepare('SELECT id FROM users WHERE username = ?').get('teacher');
  if (!userExists) {
    const hash = bcrypt.hashSync('Teacher123', 10);
    db.prepare('INSERT INTO users (username, password_hash) VALUES (?, ?)').run('teacher', hash);
  }

  const housesExist = db.prepare('SELECT id FROM houses LIMIT 1').get();
  if (!housesExist) {
    const insertHouse  = db.prepare('INSERT INTO houses (name) VALUES (?)');
    const insertSpirit = db.prepare('INSERT INTO house_spirits (name) VALUES (?)');
    const insertRoom   = db.prepare('INSERT INTO rooms (room_number, event_order_offset) VALUES (?, ?)');
    const insertEvent  = db.prepare('INSERT INTO house_events (event_number, name, room_id) VALUES (?, ?, ?)');
    const insertCard   = db.prepare('INSERT INTO score_cards (house_event_id, house_name) VALUES (?, ?)');

    const seedAll = db.transaction(() => {
      for (const name of HOUSE_NAMES) {
        insertHouse.run(name);
        insertSpirit.run(name);
      }
      for (const room of ROOMS) {
        insertRoom.run(room.number, room.offset);
      }
      const rooms = db.prepare('SELECT id, room_number, event_order_offset FROM rooms').all();
      for (const room of rooms) {
        for (let i = 0; i < HOUSE_EVENTS.length; i++) {
          const { lastInsertRowid } = insertEvent.run(i, HOUSE_EVENTS[i], room.id);
          for (const name of HOUSE_NAMES) {
            insertCard.run(lastInsertRowid, name);
          }
        }
      }
    });
    seedAll();
  }
}

seedInitialData();

// ── Re-seed (reset) ───────────────────────────────────────────────────────────

function resetData() {
  const insertHouse  = db.prepare('INSERT INTO houses (name) VALUES (?)');
  const insertSpirit = db.prepare('INSERT INTO house_spirits (name) VALUES (?)');
  const insertRoom   = db.prepare('INSERT INTO rooms (room_number, event_order_offset) VALUES (?, ?)');
  const insertEvent  = db.prepare('INSERT INTO house_events (event_number, name, room_id) VALUES (?, ?, ?)');
  const insertCard   = db.prepare('INSERT INTO score_cards (house_event_id, house_name) VALUES (?, ?)');

  db.transaction(() => {
    db.prepare('DELETE FROM score_cards').run();
    db.prepare('DELETE FROM house_events').run();
    db.prepare('DELETE FROM house_spirits').run();
    db.prepare('DELETE FROM rooms').run();
    db.prepare('DELETE FROM houses').run();

    for (const name of HOUSE_NAMES) {
      insertHouse.run(name);
      insertSpirit.run(name);
    }
    for (const room of ROOMS) {
      insertRoom.run(room.number, room.offset);
    }
    const rooms = db.prepare('SELECT id, room_number, event_order_offset FROM rooms').all();
    for (const room of rooms) {
      for (let i = 0; i < HOUSE_EVENTS.length; i++) {
        const { lastInsertRowid } = insertEvent.run(i, HOUSE_EVENTS[i], room.id);
        for (const name of HOUSE_NAMES) {
          insertCard.run(lastInsertRowid, name);
        }
      }
    }
  })();
}

module.exports = { db, resetData, HOUSE_NAMES, HOUSE_EVENTS, SCHOOL_EVENTS, ROOMS, DB_PATH };

