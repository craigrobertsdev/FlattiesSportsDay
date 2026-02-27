import { useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { apiPost } from '../api';
import { HOUSE_NAMES, HOUSE_STYLES, ROOM_NUMBERS } from '../constants';

export default function Home() {
  const { logout } = useAuth();
  const navigate   = useNavigate();
  const [selectedRoom, setSelectedRoom] = useState('');
  const [resetting, setResetting]       = useState(false);
  const dialogRef = useRef(null);

  const handleReset = async () => {
    setResetting(true);
    try {
      await apiPost('/api/reset');
    } finally {
      setResetting(false);
      dialogRef.current?.close();
    }
  };

  return (
    <div className="min-h-screen bg-slate-100 flex justify-center px-4 py-6">
      <div className="w-full max-w-sm flex flex-col gap-3">

        {/* Header */}
        <header className="rounded-2xl bg-gradient-to-br from-blue-600 to-blue-800 px-4 py-3 text-center text-white shadow-md">
          <h1 className="text-xl font-bold leading-tight">🏆 McLaren Flat Sports Day 🏆</h1>
          <p className="text-blue-200 text-xs font-medium">{new Date().getFullYear()}</p>
        </header>

        {/* Class Events */}
        <div className="rounded-2xl bg-white shadow-sm border border-slate-200 px-3 py-2 flex flex-col gap-2">
          <p className="text-xs font-bold uppercase tracking-widest text-slate-500">Class Events</p>
          <div className="flex gap-2">
            <select
              value={selectedRoom}
              onChange={e => setSelectedRoom(e.target.value)}
              className="flex-1 rounded-xl border-2 border-slate-200 bg-slate-50 px-3 py-2 text-sm font-semibold focus:border-blue-400 focus:outline-none"
            >
              <option value="" disabled hidden>Select room…</option>
              {ROOM_NUMBERS.map(r => <option key={r} value={r}>Room {r}</option>)}
            </select>
            <button
              onClick={() => navigate(`/class-events/${selectedRoom}`)}
              disabled={!selectedRoom}
              className={`rounded-xl px-4 py-2 text-sm font-bold shadow-sm ${selectedRoom ? HOUSE_STYLES[HOUSE_NAMES[0]].bg : 'bg-slate-300'} ${selectedRoom ? HOUSE_STYLES[HOUSE_NAMES[0]].textOnBg : 'text-white'}`}
            >
              Go →
            </button>
          </div>
        </div>

        {/* Nav buttons */}
        <div className="flex flex-col gap-2 px-6">
          <button
            onClick={() => navigate('/school-events')}
            className={`flex items-center gap-3 rounded-2xl px-4 py-2.5 text-left text-sm font-bold shadow-sm ${HOUSE_STYLES[HOUSE_NAMES[1]].textOnBg} ${HOUSE_STYLES[HOUSE_NAMES[1]].bg}`}
          >
            <span className="text-xl">🏫</span>
            <span>Whole School Events</span>
          </button>
          <button
            onClick={() => navigate('/spirit')}
            className={`flex items-center gap-3 rounded-2xl px-4 py-2.5 text-left text-sm font-bold shadow-sm ${HOUSE_STYLES[HOUSE_NAMES[2]].textOnBg} ${HOUSE_STYLES[HOUSE_NAMES[2]].bg}`}
          >
            <span className="text-xl">⭐</span>
            <span>Award Spirit Points</span>
          </button>
          <button
            onClick={() => navigate('/scores')}
            className={`flex items-center gap-3 rounded-2xl px-4 py-2.5 text-left text-sm font-bold shadow-sm ${HOUSE_STYLES[HOUSE_NAMES[3]].textOnBg} ${HOUSE_STYLES[HOUSE_NAMES[3]].bg}`}
          >
            <span className="text-xl">📊</span>
            <span>Leaderboard</span>
          </button>
        </div>

        {/* Danger zone */}
        <details className="rounded-2xl border border-red-200 bg-red-50">
          <summary className="cursor-pointer list-none px-3 py-2 text-sm font-semibold text-red-700 select-none">
            ⚠️ Danger zone
          </summary>
          <div className="px-3 pb-3">
            <p className="mb-2 text-xs text-red-600">Resetting will clear all scores. This cannot be undone.</p>
            <button
              onClick={() => dialogRef.current?.showModal()}
              className="w-full rounded-xl bg-red-700 px-4 py-2 text-sm font-bold text-white shadow-sm"
            >
              Reset all data
            </button>
          </div>
        </details>

        {/* Sign out */}
        <button
          onClick={logout}
          className="text-xs text-slate-400 underline text-center"
        >
          Sign out
        </button>
      </div>

      {/* Confirm dialog */}
      <dialog ref={dialogRef} className="w-80 rounded-2xl p-4 shadow-xl backdrop:bg-black/40">
        <h2 className="text-base font-bold text-slate-800 mb-1">Are you sure?</h2>
        <p className="mb-3 text-sm text-slate-500">This will reset all scores for the day and cannot be undone.</p>
        <div className="flex gap-2">
          <button
            onClick={handleReset}
            disabled={resetting}
            className="flex-1 rounded-xl bg-red-700 py-2 text-sm font-bold text-white disabled:opacity-50"
          >
            {resetting ? 'Resetting…' : 'Yes, reset'}
          </button>
          <button
            onClick={() => dialogRef.current?.close()}
            className="flex-1 rounded-xl bg-slate-200 py-2 text-sm font-bold text-slate-700"
          >
            Cancel
          </button>
        </div>
      </dialog>
    </div>
  );
}

