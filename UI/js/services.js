import { apiFetch } from './api.js';
import { ENDPOINTS, MOCK_LOANS } from './config.js';

const sleep = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

// Catalog devuelve { items, totalCount, ... }.
const itemsOf = (page) => page?.items ?? page ?? [];

// Une software + licencias (+ categoría) y cuenta cuántas licencias hay disponibles.
export async function getCatalog() {
  const [software, licenses, categories] = await Promise.all([
    apiFetch(ENDPOINTS.software),
    apiFetch(ENDPOINTS.license),
    apiFetch(ENDPOINTS.category).catch(() => null), // la categoría es opcional
  ]);

  const categoryName = new Map(itemsOf(categories).map((c) => [c.id, c.nombre]));
  const licenseList = itemsOf(licenses);

  const rows = itemsOf(software).map((s) => {
    const mine = licenseList.filter((l) => l.softwareId === s.id);
    return {
      id: s.id,
      nombre: s.nombre,
      categoria: categoryName.get(s.categoryId) ?? '—',
      total: mine.length,
      disponibles: mine.filter((l) => l.isAvailable).length,
    };
  });

  // Catalog solo devuelve la primera página (15). Si hay más, el conteo sale incompleto.
  const truncated = (licenses?.totalCount ?? 0) > licenseList.length;

  return { rows, truncated };
}

export async function createLoan(softwareId, days) {
  if (MOCK_LOANS) {
    await sleep(600);
    return {
      softwareId,
      status: 'Active',
      dueDate: new Date(Date.now() + days * 86_400_000).toISOString(),
      mock: true,
    };
  }
  return apiFetch(ENDPOINTS.createLoan, { method: 'POST', body: { softwareId, days } });
}


// Consultar los préstamos del usuario autenticado.
export async function getMyLoans() {
    return apiFetch(ENDPOINTS.myLoans);
}

// Consultar todos los préstamos (solo Admin).
export async function getAllLoans() {
    return apiFetch(ENDPOINTS.allLoans);
}

// Devolver un préstamo.
export async function returnLoan(id) {
    return apiFetch(ENDPOINTS.returnLoan(id), {
        method: 'POST',
    });
}

// Extender un préstamo.
export async function extendLoan(id, extraDays) {
    return apiFetch(ENDPOINTS.extendLoan(id), {
        method: 'POST',
        body: { extraDays },
    });
}
