import {useState, useEffect} from 'react';
import {useParams, useNavigate} from 'react-router-dom';
import {apiGet, apiPut} from '../api';
import {HOUSE_NAMES, HOUSE_STYLES, ATHLETIC_SCORES, SPIRIT_SCORES} from '../constants';

function EventCard({event, onSaved}) {
    const [cards, setCards] = useState(
        () => HOUSE_NAMES.map(h => {
            const existing = event.score_cards.find(c => c.house_name === h);
            return {
                house_name: h,
                athletic_points: existing?.athletic_points ?? 0,
                spirit_points: existing?.spirit_points ?? 0
            };
        })
    );
    const [error, setError] = useState(false);
    const [saving, setSaving] = useState(false);

    const updateCard = (houseName, field, value) => {
        setCards(prev => prev.map(c => c.house_name === houseName ? {...c, [field]: Number(value)} : c));
    };

    const handleSave = async () => {
        if (cards.some(c => c.athletic_points === 0)) {
            setError(true);
            return;
        }
        setSaving(true);
        try {
            await apiPut(`/api/events/${event.id}`, {score_cards: cards});
            onSaved();
        } catch (e) {
            console.error(e);
        } finally {
            setSaving(false);
        }
    };

    const saved = event.is_saved;

    return (
        <div
            className={`rounded-2xl border shadow-sm overflow-hidden ${saved ? 'border-green-200 bg-green-50' : 'border-slate-200 bg-white'}`}>
            {/* Title bar */}
            <div
                className={`flex items-center justify-between px-3 py-2 border-b ${saved ? 'border-green-200 bg-green-100' : 'border-slate-100 bg-slate-50'}`}>
                <h3 className="font-bold text-sm text-slate-800">{event.name}</h3>
                {saved
                    ? <span
                        className="text-xs font-bold text-green-700 bg-green-200 rounded-full px-2 py-0.5">✓ Saved</span>
                    : error && <span className="text-xs font-bold text-red-700 bg-red-100 rounded-full px-2 py-0.5">Fill all scores</span>
                }
            </div>

            {/* House rows */}
            <div className="divide-y divide-slate-100">
                {HOUSE_NAMES.map(houseName => {
                    const card = cards.find(c => c.house_name === houseName);
                    const style = HOUSE_STYLES[houseName];
                    return (
                        <div key={houseName} className="flex items-center gap-2 px-3 py-1.5">
                            <div className={`w-2.5 h-6 rounded-full ${style.bg} flex-shrink-0`}/>
                            <span className="w-16 text-xs font-semibold text-slate-700">{houseName}</span>
                            {saved ? (
                                <>
                                    <span
                                        className="flex-1 text-right text-xs text-slate-600">Athletic: <strong>{card.athletic_points}</strong></span>
                                    <span
                                        className="text-right text-xs text-slate-600">Spirit: <strong>{card.spirit_points}</strong></span>
                                </>
                            ) : (
                                <>
                                    <select
                                        className={`flex-1 rounded-lg ${style.bg} border-0 ${style.textOnBg} text-xs font-semibold py-1.5 px-1 text-center`}
                                        value={card.athletic_points || ''}
                                        onChange={e => updateCard(houseName, 'athletic_points', e.target.value)}>
                                        <option value="" disabled hidden>Athletic</option>
                                        {ATHLETIC_SCORES.map(s => <option key={s} value={s}>{s}</option>)}
                                    </select>
                                    <select
                                        className={`flex-1 rounded-lg ${style.bg} border-0 ${style.textOnBg} text-xs font-semibold py-1.5 px-1 text-center`}
                                        value={card.spirit_points || ''}
                                        onChange={e => updateCard(houseName, 'spirit_points', e.target.value)}>
                                        <option value="" disabled hidden>Spirit</option>
                                        {SPIRIT_SCORES.map(s => <option key={s} value={s}>{s}</option>)}
                                    </select>
                                </>
                            )}
                        </div>
                    );
                })}
            </div>

            {/* Save button */}
            {!saved && (
                <div className="px-3 py-2 border-t border-slate-100">
                    <button
                        onClick={handleSave}
                        disabled={saving}
                        className={`w-full rounded-xl py-2 text-xs font-bold text-white ${error ? 'bg-red-500' : 'bg-green-600'} disabled:opacity-50`}
                    >
                        {saving ? 'Saving…' : 'Save Scores'}
                    </button>
                </div>
            )}
        </div>
    );
}

export default function ClassEvents() {
    const {roomNumber} = useParams();
    const navigate = useNavigate();
    const [data, setData] = useState(null);
    const [error, setError] = useState('');

    const load = () => {
        apiGet(`/api/rooms/${roomNumber}`)
            .then(setData)
            .catch(e => setError(e.message));
    };

    useEffect(() => {
        load();
    }, [roomNumber]);

    const done = data?.events.filter(e => e.is_saved).length ?? 0;
    const total = data?.events.length ?? 0;

    return (
        <div className="min-h-screen bg-slate-100 flex justify-center px-4 py-6">
            <div className="w-full max-w-sm flex flex-col gap-3">
                <div className="flex items-center gap-2 mb-1">
                    <button onClick={() => navigate('/')}
                            className="rounded-xl bg-white border border-slate-200 px-2 py-1.5 text-sm text-slate-600 shadow-sm">←
                        Back
                    </button>
                    <div>
                        <h1 className="text-lg font-bold">Room {roomNumber}</h1>
                        {data && <p className="text-xs text-slate-500">{done} / {total} events complete</p>}
                    </div>
                </div>

                {error && <div
                    className="rounded-xl bg-red-50 border border-red-200 px-3 py-2 text-xs text-red-700">{error}</div>}
                {!data && !error && <div className="text-center text-slate-400 py-6">Loading…</div>}

                {data?.events.map(ev => (
                    <EventCard key={ev.id} event={ev} onSaved={load}/>
                ))}
            </div>
        </div>
    );
}

