import { apiFetch, ApiError } from '../api.js';
import { ENDPOINTS } from '../config.js';
import { saveToken } from '../session.js';
import { esc } from '../ui.js';

const DEMO_USERS = [
  { label: 'Usuario', email: 'user@licencias.local', password: '1234' },
  { label: 'Admin', email: 'admin@licencias.local', password: '1234*' },
];

export function renderLogin(root) {
  root.innerHTML = `
    <section class="card login-card">
      <h1>Iniciar sesión</h1>
      <div id="login-error"></div>
      <form id="login-form">
        <label for="email">Correo</label>
        <input id="email" type="email" required autocomplete="username">
        <label for="password">Contraseña</label>
        <input id="password" type="password" required autocomplete="current-password">
        <button type="submit">Entrar</button>
      </form>
      <div class="demo-users">
        Usuarios de prueba:<br>
        ${DEMO_USERS.map((u, i) => `<button type="button" class="chip" data-demo="${i}">${esc(u.label)}</button>`).join('')}
      </div>
    </section>`;

  const form = root.querySelector('#login-form');
  const errorBox = root.querySelector('#login-error');
  const emailInput = root.querySelector('#email');
  const passwordInput = root.querySelector('#password');
  const submit = form.querySelector('button[type="submit"]');

  root.querySelectorAll('[data-demo]').forEach((btn) => {
    btn.addEventListener('click', () => {
      const u = DEMO_USERS[Number(btn.dataset.demo)];
      emailInput.value = u.email;
      passwordInput.value = u.password;
    });
  });

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    errorBox.innerHTML = '';
    submit.disabled = true;
    submit.textContent = 'Entrando…';

    try {
      const res = await apiFetch(ENDPOINTS.login, {
        method: 'POST',
        auth: false,
        body: { email: emailInput.value.trim(), password: passwordInput.value },
      });

      // Identity responde { isSuccess, data: { accessToken, tokenType, expiresAtUTC } }
      const token = res?.data?.accessToken ?? res?.accessToken;
      if (!token) throw new ApiError('La respuesta del login no trae token.');

      saveToken(token);
      location.hash = '#/catalogo';
    } catch (err) {
      errorBox.innerHTML = `<div class="alert error">${esc(err.message)}</div>`;
      submit.disabled = false;
      submit.textContent = 'Entrar';
    }
  });
}