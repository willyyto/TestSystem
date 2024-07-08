import axios from 'axios';
import API_BASE_URL from 'contexts/AppConfig';
import { getToken } from 'contexts/TokenService';

// Create an Axios instance
const apiClient = axios.create({
    baseURL: API_BASE_URL.Dev,
    headers: {
        'Content-Type': 'application/json'
    }
});

// URLs that do not require authentication
const noAuthUrls = [ '/public'];

apiClient.interceptors.request.use(
    (config) => {
        const token = getToken();
        if (token && !noAuthUrls.some(url => config.url?.includes(url))) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default apiClient;
