import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import axios from 'axios';
import {InputText} from 'primereact/inputtext';
import {Password} from 'primereact/password';
import {Button} from 'primereact/button';

const RegisterForm: React.FC = () => {
    const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const navigate = useNavigate();

    const handleRegister = async () => {
        try {
            await axios.post('/api/auth/register', {name, username, password, email, role: 'User'});
            navigate('/login');
        } catch (error) {
            console.error('Registration failed', error);
        }
    };

    return (
        <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="p-6 bg-white shadow-md rounded-md w-full max-w-md">
                <h2 className="text-3xl mb-6 text-center">Register</h2>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Name</label>
                    <InputText value={name} onChange={(e) => setName(e.target.value)}
                               className="w-full p-inputtext-sm"/>
                </div>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Username</label>
                    <InputText value={username} onChange={(e) => setUsername(e.target.value)}
                               className="w-full p-inputtext-sm"/>
                </div>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Email</label>
                    <InputText value={email} onChange={(e) => setEmail(e.target.value)}
                               className="w-full p-inputtext-sm"/>
                </div>
                <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Password</label>
                    <Password value={password} onChange={(e) => setPassword(e.target.value)}
                              className="w-full p-inputtext-sm" toggleMask/>
                </div>
                <Button label="Register" icon="pi pi-user-plus" className="w-full p-button-raised p-button-primary"
                        onClick={handleRegister}/>
            </div>
        </div>
    );
};

export default RegisterForm;
