import { getUser, clearSession } from './session.js';
import { esc } from './ui.js';
import { renderLogin } from './views/login.js';
import { renderCatalog } from './views/catalog.js';
import { renderMyLoans } from './views/myLoans.js';
import { renderAdmin } from './views/admin.js';

// Para agregar una pantalla nueva basta con añadir una línea aquí.
const routes = [
  { path: '#/catalogo', label: 'Catálogo', render: renderCatalog },
  { path: '#/mis-prestamos', label: 'Mis préstamos', render: renderMyLoans },
  { path: '#/admin', label: 'Admin', render: renderAdmin, role: 'Admin' },
];

const app = document.getElementById('app');
const topbar = document.getElementById('topbar');
const nav = document.getElementById('nav');
const userbox = document.getElementById('userbox');

function renderChrome(user, current) {
  topbar.hidden = false;

  nav.innerHTML = routes
    .filter((r) => !r.role || user.roles.includes(r.role))
    .map((r) => `<a href="${r.path}" class="${r === current ? 'active' : ''}">${esc(r.label)}</a>`)
    .join('');

  userbox.innerHTML = `
    <span>${esc(user.name || user.email)}</span>
    ${user.roles.map((role) => `<span class="badge">${esc(role)}</span>`).join('')}
    <button id="logout" class="link">Salir</button>`;

  document.getElementById('logout').addEventListener('click', () => {
    clearSession();
    location.hash = '#/login';
  });
}

function router() {
  const user = getUser();
  const hash = location.hash || '#/catalogo';

  if (!user) {
    topbar.hidden = true;
    if (hash !== '#/login') { location.hash = '#/login'; return; }
    renderLogin(app);
    return;
  }

  if (hash === '#/login') { location.hash = '#/catalogo'; return; }

  const route = routes.find((r) => r.path === hash);
  if (!route || (route.role && !user.roles.includes(route.role))) {
    location.hash = '#/catalogo';
    return;
  }

  renderChrome(user, route);
  route.render(app, user);
}

window.addEventListener('hashchange', router);
router();