import { useState } from 'react';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const { login } = useAuth();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError]       = useState('');
  const [loading, setLoading]   = useState(false);

  const handleSubmit = async e => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await login(username, password);
    } catch {
      setError('Invalid username or password');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-slate-100 flex items-center justify-center px-4">
      <div className="w-full max-w-sm">
        <div className="rounded-2xl bg-gradient-to-br from-blue-600 to-blue-800 px-6 py-5 text-center text-white shadow-md mb-4">
          <h1 className="text-xl font-bold">🏆 McLaren Flat Sports Day 🏆</h1>
          <p className="text-blue-200 text-xs font-medium mt-0.5">Sign in to continue</p>
        </div>

        <form onSubmit={handleSubmit} className="rounded-2xl bg-white shadow-sm border border-slate-200 px-5 py-5 flex flex-col gap-3">
          {error && (
            <div className="rounded-xl bg-red-50 border border-red-200 px-3 py-2 text-xs text-red-700">{error}</div>
          )}
          <div className="flex flex-col gap-1">
            <label className="text-xs font-bold uppercase tracking-widest text-slate-500">Username</label>
            <input
              type="text"
              autoComplete="username"
              value={username}
              onChange={e => setUsername(e.target.value)}
              className="rounded-xl border-2 border-slate-200 bg-slate-50 px-3 py-2 text-sm focus:border-blue-400 focus:outline-none"
              required
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs font-bold uppercase tracking-widest text-slate-500">Password</label>
            <input
              type="password"
              autoComplete="current-password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              className="rounded-xl border-2 border-slate-200 bg-slate-50 px-3 py-2 text-sm focus:border-blue-400 focus:outline-none"
              required
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            className="w-full rounded-xl bg-blue-600 py-2.5 text-sm font-bold text-white shadow-sm disabled:opacity-50"
          >
            {loading ? 'Signing in…' : 'Sign In'}
          </button>
        </form>
      </div>
    </div>
  );
}

