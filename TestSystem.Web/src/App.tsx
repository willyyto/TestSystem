import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from '@/services/AuthContext';
import LoginForm from '@/components/LoginForm';
import RegisterForm from '@/components/RegisterForm';
import PrivateRoute from '@/navigation/PrivateRoute';
import Dashboard from '@/pages/Dashboard'; // Ensure this matches your structure

const App: React.FC = () => {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/login" element={<LoginForm />} />
                    <Route path="/register" element={<RegisterForm />} />
                    <Route
                        path="/dashboard"
                        element={<PrivateRoute element={<Dashboard />} />}
                    />
                </Routes>
            </Router>
        </AuthProvider>
    );
};

export default App;
