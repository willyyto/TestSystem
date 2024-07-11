import React, {createContext, useContext, useEffect, useState} from 'react';
import axios from 'axios';
import {jwtDecode} from 'jwt-decode';
import {useNavigate} from 'react-router-dom';
import {clearTokens, getRefreshToken, getToken, setRefreshToken, setToken} from 'contexts/TokenService';
import API_BASE_URL from 'contexts/AppConfig';

interface AuthContextType {
    login: (username: string, password: string) => Promise<void>;
    register: (username: string, password: string, email: string, name: string, role: string) => Promise<void>;
    logout: () => void;
    isAuthenticated: boolean;
    hasRole: (role: string) => boolean;
    getToken: () => string | null;
    userRole: string | null;
    userGivenName: string | null;
    userEmail: string | null;// Expose userRole for loading state in ProtectedRoute
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [isAuthenticatedState, setIsAuthenticatedState] = useState<boolean>(!!getToken());
    const [userRole, setUserRole] = useState<string | null>(null);
    const [userGivenName, setUserGivenName] = useState<string | null>(null);
    const [userEmail, setUserEmail] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const token = getToken();
        if (token) {
            const decodedToken: any = jwtDecode(token);
            if (decodedToken.exp * 1000 < Date.now()) {
                refreshAccessToken();
            } else {
                setUserRole(decodedToken.role);
                setUserGivenName(decodedToken.given_name);
                setUserEmail(decodedToken.email);
            }
        }
    }, []);

    const login = async (username: string, password: string) => {
        try {
            const response = await axios.post(`${API_BASE_URL.Dev}/auth/login`, { username, password });
            const { token, refreshToken } = response.data;
            setToken(token);
            setRefreshToken(refreshToken);
            setIsAuthenticatedState(true);
            const decodedToken: any = jwtDecode(token);
            setUserRole(decodedToken.role);
            setUserGivenName(decodedToken.given_name);
            setUserEmail(decodedToken.email);
            navigate('/dashboard');
        } catch (error) {
            console.error('Login failed', error);
            throw new Error('Login failed. Please check your credentials and try again.');
        }
    };

    const register = async (username: string, password: string, email: string, name: string, role: string) => {
        try {
            await axios.post(`${API_BASE_URL.Dev}/auth/register`, { username, password, email, name, role });
            await login(username, password);
        } catch (error) {
            console.error('Registration failed', error);
            throw new Error('Registration failed. Please try again.');
        }
    };

    const logout = () => {
        clearTokens();
        setIsAuthenticatedState(false);
        setUserRole(null);
        setUserEmail(null);
        setUserGivenName(null);
        navigate('/login');
    };

    const refreshAccessToken = async () => {
        const refreshToken = getRefreshToken();
        if (!refreshToken) {
            logout();
            return;
        }
        try {
            const response = await axios.post(`${API_BASE_URL.Dev}/auth/refresh`, { token: refreshToken });
            const { token: newToken, refreshToken: newRefreshToken } = response.data;
            setToken(newToken);
            setRefreshToken(newRefreshToken);
            setIsAuthenticatedState(true);
            const decodedToken: any = jwtDecode(newToken);
            setUserRole(decodedToken.role);
            setUserGivenName(decodedToken.given_name);
            setUserEmail(decodedToken.email);
        } catch (error) {
            console.error('Failed to refresh token', error);
            logout();
        }
    };

    const isAuthenticated = () => {
        const token = getToken();
        if (!token) return false;

        try {
            const decodedToken: any = jwtDecode(token);
            return decodedToken.exp * 1000 > Date.now();
        } catch (error) {
            console.error('Failed to decode token', error);
            return false;
        }
    };

    const hasRole = (role: string) => {
        return userRole === role;
    };

    return (
        <AuthContext.Provider value={{
            login,
            register,
            logout,
            isAuthenticated: isAuthenticated(),
            hasRole,
            getToken,
            userRole,
            userGivenName,
            userEmail
        }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
