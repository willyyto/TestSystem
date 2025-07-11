import { addToast } from '@heroui/react'
import axios, { AxiosInstance, AxiosError } from 'axios'
import { useAuthStore } from '../auth/AuthStore.ts'

// Create axios instance
const api: AxiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5220/api',
    timeout: 30000,
    headers: {
        'Content-Type': 'application/json',
    },
})

// Request interceptor to add auth token
api.interceptors.request.use(
    (config) => {
        const token = useAuthStore.getState().token
        if (token) {
            config.headers.Authorization = `Bearer ${token}`
        }
        return config
    },
    (error) => {
        return Promise.reject(error)
    }
)

// Response interceptor to handle token refresh
api.interceptors.response.use(
    (response) => {
        return response
    },
    async (error: AxiosError) => {
        const originalRequest = error.config as any

        // Handle 401 errors (token expired)
        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true

            const refreshToken = useAuthStore.getState().refreshToken

            if (refreshToken) {
                try {
                    const response = await axios.post('/auth/refresh', {
                        token: useAuthStore.getState().token,
                        refreshToken: refreshToken,
                    })

                    const { token: newToken, refreshToken: newRefreshToken } = response.data.data

                    // Update tokens in store
                    useAuthStore.getState().login(
                        useAuthStore.getState().user!,
                        newToken,
                        newRefreshToken
                    )

                    // Retry original request with new token
                    originalRequest.headers.Authorization = `Bearer ${newToken}`
                    return api(originalRequest)
                } catch (refreshError) {
                    // Refresh failed, logout user
                    useAuthStore.getState().logout()
                    window.location.href = '/auth/login'
                    return Promise.reject(refreshError)
                }
            } else {
                // No refresh token, logout user
                useAuthStore.getState().logout()
                window.location.href = '/auth/login'
            }
        }

        // Handle other errors
        if (error.response?.status >= 500) {
            addToast({title: "500", description: "Server error. Please try again later.", color: "danger",})
        } else if (error.response?.status === 404) {
            addToast({title: "404", description: "Server error. Please try again later.", color: "danger",})
        } else if (error.response?.status === 403) {
            addToast({title: "403", description: "Server error. Please try again later.", color: "danger",})
        } else if (error.response?.status === 400) {
            const errorMessage = error.response.data?.message || 'Invalid request.'
            addToast({title: "400", description: errorMessage, color: "danger",})
        } else if (error.code === 'ECONNABORTED') {
            addToast({title: "ECONNABORTED", description: "Request timeout. Please try again.", color: "danger",})
        } else if (!error.response) {
            addToast({title: "ECONNABORTED", description: "Network error. Please check your connection.", color: "danger",})
        }

        return Promise.reject(error)
    }
)

export default api

// Generic API response type
export interface ApiResponse<T = any> {
    success: boolean
    data: T
    message?: string
    errors?: string[]
    statusCode?: number
}

// Utility function to handle API errors
export const handleApiError = (error: any): string => {
    if (error.response?.data?.message) {
        return error.response.data.message
    }
    if (error.response?.data?.errors?.length > 0) {
        return error.response.data.errors.join(', ')
    }
    if (error.message) {
        return error.message
    }
    return 'An unexpected error occurred'
}

// Generic query function for React Query
export const apiQuery = async <T>(url: string): Promise<T> => {
    const response = await api.get<ApiResponse<T>>(url)
    return response.data.data
}

// Generic mutation function for React Query
export const apiMutation = async <T, D = any>(
    url: string,
    data?: D,
    method: 'POST' | 'PUT' | 'DELETE' = 'POST'
): Promise<T> => {
    const response = await api.request<ApiResponse<T>>({
        url,
        method,
        data,
    })
    return response.data.data
}