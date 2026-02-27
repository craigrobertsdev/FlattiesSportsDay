import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiGet } from '../api';
import { HOUSE_STYLES } from '../constants';

const medal = (place) => ({ 1: '🥇', 2: '🥈', 3: '🥉' }[place] ?? `${place}th`);

function rankCards(cards, key) {
  const sorted = [...cards].sort((a, b) => b[key] - a[key]);
  let place = 1;
  return sorted.map((card, i) => {
    if (i > 0 && card[key] === sorted[i - 1][key]){
      sorted[i]._place = sorted[i - 1]._place;
      return { place: sorted[i - 1]._place, card }; 
    }
    const p = { place, card };
    sorted[i]._place = place;
    place++;
    return p;
  });
}

export default function Scores() {
  const navigate = useNavigate();
  const [scores, setScores] = useState([]);
  const [tab, setTab]       = useState('athletics');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    apiGet('/api/scores')
      .then(setScores)
      .finally(() => setLoading(false));
  }, []);

  const athRanked     = rankCards(scores, 'athletic_points');
  const spiritRanked  = rankCards(scores, 'spirit_points');
  const ranked        = tab === 'athletics' ? athRanked : spiritRanked;
  const pointsKey     = tab === 'athletics' ? 'athletic_points' : 'spirit_points';

  return (
    <div className="min-h-screen bg-slate-100 flex justify-center px-4 py-6">
      <div className="w-full max-w-sm flex flex-col gap-3">
        <div className="flex items-center gap-2 mb-1">
          <button onClick={() => navigate('/')} className="rounded-xl bg-white border border-slate-200 px-2 py-1.5 text-sm text-slate-600 shadow-sm">← Back</button>
          <h1 className="text-lg font-bold">Leaderboard</h1>
        </div>

        {loading
          ? <div className="text-center text-slate-400 py-6">Loading scores…</div>
          : (
            <>
              {/* Tabs */}
              <div className="flex rounded-2xl bg-slate-200 p-1 gap-1">
                <button
                  onClick={() => setTab('athletics')}
                  className={`flex-1 rounded-xl py-1.5 text-sm font-bold transition-colors ${tab === 'athletics' ? 'bg-white text-slate-800 shadow-sm' : 'text-slate-500'}`}
                >
                  🏃 Athletics
                </button>
                <button
                  onClick={() => setTab('spirit')}
                  className={`flex-1 rounded-xl py-1.5 text-sm font-bold transition-colors ${tab === 'spirit' ? 'bg-white text-slate-800 shadow-sm' : 'text-slate-500'}`}
                >
                  ⭐ Spirit
                </button>
              </div>

              {/* Leaderboard */}
              <div className="flex flex-col gap-1.5">
                {ranked.map(({ place, card }) => (
                  <div key={card.house_name} className={`flex items-center gap-3 rounded-2xl ${HOUSE_STYLES[card.house_name].bg} px-3 py-2 shadow-sm`}>
                    <span className="text-xl w-7 text-center">{medal(place)}</span>
                    <span className={`flex-1 text-sm font-bold  ${HOUSE_STYLES[card.house_name].textOnBg}`}>{card.house_name}</span>
                    <span className={`text-sm font-bold opacity-90 ${HOUSE_STYLES[card.house_name].textOnBg}`}>{card[pointsKey]} pts</span>
                  </div>
                ))}
              </div>
            </>
          )
        }
      </div>
    </div>
  );
}

