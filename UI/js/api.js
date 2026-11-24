import { API_BASE } from './config.js';
import { getToken, clearSession } from './session.js';

export class ApiError extends Error {
  constructor(message, status = 0) {
    super(message);
    this.status = status;
  }
}

// Los 3 formatos de error que devuelve el backend:
//  - Loans / Catalog:  { error: "..." }
//  - Identity:         { isSuccess:false, errors:[{ description }] }
//  - Validación .NET:  { title, errors: { Campo: ["..."] } }
function extractMessage(data, status) {
  if (data?.error) return data.error;

  if (Array.isArray(data?.errors) && data.errors.length) {
    return data.errors.map((e) => e.description).join(' ');
  }
  if (data?.errors && typeof data.errors === 'object') {
    return Object.values(data.errors).flat().join(' ');
  }

  if (data?.message) return data.message;

  if (status === 403) return 'No tienes permiso para hacer esta acción.';
  if (status === 404) return 'No se encontró lo que buscabas.';
  if (status >= 502 && status <= 504) return 'Un servicio no está disponible en este momento.';
  return data?.title ?? `Error inesperado (${status}).`;
}

export async function apiFetch(path, { method = 'GET', body, auth = true } = {}) {
  const headers = { Accept: 'application/json' };
  if (body !== undefined) headers['Content-Type'] = 'application/json';

  const token = getToken();
  if (auth && token) headers.Authorization = `Bearer ${token}`;

  let res;
  try {
    res = await fetch(API_BASE + path, {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    });
  } catch {
    throw new ApiError('No se pudo conectar con el servidor. ¿Está encendido el gateway?');
  }

  const text = await res.text();
  let data = null;
  if (text) {
    try { data = JSON.parse(text); } catch { data = null; }
  }

  if (!res.ok) {
    // Token vencido o inválido: se cierra la sesión y se vuelve al login.
    if (res.status === 401 && auth) {
      clearSession();
      location.hash = '#/login';
      throw new ApiError('Tu sesión expiró. Inicia sesión de nuevo.', 401);
    }
    throw new ApiError(extractMessage(data, res.status), res.status);
  }

  return data;
}