import React, { useContext } from 'react';
import AuthContext from '@/services/AuthContext';
import { Button } from 'primereact/button';

const Dashboard: React.FC = () => {
    const { role, logout } = useContext(AuthContext)!;

    return (
        <div className="p-6">
            <h1 className="text-3xl mb-6">Dashboard</h1>
            {role === 'Admin' ? (
                <div>
                    <h2 className="text-2xl mb-4">Admin Content</h2>
                    <p>Welcome, Admin! Here you can manage users, view reports, and configure system settings.</p>
                    {/* Add more admin-specific content here */}
                </div>
            ) : (
                <div>
                    <h2 className="text-2xl mb-4">User Content</h2>
                    <p>Welcome, User! Here you can view your quizzes, check your scores, and manage your profile.</p>
                    {/* Add more user-specific content here */}
                </div>
            )}
            <Button label="Logout" icon="pi pi-sign-out" className="mt-6" onClick={logout} />
        </div>
    );
};

export default Dashboard;
