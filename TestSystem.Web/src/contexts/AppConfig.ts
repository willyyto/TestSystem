/**
 * Avoid hardcoding router-paths all over the app! use this file
 * as the source of truth
 */
const API_BASE_URL = Object.freeze({
    Dev: 'https://localhost:44395/api',
});
export default API_BASE_URL;