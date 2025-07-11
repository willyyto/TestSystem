import React from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from 'auth/AuthContext'
import { LoadingSpinner } from "../components/common/LoadingSpinner"
import AppRoutes from '../navigation/AppRoutes'

interface ProtectedRouteProps {
    children: React.ReactNode
    roles?: string[]
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, roles }) => {
    const location = useLocation()
    const {
        isAuthenticated,
        isLoading,
        hasRole,
        userRole,
        isTokenExpired,
        refreshAccessToken
    } = useAuth()

    // Show loading while userRole is being determined
    if (userRole === null && isAuthenticated) {
        return <LoadingSpinner text="Loading user data..." />
    }

    // Show loading while checking authentication
    if (isLoading) {
        return <LoadingSpinner text="Authenticating..." />
    }

    // Check if token is expired and try to refresh
    if (isAuthenticated && isTokenExpired()) {
        console.log('Token expired in ProtectedRoute, attempting refresh...')

        // Attempt to refresh token
        refreshAccessToken().catch(() => {
            console.log('Token refresh failed, user will be logged out')
        })

        // Show loading while refreshing
        return <LoadingSpinner text="Refrehsing Session..." />
    }

    // Redirect to login if not authenticated
    if (!isAuthenticated) {
        return <Navigate to={AppRoutes.login} state={{ from: location }} replace />
    }

    // Check role requirements (keeping your existing logic)
    if (roles && !roles.some(role => hasRole(role))) {
        console.log(`Access denied. User role: ${userRole}, Required roles: ${roles.join(', ')}`)
        return <Navigate to={AppRoutes.unauthorised} replace />
    }

    // All checks passed, render the protected content
    return <>{children}</>
}

export default ProtectedRoute