import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Login from './pages/Login';
import Home from './pages/Home';
import ClassEvents from './pages/ClassEvents';
import SchoolEvents from './pages/SchoolEvents';
import SpiritPoints from './pages/SpiritPoints';
import Scores from './pages/Scores';;

function RequireAuth({ children }) {
  const { loggedIn } = useAuth();
  if (loggedIn === null) return <div className="min-h-screen bg-slate-100 flex items-center justify-center text-slate-400">Loading…</div>;
  if (!loggedIn) return <Navigate to="/login" replace />;
  return children;
}

function AppRoutes() {
  const { loggedIn } = useAuth();
  return (
    <Routes>
      <Route path="/login" element={loggedIn ? <Navigate to="/" replace /> : <Login />} />
      <Route path="/" element={<RequireAuth><Home /></RequireAuth>} />
      <Route path="/class-events/:roomNumber" element={<RequireAuth><ClassEvents /></RequireAuth>} />
      <Route path="/school-events" element={<RequireAuth><SchoolEvents /></RequireAuth>} />
      <Route path="/spirit" element={<RequireAuth><SpiritPoints /></RequireAuth>} />
      <Route path="/scores" element={<RequireAuth><Scores /></RequireAuth>} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}
