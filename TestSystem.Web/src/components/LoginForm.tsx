import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { Button } from 'primereact/button';
import AuthContext from '@/services/AuthContext';

const LoginForm: React.FC = () => {
    const { login } = useContext(AuthContext)!;
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            await login(username, password);
            navigate('/dashboard');
        } catch (error) {
            console.error('Login failed', error);
        }
    };

    return (
        <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="p-6 bg-white shadow-md rounded-md w-full max-w-md">
                <h2 className="text-3xl mb-6 text-center">Login</h2>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Username</label>
                    <InputText value={username} onChange={(e) => setUsername(e.target.value)} className="w-full p-inputtext-sm" />
                </div>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Password</label>
                    <Password value={password} onChange={(e) => setPassword(e.target.value)} className="w-full p-inputtext-sm" toggleMask />
                </div>
                <Button label="Login" icon="pi pi-sign-in" className="w-full p-button-raised p-button-primary" onClick={handleLogin} />
            </div>
        </div>
    );
};

export default LoginForm;
