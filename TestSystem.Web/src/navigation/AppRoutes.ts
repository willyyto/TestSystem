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
    about: '/about',
    docs: '/docs',
    blog: '/blog',
    pricing: '/pricing',
    login: '/login',
    logout: '/logout',
    unauthorised: '/unauthorised',
    quiz: '/quiz/:testId',
    result: '/result/:resultId',
    createtest: '/createtest',
});

export default AppRoutes;
