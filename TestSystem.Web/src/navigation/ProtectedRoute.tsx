import React from 'react';
import {Navigate} from 'react-router-dom';
import {useAuth} from 'contexts/AuthContext';
import {Progress} from "@nextui-org/react";

interface ProtectedRouteProps {
    children: React.ReactNode;
    roles?: string[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, roles }) => {
    const { isAuthenticated, hasRole, userRole } = useAuth();

    if (userRole === null) {
        // Render a loading state or spinner until userRole is set
        return <div className="flex items-center justify-center py-40">
            <div className="text-center">
                <Progress
                    size="lg"
                    isIndeterminate
                    aria-label="Loading..."
                    className="max-w-sm mx-auto"
                />
                <p className="mt-4 text-md">Loading...</p>
            </div>
        </div>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login"/>;
    }

    if (roles && !roles.some(role => hasRole(role))) {
        return <Navigate to="/unauthorised"/>;
    }

    return <>{children}</>;
};

export default ProtectedRoute;
