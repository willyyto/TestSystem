// navigation/utils.ts
import { useNavigate } from 'react-router-dom'
import { useAuth } from 'auth/AuthContext'
import AppRoutes from './AppRoutes'

/**
 * Navigation utilities that work with your existing AppRoutes
 */
export const useAppNavigation = () => {
    const navigate = useNavigate()
    const { userRole, isAuthenticated } = useAuth()

    /**
     * Navigate to the appropriate dashboard based on user role
     */
    const goToDashboard = () => {
        if (!isAuthenticated) {
            navigate(AppRoutes.login)
            return
        }

        if (userRole === 'admin' || userRole === 'administrator') {
            navigate(AppRoutes.admindashboard)
        } else {
            navigate(AppRoutes.dashboard)
        }
    }

    /**
     * Navigate to login page
     */
    const goToLogin = () => {
        navigate(AppRoutes.login)
    }

    /**
     * Navigate to admin dashboard (if user has permission)
     */
    const goToAdmin = () => {
        if (userRole === 'admin' || userRole === 'administrator') {
            navigate(AppRoutes.admindashboard)
        } else {
            navigate(AppRoutes.unauthorised)
        }
    }

    /**
     * Build quiz URL with testId
     */
    const buildQuizUrl = (testId: string): string => {
        return AppRoutes.quiz.replace(':testId', testId)
    }

    /**
     * Navigate to quiz
     */
    const goToQuiz = (testId: string) => {
        navigate(buildQuizUrl(testId))
    }

    /**
     * Build result URL with resultId
     */
    const buildResultUrl = (resultId: string): string => {
        return AppRoutes.result.replace(':resultId', resultId)
    }

    /**
     * Navigate to result
     */
    const goToResult = (resultId: string) => {
        navigate(buildResultUrl(resultId))
    }

    /**
     * Go back in history
     */
    const goBack = () => {
        window.history.back()
    }

    /**
     * Navigate to logout
     */
    const goToLogout = () => {
        navigate(AppRoutes.logout)
    }

    return {
        // Navigation functions
        goToDashboard,
        goToLogin,
        goToAdmin,
        goToQuiz,
        goToResult,
        goToLogout,
        goBack,

        // URL builders
        buildQuizUrl,
        buildResultUrl,

        // Raw navigate function for custom navigation
        navigate,

        // Current user info for conditional navigation
        userRole,
        isAuthenticated,
    }
}

/**
 * Route utilities
 */
export const routeUtils = {
    /**
     * Check if current path is an admin route
     */
    isAdminRoute: (pathname: string): boolean => {
        return pathname.startsWith('/admin')
    },

    /**
     * Check if current path is a public route
     */
    isPublicRoute: (pathname: string): boolean => {
        const publicRoutes = [
            AppRoutes.login,
            AppRoutes.about,
            AppRoutes.docs,
            AppRoutes.blog,
            AppRoutes.pricing,
            AppRoutes.unauthorised
        ]
        return publicRoutes.includes(pathname)
    },

    /**
     * Get page title based on route
     */
    getPageTitle: (pathname: string): string => {
        const titles: Record<string, string> = {
            [AppRoutes.root]: 'Home',
            [AppRoutes.dashboard]: 'Dashboard',
            [AppRoutes.admindashboard]: 'Admin Dashboard',
            [AppRoutes.admintest]: 'Manage Tests',
            [AppRoutes.admincompany]: 'Manage Companies',
            [AppRoutes.adminuser]: 'Manage Users',
            [AppRoutes.adminuserview]: 'User Details',
            [AppRoutes.admintestcreate]: 'Create Test',
            [AppRoutes.login]: 'Login',
            [AppRoutes.logout]: 'Logout',
            [AppRoutes.unauthorised]: 'Access Denied',
            [AppRoutes.about]: 'About',
            [AppRoutes.docs]: 'Documentation',
            [AppRoutes.blog]: 'Blog',
            [AppRoutes.pricing]: 'Pricing',
        }

        // Handle dynamic routes
        if (pathname.startsWith('/quiz/')) {
            return 'Quiz'
        }
        if (pathname.startsWith('/result/')) {
            return 'Results'
        }

        return titles[pathname] || 'TestSystem'
    },
}

/**
 * Hook to get current route information
 */
export const useRouteInfo = () => {
    const { pathname } = window.location

    return {
        pathname,
        isAdminRoute: routeUtils.isAdminRoute(pathname),
        isPublicRoute: routeUtils.isPublicRoute(pathname),
        pageTitle: routeUtils.getPageTitle(pathname),
    }
}