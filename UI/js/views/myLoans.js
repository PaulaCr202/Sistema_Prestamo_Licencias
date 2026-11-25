
import {
    getMyLoans,
    returnLoan,
    extendLoan
} from '../services.js';

import { esc, toast } from '../ui.js';

function formatDate(date) {
    return date
        ? new Date(date).toLocaleString('es-CO')
        : '—';
}

function getStatus(loan) {
    if (loan.status === 'Returned') return 'Devuelto';
    if (loan.isOverdue) return 'Vencido';
    if (loan.status === 'Active') return 'Activo';

    return loan.status;
}

export async function renderMyLoans(root) {
    root.innerHTML = `
    <h1>Mis préstamos</h1>
    <p class="muted">Cargando préstamos...</p>
  `;

    try {
        const loans = await getMyLoans();

        if (location.hash !== '#/mis-prestamos') return;

        root.innerHTML = `
      <h1>Mis préstamos</h1>

      ${loans.length === 0
                ? '<p class="muted">No tienes préstamos registrados.</p>'
                : ''}

      <div class="loans-table-container">
        <table class="loans-table">
          <thead>
            <tr>
              <th>Software ID</th>
              <th>Fecha de inicio</th>
              <th>Fecha límite</th>
              <th>Estado</th>
              <th>Extensiones</th>
              <th>Acciones</th>
            </tr>
          </thead>

          <tbody>
            ${loans.map(loan => loanRow(loan)).join('')}
          </tbody>
        </table>
      </div>
    `;

        root.querySelectorAll('.btn-return').forEach(button => {
            button.addEventListener('click', async () => {
                if (!confirm('¿Deseas devolver este préstamo?')) return;

                button.disabled = true;

                try {
                    await returnLoan(button.dataset.id);

                    toast('Préstamo devuelto correctamente.');

                    if (location.hash === '#/mis-prestamos') {
                        await renderMyLoans(root);
                    }
                } catch (err) {
                    toast(err.message, 'error');
                    button.disabled = false;
                }
            });
        });

        root.querySelectorAll('.btn-extend').forEach(button => {
            button.addEventListener('click', async () => {
                const value = prompt(
                    '¿Cuántos días deseas extender el préstamo? (1 a 7)',
                    '1'
                );

                if (value === null) return;

                const days = Number(value);

                if (!Number.isInteger(days) || days < 1 || days > 7) {
                    toast('Debes ingresar entre 1 y 7 días.', 'error');
                    return;
                }

                button.disabled = true;

                try {
                    await extendLoan(button.dataset.id, days);

                    toast('Préstamo extendido correctamente.');

                    if (location.hash === '#/mis-prestamos') {
                        await renderMyLoans(root);
                    }
                } catch (err) {
                    toast(err.message, 'error');
                    button.disabled = false;
                }
            });
        });

    } catch (err) {
        if (location.hash !== '#/mis-prestamos') return;

        root.innerHTML = `
      <h1>Mis préstamos</h1>
      <div class="alert error">${esc(err.message)}</div>
    `;
    }
}

function loanRow(loan) {
    const active = loan.status === 'Active';
    const canExtend = active && loan.extensionsCount < 1;

    return `
    <tr>
      <td>${esc(loan.softwareId)}</td>
      <td>${esc(formatDate(loan.startDate))}</td>
      <td>${esc(formatDate(loan.dueDate))}</td>
      <td>${esc(getStatus(loan))}</td>
      <td>${esc(loan.extensionsCount)} / 1</td>
      <td class="loan-actions">

        <button
          class="btn-return"
          data-id="${esc(loan.id)}"
          ${active ? '' : 'disabled'}>
          Devolver
        </button>

        <button
          class="btn-extend"
          data-id="${esc(loan.id)}"
          ${canExtend ? '' : 'disabled'}>
          Extender
        </button>

      </td>
    </tr>
  `;
}
