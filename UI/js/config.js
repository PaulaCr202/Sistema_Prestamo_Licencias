// Dirección del API Gateway (Persona 3 lo levanta en el puerto 5000).
export const API_BASE = 'http://localhost:5000';

// true  -> "Pedir préstamo" responde con datos falsos (Loans todavía no está listo).
// false -> llama de verdad a POST /loans/loans a través del gateway.
export const MOCK_LOANS = true;

// Rutas del contrato (siempre pasan por el gateway).
export const ENDPOINTS = {
  login: '/identity/auth/login',
  software: '/catalog/software',
  license: '/catalog/license',
  category: '/catalog/category',
  createLoan: '/loans/loans',
  myLoans: '/loans/loans/mine',   // la usa la Persona 5
  allLoans: '/loans/loans',       // la usa la Persona 5 (solo Admin)
};