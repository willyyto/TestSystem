import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from 'contexts/AuthContext';

interface ProtectedRouteProps {
    children: React.ReactNode;
    roles?: string[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, roles }) => {
    const { isAuthenticated, hasRole, userRole } = useAuth();

    if (userRole === null) {
        // Render a loading state or spinner until userRole is set
        return <div>Loading...</div>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" />;
    }

    if (roles && !roles.some(role => hasRole(role))) {
        return <Navigate to="/unauthorised" />;
    }

    return <>{children}</>;
};

export default ProtectedRoute;
