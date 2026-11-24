// Guarda el token y lee de él el usuario y sus roles.
// sessionStorage: la sesión vive por pestaña (sirve para tener user y admin en pestañas distintas).
const KEY = 'licencias.token';
const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

export const saveToken = (token) => sessionStorage.setItem(KEY, token);
export const getToken = () => sessionStorage.getItem(KEY);
export const clearSession = () => sessionStorage.removeItem(KEY);

function decodeJwt(token) {
  const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
  const bytes = atob(payload);
  const json = decodeURIComponent(
    Array.from(bytes, (c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0')).join('')
  );
  return JSON.parse(json);
}

// Devuelve { id, email, name, roles } o null si no hay sesión válida.
export function getUser() {
  const token = getToken();
  if (!token) return null;

  try {
    const claims = decodeJwt(token);

    if (claims.exp && claims.exp * 1000 < Date.now()) {
      clearSession();
      return null;
    }

    // Con un solo rol llega un string; con varios (el admin), un arreglo.
    const raw = claims.role ?? claims.roles ?? claims[ROLE_CLAIM] ?? [];
    const roles = Array.isArray(raw) ? raw : [raw];

    return {
      id: claims.sub ?? claims.nameid,
      email: claims.unique_name ?? claims.name ?? claims.email ?? '',
      name: `${claims.firstName ?? ''} ${claims.lastName ?? ''}`.trim(),
      roles,
    };
  } catch {
    clearSession();
    return null;
  }
}