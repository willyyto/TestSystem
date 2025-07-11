import { useEffect } from 'react'
import { setupActivityTracking } from './AuthStore.ts'
import { AUTH_CONFIG } from 'auth/config'

interface ActivityTrackerProps {
    enabled?: boolean
    timeoutMinutes?: number
}

export const ActivityTracker = ({
                                    enabled = AUTH_CONFIG.ACTIVITY_TRACKING_ENABLED,
                                    timeoutMinutes = AUTH_CONFIG.INACTIVITY_TIMEOUT_MINUTES
                                }: ActivityTrackerProps) => {
    useEffect(() => {
        if (!enabled) return

        console.log(`Setting up activity tracking with ${timeoutMinutes}min timeout`)

        // Setup activity tracking with cleanup
        const cleanup = setupActivityTracking(timeoutMinutes)

        return cleanup
    }, [enabled, timeoutMinutes])

    return null // This component doesn't render anything
}
