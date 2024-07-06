/**
 * Avoid hardcoding router-paths all over the app! use this file
 * as the source of truth
 */

const AppRoutes = Object.freeze({
    root: '/',
    about: '/about',
    docs: '/docs',
    blog: '/blog',
    pricing: '/pricing',
    login: '/login',
});

export default AppRoutes;
