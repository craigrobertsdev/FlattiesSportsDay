import { createContext, useContext, useState, useEffect } from 'react';
import { apiGet, apiPost } from '../api';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [loggedIn, setLoggedIn] = useState(null); // null = loading

  useEffect(() => {
    apiGet('/api/me')
      .then(d => setLoggedIn(d.loggedIn))
      .catch(() => setLoggedIn(false));
  }, []);

  const login = async (username, password) => {
    await apiPost('/api/login', { username, password });
    setLoggedIn(true);
  };

  const logout = async () => {
    await apiPost('/api/logout');
    setLoggedIn(false);
  };

  return (
    <AuthContext.Provider value={{ loggedIn, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);

