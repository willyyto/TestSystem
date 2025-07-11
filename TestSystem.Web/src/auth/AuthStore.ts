import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import { User } from '../types/Interfaces.ts'

interface AuthStore {
    user: User | null
    token: string | null
    refreshToken: string | null
    isAuthenticated: boolean
    isLoading: boolean
    lastActivity: number | null

    // Actions
    login: (user: User, token: string, refreshToken: string) => void
    logout: () => void
    updateUser: (user: Partial<User>) => void
    setLoading: (loading: boolean) => void
    setTokens: (token: string, refreshToken: string) => void
    updateLastActivity: () => void

    // Role-based helper methods
    hasRole: (role: string) => boolean
    isAdmin: () => boolean
    isManager: () => boolean
    isUser: () => boolean
    getUserRole: () => string | null
}

export const useAuthStore = create<AuthStore>()(
    persist(
        (set, get) => ({
            user: null,
            token: null,
            refreshToken: null,
            isAuthenticated: false,
            isLoading: false,
            lastActivity: null,

            login: (user: User, token: string, refreshToken: string) => {
                set({
                    user,
                    token,
                    refreshToken,
                    isAuthenticated: true,
                    isLoading: false,
                    lastActivity: Date.now(),
                })
            },

            logout: () => {
                set({
                    user: null,
                    token: null,
                    refreshToken: null,
                    isAuthenticated: false,
                    isLoading: false,
                    lastActivity: null,
                })
            },

            updateUser: (userData: Partial<User>) => {
                const currentUser = get().user
                if (currentUser) {
                    set({
                        user: { ...currentUser, ...userData },
                        lastActivity: Date.now(),
                    })
                }
            },

            setLoading: (loading: boolean) => {
                set({ isLoading: loading })
            },

            setTokens: (token: string, refreshToken: string) => {
                set({
                    token,
                    refreshToken,
                    lastActivity: Date.now(),
                })
            },

            updateLastActivity: () => {
                set({ lastActivity: Date.now() })
            },

            // Role-based helper methods
            hasRole: (role: string): boolean => {
                const user = get().user
                if (!user || !user.role) return false
                return user.role.toLowerCase() === role.toLowerCase()
            },

            isAdmin: (): boolean => {
                const user = get().user
                return user?.role?.toLowerCase() === 'administrator' ||
                    user?.role?.toLowerCase() === 'admin'
            },

            isManager: (): boolean => {
                const user = get().user
                return user?.role?.toLowerCase() === 'manager'
            },

            isUser: (): boolean => {
                const user = get().user
                return user?.role?.toLowerCase() === 'user'
            },

            getUserRole: (): string | null => {
                const user = get().user
                return user?.role || null
            },
        }),
        {
            name: 'auth-storage',
            // Only persist essential auth data
            partialize: (state) => ({
                user: state.user,
                token: state.token,
                refreshToken: state.refreshToken,
                isAuthenticated: state.isAuthenticated,
                lastActivity: state.lastActivity,
            }),
            // Optional: Add version for migration support
            version: 1,
            // Optional: Add migration logic for future updates
            migrate: (persistedState: any, version: number) => {
                if (version === 0) {
                    // Migration from version 0 to 1
                    return {
                        ...persistedState,
                        lastActivity: Date.now(),
                    }
                }
                return persistedState
            },
        }
    )
)

// Utility functions for external use
export const getStoredToken = () => useAuthStore.getState().token
export const getStoredRefreshToken = () => useAuthStore.getState().refreshToken
export const getStoredUser = () => useAuthStore.getState().user
export const isUserAuthenticated = () => useAuthStore.getState().isAuthenticated

// Session management utilities
export const getLastActivity = () => useAuthStore.getState().lastActivity
export const isSessionActive = (maxInactiveMinutes: number = 60): boolean => {
    const lastActivity = getLastActivity()
    if (!lastActivity) return false

    const inactiveTime = Date.now() - lastActivity
    const maxInactiveTime = maxInactiveMinutes * 60 * 1000

    return inactiveTime < maxInactiveTime
}

// Activity tracking for session management
export const trackActivity = () => {
    useAuthStore.getState().updateLastActivity()
}

// Auto-logout after inactivity
export const setupActivityTracking = (maxInactiveMinutes: number = 60) => {
    const events = ['mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart', 'click']

    const resetTimer = () => {
        trackActivity()
    }

    // Add event listeners
    events.forEach(event => {
        document.addEventListener(event, resetTimer, true)
    })

    // Check for inactivity every minute
    const inactivityChecker = setInterval(() => {
        if (!isSessionActive(maxInactiveMinutes)) {
            useAuthStore.getState().logout()
            // You might want to show a notification here
            console.log('User logged out due to inactivity')
        }
    }, 60 * 1000)

    // Cleanup function
    return () => {
        events.forEach(event => {
            document.removeEventListener(event, resetTimer, true)
        })
        clearInterval(inactivityChecker)
    }
}