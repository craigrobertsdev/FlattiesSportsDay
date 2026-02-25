import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiGet, apiPost } from '../api';
import { HOUSE_NAMES, HOUSE_STYLES } from '../constants';

const PLACE_POINTS = [
  { label: '🥇 1st Place', points: 40, key: 'first' },
  { label: '🥈 2nd Place', points: 30, key: 'second' },
  { label: '🥉 3rd Place', points: 20, key: 'third' },
];

export default function SchoolEvents() {
  const navigate = useNavigate();
  const [schoolEvents, setSchoolEvents] = useState([]);
  const [selected, setSelected]         = useState('');
  const [places, setPlaces]             = useState({ first: null, second: null, third: null, fourth: null });
  const [error, setError]               = useState('');
  const [saving, setSaving]             = useState(false);

  useEffect(() => {
    apiGet('/api/school-events').then(setSchoolEvents);
  }, []);

  const needsFourth = selected === 'Tug of War' || selected === 'Team Chants';

  const isDisabled = !selected || !places.first || !places.second || !places.third || (needsFourth && !places.fourth);

  const handleSave = async () => {
    setError('');
    if (isDisabled) { setError('Please select a house for each place'); return; }

    const isChants    = selected === 'Team Chants';
    const scoreCards  = HOUSE_NAMES.map(h => ({ house_name: h, athletic_points: 0, spirit_points: 0 }));
    const assign = (houseName, pts) => {
      const card = scoreCards.find(c => c.house_name === houseName);
      if (isChants) card.spirit_points  += pts;
      else          card.athletic_points += pts;
    };

    assign(places.first,  40);
    assign(places.second, 30);
    assign(places.third,  20);
    if (needsFourth && places.fourth) assign(places.fourth, 10);

    setSaving(true);
    try {
      await apiPost('/api/school-events/scores', { event_name: selected, score_cards: scoreCards });
      // Reset form for next event
      setSelected('');
      setPlaces({ first: null, second: null, third: null, fourth: null });
    } catch (e) {
      setError(e.message);
    } finally {
      setSaving(false);
    }
  };

  const PlacePicker = ({ placeKey, label, pts }) => (
    <div className="rounded-2xl bg-white border border-slate-200 px-3 py-2 shadow-sm">
      <div className="flex items-center justify-between mb-1.5">
        <span className="text-xs font-bold text-slate-700">{label}</span>
        <span className="text-xs font-bold text-slate-400">{pts} pts</span>
      </div>
      <div className="grid grid-cols-4 gap-1.5">
        {HOUSE_NAMES.map(h => (
          <button
            key={h}
            onClick={() => setPlaces(p => ({ ...p, [placeKey]: h }))}
            className={`rounded-xl py-2 text-xs font-bold text-white ${HOUSE_STYLES[h].bg} transition-opacity ${places[placeKey] !== null && places[placeKey] !== h ? 'opacity-30' : 'opacity-100'} ${places[placeKey] === h ? 'ring-2 ring-offset-1 ring-slate-600' : ''}`}
          >
            {h}
          </button>
        ))}
      </div>
    </div>
  );

  return (
    <div className="min-h-screen bg-slate-100 flex justify-center px-4 py-6">
      <div className="w-full max-w-sm flex flex-col gap-2">
        <div className="flex items-center gap-2 mb-1">
          <button onClick={() => navigate('/')} className="rounded-xl bg-white border border-slate-200 px-2 py-1.5 text-sm text-slate-600 shadow-sm">← Back</button>
          <h1 className="text-lg font-bold">School Events</h1>
        </div>

        {/* Event picker */}
        <div className="rounded-2xl bg-white border border-slate-200 px-3 py-2 shadow-sm">
          <label className="text-xs font-bold uppercase tracking-widest text-slate-500 mb-1 block">Select Event</label>
          <select
            value={selected}
            onChange={e => { setSelected(e.target.value); setPlaces({ first: null, second: null, third: null, fourth: null }); }}
            className="w-full rounded-xl border-2 border-slate-200 bg-slate-50 px-3 py-2 text-sm font-semibold focus:border-blue-400 focus:outline-none"
          >
            <option value="" disabled hidden>Choose an event…</option>
            {schoolEvents.map(ev => <option key={ev} value={ev}>{ev}</option>)}
          </select>
        </div>

        {selected && (
          <>
            {PLACE_POINTS.map(p => <PlacePicker key={p.key} placeKey={p.key} label={p.label} pts={p.points} />)}
            {needsFourth && <PlacePicker placeKey="fourth" label="4th Place" pts={10} />}
          </>
        )}

        {error && <div className="rounded-xl bg-red-50 border border-red-200 px-3 py-2 text-xs text-red-700">⚠️ {error}</div>}

        <button
          onClick={handleSave}
          disabled={isDisabled || saving}
          className={`w-full rounded-2xl py-2.5 text-sm font-bold text-white shadow-sm ${isDisabled ? 'bg-slate-300' : 'bg-green-600'} disabled:opacity-60`}
        >
          {saving ? 'Saving…' : '✓ Save Scores'}
        </button>
      </div>
    </div>
  );
}

