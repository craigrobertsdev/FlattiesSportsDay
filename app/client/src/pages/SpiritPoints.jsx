import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiGet, apiPost } from '../api';
import { HOUSE_NAMES, HOUSE_STYLES } from '../constants';

export default function SpiritPoints() {
  const navigate  = useNavigate();
  const [scores, setScores]   = useState(() => Object.fromEntries(HOUSE_NAMES.map(h => [h, 0])));
  const [saving, setSaving]   = useState(false);

  const clamp = (v) => Math.max(0, Math.min(40, v));

  const handleSave = async () => {
    setSaving(true);
    try {
      await apiPost('/api/spirit', {
        scores: HOUSE_NAMES.map(h => ({ house_name: h, points: scores[h] })),
      });
      navigate('/');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="min-h-screen bg-slate-100 flex justify-center px-4 py-6">
      <div className="w-full max-w-sm flex flex-col gap-3">
        <div className="flex items-center gap-2 mb-1">
          <button onClick={() => navigate('/')} className="rounded-xl bg-white border border-slate-200 px-2 py-1.5 text-sm text-slate-600 shadow-sm">← Back</button>
          <h1 className="text-lg font-bold">Spirit Points</h1>
        </div>

        <div className="flex flex-col gap-2">
          {HOUSE_NAMES.map(h => (
            <div key={h} className={`rounded-2xl ${HOUSE_STYLES[h].bg} px-4 py-2.5 shadow-sm flex items-center justify-between`}>
              <span className="text-sm font-bold text-white drop-shadow">{h}</span>
              <div className="flex items-center gap-3">
                <button
                  onClick={() => setScores(s => ({ ...s, [h]: clamp(s[h] - 10) }))}
                  className="w-8 h-8 rounded-full bg-white/30 text-white font-bold text-lg leading-none"
                >−</button>
                <span className="text-xl font-bold text-white w-8 text-center drop-shadow">{scores[h]}</span>
                <button
                  onClick={() => setScores(s => ({ ...s, [h]: clamp(s[h] + 10) }))}
                  className="w-8 h-8 rounded-full bg-white/30 text-white font-bold text-lg leading-none"
                >+</button>
              </div>
            </div>
          ))}
        </div>

        <div className="flex flex-col gap-2 mt-1">
          <button
            onClick={handleSave}
            disabled={saving}
            className="w-full rounded-2xl bg-green-600 py-2.5 text-sm font-bold text-white shadow-sm disabled:opacity-50"
          >
            {saving ? 'Saving…' : '✓ Save Spirit Points'}
          </button>
          <button
            onClick={() => navigate('/')}
            className="w-full rounded-2xl bg-white border border-slate-200 py-2.5 text-sm font-bold text-slate-600 shadow-sm"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
}

