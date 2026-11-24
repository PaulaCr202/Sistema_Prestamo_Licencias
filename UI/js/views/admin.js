
import { getAllLoans } from '../services.js';
import { getUser } from '../session.js';
import { esc } from '../ui.js';

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

export async function renderAdmin(root) {

    const user = getUser();

    if (!user || !user.roles.includes('Admin')) {
        location.hash = '#/catalogo';
        return;
    }

    root.innerHTML = `
    <h1>Administración de préstamos</h1>
    <p class="muted">Cargando préstamos...</p>
  `;

    try {
        const loans = await getAllLoans();

        if (location.hash !== '#/admin') return;

        const active = loans.filter(
            loan => loan.status === 'Active' && !loan.isOverdue
        ).length;

        const returned = loans.filter(
            loan => loan.status === 'Returned'
        ).length;

        const overdue = loans.filter(
            loan => loan.status === 'Active' && loan.isOverdue
        ).length;

        root.innerHTML = `
      <h1>Administración de préstamos</h1>

      <p class="muted">
        Consulta general de los préstamos del sistema.
      </p>

      <div class="admin-summary">

        <div class="summary-card">
          <span>Total de préstamos</span>
          <h2>${loans.length}</h2>
        </div>

        <div class="summary-card">
          <span>Activos</span>
          <h2>${active}</h2>
        </div>

        <div class="summary-card">
          <span>Devueltos</span>
          <h2>${returned}</h2>
        </div>

        <div class="summary-card">
          <span>Vencidos</span>
          <h2>${overdue}</h2>
        </div>

      </div>

      ${loans.length === 0
                ? '<p class="muted">No hay préstamos registrados.</p>'
                : ''}

      <div class="loans-table-container">

        <table class="loans-table">

          <thead>
            <tr>
              <th>Usuario ID</th>
              <th>Software ID</th>
              <th>Licencia ID</th>
              <th>Fecha de inicio</th>
              <th>Fecha límite</th>
              <th>Fecha devolución</th>
              <th>Estado</th>
              <th>Extensiones</th>
            </tr>
          </thead>

          <tbody>
            ${loans.map(loan => `
              <tr>
                <td>${esc(loan.userId)}</td>
                <td>${esc(loan.softwareId)}</td>
                <td>${esc(loan.licenseId)}</td>
                <td>${esc(formatDate(loan.startDate))}</td>
                <td>${esc(formatDate(loan.dueDate))}</td>
                <td>${esc(formatDate(loan.returnedAt))}</td>
                <td>${esc(getStatus(loan))}</td>
                <td>${esc(loan.extensionsCount)}</td>
              </tr>
            `).join('')}
          </tbody>

        </table>

      </div>
    `;

    } catch (err) {

        if (location.hash !== '#/admin') return;

        root.innerHTML = `
      <h1>Administración de préstamos</h1>
      <div class="alert error">
        ${esc(err.message)}
      </div>
    `;
    }
}
