import { useCallback, useEffect, useState } from 'react'
import { useAuth } from './AuthContext.tsx'
import { setupActivityTracking, isSessionActive } from './AuthStore.ts'
import {addToast} from "@heroui/react";

interface SessionInfo {
    isActive: boolean
    lastActivity: Date | null
    timeUntilExpiry: number | null
    timeUntilInactiveLogout: number | null
}

interface AuthStatus {
    isAuthenticated: boolean
    isLoading: boolean
    hasToken: boolean
    tokenExpired: boolean
    sessionInfo: SessionInfo
}

/**
 * Advanced authentication hook with session management and auto-logout features
 */
export const useAuthAdvanced = () => {
    const {
        user,
        isAuthenticated,
        isLoading,
        hasRole,
        userRole,
        userGivenName,
        userEmail,
        getToken,
        getRefreshToken,
        refreshAccessToken,
        logout,
        logoutEverywhere,
        updateUser,
        login,
        isTokenExpired,
        getTokenExpirationTime,
    } = useAuth()

    const [sessionWarningShown, setSessionWarningShown] = useState(false)
    const [autoLogoutEnabled, setAutoLogoutEnabled] = useState(true)

    // Session management
    const getSessionInfo = useCallback((): SessionInfo => {
        const lastActivity = new Date() // This would come from your store
        const expirationTime = getTokenExpirationTime()

        return {
            isActive: isSessionActive(),
            lastActivity,
            timeUntilExpiry: expirationTime ? expirationTime.getTime() - Date.now() : null,
            timeUntilInactiveLogout: null, // Calculate based on your inactivity settings
        }
    }, [getTokenExpirationTime])

    const getAuthStatus = useCallback((): AuthStatus => {
        return {
            isAuthenticated,
            isLoading,
            hasToken: !!getToken(),
            tokenExpired: isTokenExpired(),
            sessionInfo: getSessionInfo(),
        }
    }, [isAuthenticated, isLoading, getToken, isTokenExpired, getSessionInfo])

    // Enhanced logout with confirmation
    const logoutWithConfirmation = useCallback(async (showConfirmation: boolean = true) => {
        if (showConfirmation) {
            const confirmed = window.confirm('Are you sure you want to log out?')
            if (!confirmed) return false
        }

        logout()
        return true
    }, [logout])

    // Logout from all devices with confirmation
    const logoutEverywhereWithConfirmation = useCallback(async (showConfirmation: boolean = true) => {
        if (showConfirmation) {
            const confirmed = window.confirm(
                'This will log you out from all devices. Are you sure you want to continue?'
            )
            if (!confirmed) return false
        }

        await logoutEverywhere()
        return true
    }, [logoutEverywhere])

    // Force token refresh
    const forceTokenRefresh = useCallback(async () => {
        try {
            await refreshAccessToken()
            addToast({title: 'Session refreshed successfully', color: "success"})
            return true
        } catch (error) {
            addToast({title: 'Failed to refresh session', color: "danger"})
            return false
        }
    }, [refreshAccessToken])

    // Check if user has any of the specified roles
    const hasAnyRole = useCallback((roles: string[]): boolean => {
        return roles.some(role => hasRole(role))
    }, [hasRole])

    // Check if user has all of the specified roles
    const hasAllRoles = useCallback((roles: string[]): boolean => {
        return roles.every(role => hasRole(role))
    }, [hasRole])

    // Permission-based helpers
    const permissions = {
        canAccessAdmin: () => hasRole('admin') || hasRole('administrator'),
        canManageUsers: () => hasAnyRole(['admin', 'administrator', 'manager']),
        canCreateTests: () => hasAnyRole(['admin', 'administrator', 'manager']),
        canViewReports: () => hasAnyRole(['admin', 'administrator', 'manager']),
        canEditProfile: () => isAuthenticated,
        canChangePassword: () => isAuthenticated,
    }

    // Session warning system
    useEffect(() => {
        if (!isAuthenticated || !autoLogoutEnabled) return

        const expirationTime = getTokenExpirationTime()
        if (!expirationTime) return

        const timeUntilExpiry = expirationTime.getTime() - Date.now()
        const warningTime = 5 * 60 * 1000 // 5 minutes before expiry

        if (timeUntilExpiry <= warningTime && timeUntilExpiry > 0 && !sessionWarningShown) {
            setSessionWarningShown(true)

            const showWarning = () => {
                const extendSession = window.confirm(
                    'Your session will expire soon. Do you want to extend it?'
                )

                if (extendSession) {
                    forceTokenRefresh().then((success) => {
                        if (success) {
                            setSessionWarningShown(false)
                        }
                    })
                } else {
                    logout()
                }
            }

            const warningTimeout = setTimeout(showWarning, Math.max(0, timeUntilExpiry - warningTime))

            return () => clearTimeout(warningTimeout)
        }
    }, [
        isAuthenticated,
        autoLogoutEnabled,
        getTokenExpirationTime,
        sessionWarningShown,
        forceTokenRefresh,
        logout
    ])

    // Activity tracking setup
    useEffect(() => {
        if (!isAuthenticated || !autoLogoutEnabled) return

        const cleanup = setupActivityTracking(60) // 60 minutes of inactivity

        return cleanup
    }, [isAuthenticated, autoLogoutEnabled])

    // Reset session warning when token is refreshed
    useEffect(() => {
        if (isAuthenticated && !isTokenExpired()) {
            setSessionWarningShown(false)
        }
    }, [isAuthenticated, isTokenExpired])

    // User profile helpers
    const getUserInfo = useCallback(() => {
        return {
            id: user?.id || null,
            username: user?.username || null,
            name: userGivenName,
            email: userEmail,
            role: userRole,
            isActive: user?.isActive || false,
            company: user?.company || null,
        }
    }, [user, userGivenName, userEmail, userRole])

    // Navigation helpers based on role
    const getDefaultRoute = useCallback(() => {
        if (!isAuthenticated) return '/login'

        if (hasRole('admin') || hasRole('administrator')) {
            return '/admin/dashboard'
        } else if (hasRole('manager')) {
            return '/manager/dashboard'
        } else {
            return '/dashboard'
        }
    }, [isAuthenticated, hasRole])

    return {
        // User data
        user,
        getUserInfo,

        // Authentication state
        isAuthenticated,
        isLoading,
        getAuthStatus,

        // Tokens
        getToken,
        getRefreshToken,
        isTokenExpired,
        getTokenExpirationTime,

        // Role management
        hasRole,
        hasAnyRole,
        hasAllRoles,
        userRole,
        userGivenName,
        userEmail,
        permissions,

        // Actions
        login,
        logout: logoutWithConfirmation,
        logoutEverywhere: logoutEverywhereWithConfirmation,
        forceLogout: logout, // Direct logout without confirmation
        updateUser,
        refreshAccessToken,
        forceTokenRefresh,

        // Session management
        getSessionInfo,
        autoLogoutEnabled,
        setAutoLogoutEnabled,

        // Navigation
        getDefaultRoute,

        // Utilities
        sessionWarningShown,
        setSessionWarningShown,
    }
}

// Custom hook for session monitoring
export const useSessionMonitor = () => {
    const { getTokenExpirationTime, isTokenExpired, refreshAccessToken } = useAuth()
    const [timeUntilExpiry, setTimeUntilExpiry] = useState<number | null>(null)

    useEffect(() => {
        const updateTimeUntilExpiry = () => {
            const expirationTime = getTokenExpirationTime()
            if (expirationTime) {
                const time = expirationTime.getTime() - Date.now()
                setTimeUntilExpiry(Math.max(0, time))
            } else {
                setTimeUntilExpiry(null)
            }
        }

        updateTimeUntilExpiry()
        const interval = setInterval(updateTimeUntilExpiry, 1000)

        return () => clearInterval(interval)
    }, [getTokenExpirationTime])

    const formatTimeUntilExpiry = (): string => {
        if (!timeUntilExpiry) return 'Unknown'

        const minutes = Math.floor(timeUntilExpiry / 1000 / 60)
        const seconds = Math.floor((timeUntilExpiry / 1000) % 60)

        if (minutes > 0) {
            return `${minutes}m ${seconds}s`
        } else {
            return `${seconds}s`
        }
    }

    return {
        timeUntilExpiry,
        formatTimeUntilExpiry,
        isTokenExpired: isTokenExpired(),
        refreshToken: refreshAccessToken,
    }
}