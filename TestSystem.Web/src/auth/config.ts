// auth/config.ts
export const AUTH_CONFIG = {
    // Session timeouts
    INACTIVITY_TIMEOUT_MINUTES: 60,
    SESSION_WARNING_MINUTES: 5,

    // Auto-refresh settings
    AUTO_REFRESH_ENABLED: true,
    REFRESH_BEFORE_EXPIRY_MINUTES: 5,

    // Activity tracking
    ACTIVITY_TRACKING_ENABLED: true,

    // UI preferences
    SHOW_SESSION_COUNTDOWN: true,
    SHOW_REFRESH_NOTIFICATIONS: true,
}

export type AuthConfig = typeof AUTH_CONFIG