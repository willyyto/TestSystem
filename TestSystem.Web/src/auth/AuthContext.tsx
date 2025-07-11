import { createContext, useContext, useEffect, ReactNode, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuthStore } from './AuthStore.ts'
import { useQuery } from '@tanstack/react-query'
import { apiQuery } from '../libs/api.ts'
import { User } from '../types/Interfaces.ts'
import { jwtDecode } from 'jwt-decode'
import api from '../libs/api.ts'
import {addToast} from "@heroui/react";

interface DecodedToken {
    exp: number
    role: string
    given_name: string
    email: string
    sub: string
    [key: string]: any
}

interface AuthContextType {
    user: User | null
    isAuthenticated: boolean
    isLoading: boolean
    login: (user: User, token: string, refreshToken: string) => void
    hasRole: (role: string) => boolean
    userRole: string | null
    userGivenName: string | null
    userEmail: string | null
    getToken: () => string | null
    getRefreshToken: () => string | null
    refreshAccessToken: () => Promise<void>
    logout: () => void
    logoutEverywhere: () => Promise<void>
    updateUser: (user: Partial<User>) => void
    isTokenExpired: () => boolean
    getTokenExpirationTime: () => Date | null
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export const useAuth = () => {
    const context = useContext(AuthContext)
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider')
    }
    return context
}

interface AuthProviderProps {
    children: ReactNode
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const navigate = useNavigate()
    const {
        user,
        token,
        refreshToken,
        isAuthenticated,
        isLoading,
        login,
        logout: storeLogout,
        updateUser,
        setLoading
    } = useAuthStore()

    // Fetch user profile if we have a token but no user data
    const { data: userProfile } = useQuery({
        queryKey: ['profile'],
        queryFn: () => apiQuery<User>('/user/profile'),
        enabled: !!token && !user,
        retry: false,
    })

    // Update user data if we fetched it
    useEffect(() => {
        if (userProfile && !user) {
            updateUser(userProfile)
        }
    }, [userProfile, user, updateUser])

    // Token validation and refresh logic
    const isTokenExpired = useCallback((): boolean => {
        if (!token) return true

        try {
            const decodedToken: DecodedToken = jwtDecode(token)
            return decodedToken.exp * 1000 < Date.now()
        } catch (error) {
            console.error('Failed to decode token', error)
            return true
        }
    }, [token])

    const getTokenExpirationTime = useCallback((): Date | null => {
        if (!token) return null

        try {
            const decodedToken: DecodedToken = jwtDecode(token)
            return new Date(decodedToken.exp * 1000)
        } catch (error) {
            console.error('Failed to decode token', error)
            return null
        }
    }, [token])

    // Refresh access token
    const refreshAccessToken = useCallback(async (): Promise<void> => {
        if (!refreshToken) {
            console.warn('No refresh token available')
            logout()
            return
        }

        try {
            setLoading(true)
            console.log('Refreshing access token...')

            const response = await api.post('/auth/refresh', {
                token: refreshToken
            })

            if (!response.data.success) {
                throw new Error(response.data.message || 'Token refresh failed')
            }

            const { token: newToken, refreshToken: newRefreshToken } = response.data.data

            if (!newToken || !newRefreshToken) {
                throw new Error('Invalid refresh response: missing tokens')
            }

            // Get user profile with new token if we don't have user data
            let userData = user
            if (!userData) {
                try {
                    const profileResponse = await api.get('/user/profile', {
                        headers: {
                            Authorization: `Bearer ${newToken}`
                        }
                    })
                    if (profileResponse.data.success) {
                        userData = profileResponse.data.data
                    }
                } catch (profileError) {
                    console.error('Failed to fetch user profile after refresh', profileError)
                }
            }

            // Update auth state with new tokens and user data
            if (userData) {
                login(userData, newToken, newRefreshToken)
                console.log('Token refreshed successfully')
            } else {
                // If we can't get user data, at least update the tokens
                useAuthStore.getState().setTokens(newToken, newRefreshToken)
            }

        } catch (error: any) {
            console.error('Failed to refresh token', error)

            // If refresh fails, log out the user
            logout()

            // Show error message
            addToast({title: 'Session expired. Please log in again.', color: "danger"})

            throw error
        } finally {
            setLoading(false)
        }
    }, [refreshToken, user, login, setLoading])

    // Regular logout
    const logout = useCallback(() => {
        console.log('Logging out user...')

        // Clear auth state
        storeLogout()

        // Show success message
        addToast({title: 'Logged out successfully', color: "success"})

        // Navigate to login page
        navigate('/login', { replace: true })
    }, [storeLogout, navigate])

    // Logout from all devices
    const logoutEverywhere = useCallback(async (): Promise<void> => {
        try {
            setLoading(true)
            console.log('Logging out from all devices...')

            // Call API to invalidate all tokens for this user
            if (token) {
                await api.post('/auth/logout-all', {}, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
            }

            // Clear local auth state
            storeLogout()
            addToast({title: 'Logged out from all devices successfully', color: "success"})

            // Navigate to login page
            navigate('/login', { replace: true })

        } catch (error: any) {
            console.error('Failed to logout from all devices', error)
            addToast({title: 'Failed to logout from all devices', color: "danger"})
            // Even if API call fails, clear local state
            storeLogout()
            navigate('/login', { replace: true })

            addToast({title: 'Logged out locally. Some sessions may still be active.', color: "danger"})
        } finally {
            setLoading(false)
        }
    }, [token, storeLogout, navigate, setLoading])

    // Auto-refresh token when it's about to expire
    useEffect(() => {
        if (!token || !refreshToken || !isAuthenticated) return

        const checkTokenExpiration = () => {
            if (isTokenExpired()) {
                console.log('Token expired, attempting refresh...')
                refreshAccessToken().catch((error) => {
                    console.error('Auto-refresh failed', error)
                })
            }
        }

        // Check immediately
        checkTokenExpiration()

        // Set up interval to check every 5 minutes
        const interval = setInterval(checkTokenExpiration, 5 * 60 * 1000)

        return () => clearInterval(interval)
    }, [token, refreshToken, isAuthenticated, isTokenExpired, refreshAccessToken])

    // Proactively refresh token when it's close to expiring (5 minutes before)
    useEffect(() => {
        if (!token || !refreshToken || !isAuthenticated) return

        const scheduleRefresh = () => {
            const expirationTime = getTokenExpirationTime()
            if (!expirationTime) return

            const refreshTime = expirationTime.getTime() - Date.now() - (5 * 60 * 1000) // 5 minutes before expiry

            if (refreshTime > 0) {
                console.log(`Token will be refreshed in ${Math.round(refreshTime / 1000 / 60)} minutes`)

                const timeoutId = setTimeout(() => {
                    console.log('Proactively refreshing token...')
                    refreshAccessToken().catch((error) => {
                        console.error('Proactive refresh failed', error)
                    })
                }, refreshTime)

                return () => clearTimeout(timeoutId)
            }
        }

        return scheduleRefresh()
    }, [token, refreshToken, isAuthenticated, getTokenExpirationTime, refreshAccessToken])

    // Check if token exists on mount and validate it
    useEffect(() => {
        if (token && !isAuthenticated) {
            setLoading(true)

            if (isTokenExpired()) {
                console.log('Stored token is expired, attempting refresh...')
                refreshAccessToken()
                    .catch(() => {
                        console.log('Refresh failed, clearing stored tokens')
                        logout()
                    })
            } else {
                // Token is valid, validate it with the server
                apiQuery<User>('/user/profile')
                    .then((userData) => {
                        if (userData) {
                            login(userData, token, refreshToken || '')
                        }
                    })
                    .catch(() => {
                        console.log('Token validation failed, attempting refresh...')
                        refreshAccessToken()
                            .catch(() => {
                                logout()
                            })
                    })
                    .finally(() => {
                        setLoading(false)
                    })
            }
        }
    }, [token, isAuthenticated, login, logout, setLoading, refreshAccessToken, isTokenExpired])

    // Role management functions
    const hasRole = useCallback((role: string): boolean => {
        if (!user || !user.role) return false
        // Case-insensitive role comparison
        return user.role.toLowerCase() === role.toLowerCase()
    }, [user])

    const userRole = user?.role || null
    const userGivenName = user?.name || null
    const userEmail = user?.email || null

    // Get token functions
    const getToken = useCallback((): string | null => {
        return token
    }, [token])

    const getRefreshToken = useCallback((): string | null => {
        return refreshToken
    }, [refreshToken])

    const value: AuthContextType = {
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
    }

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}