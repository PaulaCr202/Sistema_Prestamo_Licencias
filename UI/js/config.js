
export const API_BASE = 'http://localhost:5000';

// Conexión real con el microservicio Loans.
export const MOCK_LOANS = false;

// Rutas a través del API Gateway.
export const ENDPOINTS = {
    login: '/identity/auth/login',
    software: '/catalog/software',
    license: '/catalog/license',
    category: '/catalog/category',

    createLoan: '/loans/loans',
    myLoans: '/loans/loans/mine',
    allLoans: '/loans/loans',

    returnLoan: (id) => `/loans/loans/${id}/return`,
    extendLoan: (id) => `/loans/loans/${id}/extend`,
};
