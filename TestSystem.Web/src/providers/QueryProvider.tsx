// providers/QueryProvider.tsx
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'

// Enhanced Query Client with better auth error handling
const createQueryClient = () => {
    return new QueryClient({
        defaultOptions: {
            queries: {
                retry: (failureCount, error: any) => {
                    // Don't retry on auth errors
                    if (error?.response?.status === 401) {
                        console.log('Auth error detected, not retrying query')
                        return false
                    }
                    if (error?.response?.status === 403) {
                        console.log('Forbidden error, not retrying query')
                        return false
                    }
                    // Retry other errors up to 3 times
                    return failureCount < 3
                },
                refetchOnWindowFocus: false,
                staleTime: 5 * 60 * 1000, // 5 minutes
                cacheTime: 10 * 60 * 1000, // 10 minutes
            },
            mutations: {
                retry: (failureCount, error: any) => {
                    // Don't retry auth-related mutations
                    if (error?.response?.status === 401 || error?.response?.status === 403) {
                        return false
                    }
                    return false // Don't retry mutations by default
                },
            },
        },
    })
}

interface QueryProviderProps {
    children: React.ReactNode
    showDevtools?: boolean
}

export const QueryProvider = ({
                                  children,
                                  showDevtools = process.env.NODE_ENV === 'development'
                              }: QueryProviderProps) => {
    const queryClient = createQueryClient()

    return (
        <QueryClientProvider client={queryClient}>
            {children}
            {showDevtools && (
                <ReactQueryDevtools initialIsOpen={false} />
            )}
        </QueryClientProvider>
    )
}