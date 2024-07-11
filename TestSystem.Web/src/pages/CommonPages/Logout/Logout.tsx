import React, {useEffect} from 'react';
import {useAuth} from 'contexts/AuthContext.tsx';
import {useNavigate} from 'react-router-dom';
import {Spinner} from '@nextui-org/react'; // Assuming you are using Next UI

const Logout: React.FC = () => {
    const { logout } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        logout();
        setTimeout(() => {
            navigate('/login');
        }, 1500); // Redirect after 1.5 seconds
    }, [logout, navigate]);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gradient-to-r from-purple-400 via-pink-500 to-red-500">
            <h1 className="text-xl mb-4">Logging out...</h1>
            <Spinner size="lg" color="primary" />
        </div>
    );
};

export default Logout;
