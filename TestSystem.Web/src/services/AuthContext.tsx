import React, {createContext, ReactNode, useEffect, useState} from 'react';
import axios from 'axios';

axios.defaults.baseURL = 'https://localhost:44395'; // Set the correct backend API URL

interface AuthContextType {
    user: any;
    role: string | null;
    login: (username: string, password: string) => Promise<void>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({children}) => {
    const [user, setUser] = useState<any>(null);
    const [role, setRole] = useState<string | null>(null);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            axios.get('/api/user').then(response => {
                setUser(response.data.user);
                setRole(response.data.role);
            }).catch(() => {
                ``
                logout();
            });
        }
    }, []);

    const login = async (username: string, password: string) => {
        const response = await axios.post('/api/auth/login', {username, password});
        localStorage.setItem('token', response.data.token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
        setUser(response.data.user);
        setRole(response.data.role);
    };

    const logout = () => {
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
        setUser(null);
        setRole(null);
        ``
    };

    return (
        <AuthContext.Provider value={{user, role, login, logout}}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;
