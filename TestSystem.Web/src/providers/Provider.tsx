// Provider.tsx - Enhanced but Simple
import { HeroUIProvider } from "@heroui/system"
import { useNavigate } from "react-router-dom"
import { AuthProvider } from '../auth/AuthContext.tsx'
import { HelmetProvider } from 'react-helmet-async'
import { ThemeProvider as NextThemesProvider } from "next-themes"
import { ErrorBoundary } from '../components/common/ErrorBoundary.tsx'

// Import our new organized components
import { QueryProvider } from './QueryProvider.tsx'
import { ActivityTracker } from '../auth/ActivityTracker.tsx'
import { AUTH_CONFIG, type AuthConfig } from '../auth/config.ts'
import {ToastProvider} from "@heroui/react";
import React from "react";

interface ProviderProps {
    children: React.ReactNode
    authConfig?: Partial<AuthConfig>
    showDevtools?: boolean
}

export default function Provider({
                                     children,
                                     authConfig = {},
                                     showDevtools = process.env.NODE_ENV === 'development'
                                 }: ProviderProps) {
    const navigate = useNavigate()

    // Merge auth configuration
    const mergedAuthConfig = { ...AUTH_CONFIG, ...authConfig }

    return (
        <ErrorBoundary>
            <HelmetProvider>
                <QueryProvider showDevtools={showDevtools}>
                    <AuthProvider>
                        <HeroUIProvider navigate={navigate}>
                            <NextThemesProvider attribute="class" defaultTheme="dark">
                                <ToastProvider placement="top-right" toastOffset={20} />

                                {/* Activity Tracker for session management */}
                                <ActivityTracker
                                    enabled={mergedAuthConfig.ACTIVITY_TRACKING_ENABLED}
                                    timeoutMinutes={mergedAuthConfig.INACTIVITY_TIMEOUT_MINUTES}
                                />

                                {children}
                            </NextThemesProvider>
                        </HeroUIProvider>
                    </AuthProvider>
                </QueryProvider>
            </HelmetProvider>
        </ErrorBoundary>
    )
}
