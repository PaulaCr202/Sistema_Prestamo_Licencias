import { getCatalog, createLoan } from '../services.js';
import { MOCK_LOANS } from '../config.js';
import { esc, toast } from '../ui.js';

const MIN_DAYS = 1;
const MAX_DAYS = 30; // mismo límite que valida Loan en el dominio

export async function renderCatalog(root) {
  root.innerHTML = `<h1>Catálogo de software</h1><p class="muted">Cargando catálogo…</p>`;

  let catalog;
  try {
    catalog = await getCatalog();
  } catch (err) {
    if (location.hash !== '#/catalogo') return;
    root.innerHTML = `<h1>Catálogo de software</h1><div class="alert error">${esc(err.message)}</div>`;
    return;
  }

  // Si el usuario cambió de pantalla mientras cargaba, no se pinta nada.
  if (location.hash !== '#/catalogo') return;

  const { rows, truncated } = catalog;

  root.innerHTML = `
    <h1>Catálogo de software</h1>
    ${MOCK_LOANS ? '<div class="alert warn">Modo demo: los préstamos son simulados (Loans aún no está conectado).</div>' : ''}
    ${truncated ? '<div class="alert warn">Catalog devolvió solo la primera página de licencias; los conteos pueden estar incompletos.</div>' : ''}
    ${rows.length === 0 ? '<p class="muted">No hay software registrado.</p>' : ''}
    <div class="grid">
      ${rows.map(cardHtml).join('')}
    </div>`;

  root.querySelectorAll('.loan-form').forEach((form) => {
    form.addEventListener('submit', async (e) => {
      e.preventDefault();

      const days = Number(form.elements.days.value);
      if (!Number.isInteger(days) || days < MIN_DAYS || days > MAX_DAYS) {
        toast(`Los días deben estar entre ${MIN_DAYS} y ${MAX_DAYS}.`, 'error');
        return;
      }

      const button = form.querySelector('button');
      button.disabled = true;
      button.textContent = 'Enviando…';

      try {
        await createLoan(form.dataset.id, days);
        toast(`Préstamo de ${form.dataset.nombre} registrado por ${days} día(s).`);
        await renderCatalog(root); // se vuelve a pedir el catálogo para ver las licencias actualizadas
      } catch (err) {
        toast(err.message, 'error');
        button.disabled = false;
        button.textContent = 'Pedir préstamo';
      }
    });
  });
}

function cardHtml(s) {
  const hasStock = s.disponibles > 0;
  return `
    <article class="card">
      <h2 class="sw-name">${esc(s.nombre)}</h2>
      <div class="sw-cat">${esc(s.categoria)}</div>
      <div class="stock ${hasStock ? 'ok' : 'none'}">
        <strong>${s.disponibles}</strong> de ${s.total} licencias disponibles
      </div>
      <form class="loan-form" data-id="${esc(s.id)}" data-nombre="${esc(s.nombre)}">
        <input name="days" type="number" min="${MIN_DAYS}" max="${MAX_DAYS}" value="7" aria-label="Días de préstamo" ${hasStock ? '' : 'disabled'}>
        <button type="submit" ${hasStock ? '' : 'disabled'}>Pedir préstamo</button>
      </form>
    </article>`;
}