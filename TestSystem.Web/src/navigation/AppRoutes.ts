/**
 * Avoid hardcoding router-paths all over the app! use this file
 * as the source of truth
 */

const AppRoutes = Object.freeze({
    root: '/',
    dashboard: '/dashboard',
    admindashboard: '/admin/dashboard',
    admintest: '/admin/test',
    admincompany: '/admin/company',
    adminuserview: '/admin/userview',
    login: '/login',
    logout: '/logout',
    unauthorised: '/unauthorised',
    quiz: '/quiz/:testId',
    result: '/result/:resultId',
    admintestcreate: '/admin/test/create',
    about: '/about',
    docs: '/docs',
    blog: '/blog',
    pricing: '/pricing',
});

export default AppRoutes;
