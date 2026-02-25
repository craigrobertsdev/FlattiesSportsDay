// Centralised fetch wrapper — always sends credentials (session cookie)
async function api(method, path, body) {
  const res = await fetch(path, {
    method,
    credentials: 'include',
    headers: body ? { 'Content-Type': 'application/json' } : {},
    body: body ? JSON.stringify(body) : undefined,
  });
  if (!res.ok) {
    const err = await res.json().catch(() => ({ error: res.statusText }));
    throw new Error(err.error || 'Request failed');
  }
  return res.json();
}

export const apiGet  = (path)        => api('GET',  path);
export const apiPost = (path, body)  => api('POST', path, body);
export const apiPut  = (path, body)  => api('PUT',  path, body);

