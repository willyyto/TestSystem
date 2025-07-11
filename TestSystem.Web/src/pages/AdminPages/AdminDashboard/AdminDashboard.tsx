import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import {
    Card,
    CardBody,
    CardHeader,
    Button,
    Chip,
    Progress,
    Skeleton
} from '@heroui/react'
import {
    TrendingUp,
    Users,
    FileText,
    BarChart3,
    Clock,
    CheckCircle,
    AlertCircle,
    Calendar,
    ArrowRight,
    Plus,
    Trophy,
    Activity
} from 'lucide-react'
import { useQuery } from '@tanstack/react-query'
import { apiQuery } from 'libs/api'
import { useAuth } from '../../../auth/AuthContext.tsx'
import { formatDateD, formatRelativeTime } from 'utils/utils'
import { Helmet } from 'react-helmet-async'

interface DashboardStats {
    totalTests: number
    activeTests: number
    totalUsers: number
    totalAttempts: number
    recentAttempts: number
    averageScore: number
    recentActivity: Array<{
        activityType: string
        description: string
        timestamp: string
        userId?: string
        userName?: string
    }>
}

interface TestResult {
    id: string
    testName: string
    score: number
    completedDate: string
    passed: boolean
}

interface UpcomingTest {
    id: string
    name: string
    endDate: string
    description?: string
}

// Stats Card Component
interface StatsCardProps {
    title: string
    value: string | number
    icon: React.ComponentType<{ className?: string }>
    color: 'blue' | 'green' | 'purple' | 'orange'
    loading?: boolean
    change?: {
        value: number
        trend: 'up' | 'down'
    }
}

const StatsCard = ({ title, value, icon: Icon, color, loading = false, change }: StatsCardProps) => {
    const colorClasses = {
        blue: 'bg-blue-100 dark:bg-blue-900/20 text-blue-600 dark:text-blue-400',
        green: 'bg-green-100 dark:bg-green-900/20 text-green-600 dark:text-green-400',
        purple: 'bg-purple-100 dark:bg-purple-900/20 text-purple-600 dark:text-purple-400',
        orange: 'bg-orange-100 dark:bg-orange-900/20 text-orange-600 dark:text-orange-400',
    }

    if (loading) {
        return (
            <Card>
                <CardBody className="p-6">
                    <div className="flex items-center justify-between">
                        <div className="space-y-2">
                            <Skeleton className="h-4 w-20 rounded" />
                            <Skeleton className="h-8 w-16 rounded" />
                        </div>
                        <Skeleton className="h-12 w-12 rounded-lg" />
                    </div>
                </CardBody>
            </Card>
        )
    }

    return (
        <Card className="hover:shadow-lg transition-shadow duration-200">
            <CardBody className="p-6">
                <div className="flex items-center justify-between">
                    <div>
                        <p className="text-sm font-medium text-gray-600 dark:text-gray-400">
                            {title}
                        </p>
                        <p className="text-2xl font-bold text-gray-900 dark:text-white mt-1">
                            {value}
                        </p>
                        {change && (
                            <div className={`flex items-center gap-1 text-xs mt-2 ${
                                change.trend === 'up'
                                    ? "text-green-600 dark:text-green-400"
                                    : "text-red-600 dark:text-red-400"
                            }`}>
                <span>
                  {change.trend === 'up' ? '↗' : '↘'} {Math.abs(change.value)}%
                </span>
                                <span className="text-gray-500">vs last month</span>
                            </div>
                        )}
                    </div>
                    <div className={`flex h-12 w-12 items-center justify-center rounded-lg ${colorClasses[color]}`}>
                        <Icon className="h-6 w-6" />
                    </div>
                </div>
            </CardBody>
        </Card>
    )
}

// Quick Actions Component
const QuickActions = () => {
    const navigate = useNavigate()

    const actions = [
        {
            title: 'Create Test',
            description: 'Build a new assessment',
            icon: Plus,
            color: 'primary' as const,
            action: () => navigate('/tests/create')
        },
        {
            title: 'View Analytics',
            description: 'Check performance metrics',
            icon: BarChart3,
            color: 'secondary' as const,
            action: () => navigate('/analytics')
        },
        {
            title: 'Manage Users',
            description: 'Add or edit users',
            icon: Users,
            color: 'success' as const,
            action: () => navigate('/users')
        },
        {
            title: 'Generate Report',
            description: 'Export test results',
            icon: FileText,
            color: 'warning' as const,
            action: () => navigate('/reports')
        }
    ]

    return (
        <Card>
            <CardHeader>
                <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                    Quick Actions
                </h3>
            </CardHeader>
            <CardBody>
                <div className="grid grid-cols-2 gap-3">
                    {actions.map((action, index) => (
                        <Button
                            key={index}
                            variant="flat"
                            color={action.color}
                            className="h-16 flex-col justify-center"
                            onPress={action.action}
                        >
                            <action.icon className="h-5 w-5 mb-1" />
                            <span className="text-xs font-medium">{action.title}</span>
                        </Button>
                    ))}
                </div>
            </CardBody>
        </Card>
    )
}

// Recent Activity Component
interface RecentActivityProps {
    activities: DashboardStats['recentActivity']
    loading: boolean
}

const RecentActivity = ({ activities, loading }: RecentActivityProps) => {
    const getActivityIcon = (type: string) => {
        switch (type) {
            case 'test_completed':
                return <CheckCircle className="h-4 w-4 text-green-600" />
            case 'test_created':
                return <Plus className="h-4 w-4 text-blue-600" />
            case 'user_registered':
                return <Users className="h-4 w-4 text-purple-600" />
            default:
                return <Activity className="h-4 w-4 text-gray-600" />
        }
    }

    return (
        <Card>
            <CardHeader className="flex flex-row items-center justify-between">
                <div>
                    <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                        Recent Activity
                    </h3>
                    <p className="text-sm text-gray-600 dark:text-gray-400">
                        Latest system activities
                    </p>
                </div>
                <Button
                    variant="ghost"
                    size="sm"
                    endContent={<ArrowRight className="h-4 w-4" />}
                >
                    View All
                </Button>
            </CardHeader>
            <CardBody>
                {loading ? (
                    <div className="space-y-4">
                        {[...Array(3)].map((_, i) => (
                            <div key={i} className="flex items-center gap-3">
                                <Skeleton className="h-8 w-8 rounded-full" />
                                <div className="flex-1 space-y-2">
                                    <Skeleton className="h-4 w-3/4 rounded" />
                                    <Skeleton className="h-3 w-1/2 rounded" />
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    <div className="space-y-4">
                        {activities.map((activity, index) => (
                            <div key={index} className="flex items-start gap-3">
                                <div className="flex h-8 w-8 items-center justify-center rounded-full bg-gray-100 dark:bg-gray-800">
                                    {getActivityIcon(activity.activityType)}
                                </div>
                                <div className="flex-1 min-w-0">
                                    <p className="text-sm font-medium text-gray-900 dark:text-white">
                                        {activity.description}
                                    </p>
                                    <p className="text-xs text-gray-600 dark:text-gray-400">
                                        {formatRelativeTime(activity.timestamp)}
                                    </p>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </CardBody>
        </Card>
    )
}

// Test Overview Component
const TestOverview = () => {
    const [timeRange, setTimeRange] = useState('7d')

    return (
        <Card>
            <CardHeader className="flex flex-row items-center justify-between">
                <div>
                    <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                        Test Performance Overview
                    </h3>
                    <p className="text-sm text-gray-600 dark:text-gray-400">
                        Test completion and success rates
                    </p>
                </div>
                <div className="flex gap-2">
                    {['7d', '30d', '90d'].map((range) => (
                        <Button
                            key={range}
                            size="sm"
                            variant={timeRange === range ? 'solid' : 'flat'}
                            color={timeRange === range ? 'primary' : 'default'}
                            onPress={() => setTimeRange(range)}
                        >
                            {range}
                        </Button>
                    ))}
                </div>
            </CardHeader>
            <CardBody>
                <div className="space-y-6">
                    {/* Performance Metrics */}
                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <div className="flex items-center justify-between">
                <span className="text-sm font-medium text-gray-600 dark:text-gray-400">
                  Completion Rate
                </span>
                                <span className="text-sm font-bold text-gray-900 dark:text-white">
                  87%
                </span>
                            </div>
                            <Progress value={87} color="success" className="max-w-full" />
                        </div>

                        <div className="space-y-2">
                            <div className="flex items-center justify-between">
                <span className="text-sm font-medium text-gray-600 dark:text-gray-400">
                  Pass Rate
                </span>
                                <span className="text-sm font-bold text-gray-900 dark:text-white">
                  73%
                </span>
                            </div>
                            <Progress value={73} color="primary" className="max-w-full" />
                        </div>
                    </div>

                    {/* Top Performing Tests */}
                    <div>
                        <h4 className="text-sm font-medium text-gray-900 dark:text-white mb-3">
                            Top Performing Tests
                        </h4>
                        <div className="space-y-3">
                            {[
                                { name: 'JavaScript Fundamentals', score: 92, attempts: 45 },
                                { name: 'React Components', score: 88, attempts: 32 },
                                { name: 'Database Design', score: 85, attempts: 28 }
                            ].map((test, index) => (
                                <div key={index} className="flex items-center justify-between p-3 rounded-lg bg-gray-50 dark:bg-gray-800">
                                    <div className="flex items-center gap-3">
                                        <div className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-100 dark:bg-blue-900/20">
                                            <Trophy className="h-4 w-4 text-blue-600 dark:text-blue-400" />
                                        </div>
                                        <div>
                                            <p className="text-sm font-medium text-gray-900 dark:text-white">
                                                {test.name}
                                            </p>
                                            <p className="text-xs text-gray-600 dark:text-gray-400">
                                                {test.attempts} attempts
                                            </p>
                                        </div>
                                    </div>
                                    <Chip color="success" variant="flat" size="sm">
                                        {test.score}%
                                    </Chip>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </CardBody>
        </Card>
    )
}

// Main Dashboard Component
const AdminDashboard = () => {
    const { user } = useAuth()
    const navigate = useNavigate()

    const { data: stats, isLoading: statsLoading } = useQuery({
        queryKey: ['dashboard-stats'],
        queryFn: () => apiQuery<DashboardStats>('/analytics/dashboard'),
    })

    const { data: recentTests, isLoading: testsLoading } = useQuery({
        queryKey: ['recent-test-results'],
        queryFn: () => apiQuery<TestResult[]>('/user/usertestresult'),
        select: (data) => data.slice(0, 5), // Only show last 5 results
    })

    const { data: upcomingTests, isLoading: upcomingLoading } = useQuery({
        queryKey: ['upcoming-tests'],
        queryFn: () => apiQuery<UpcomingTest[]>('/user/tests/upcoming'),
    })

    const isAdmin = user?.role === 'Administrator'
    const isManager = user?.role === 'Manager'

    return (
        <>
            <Helmet>
                <title>Dashboard - TestSystem</title>
                <meta name="description" content="TestSystem Dashboard - Overview of your testing activities" />
            </Helmet>

            <div className="space-y-6">
                {/* Header */}
                <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                    <div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
                            Welcome back, {user?.name}
                        </h1>
                        <p className="text-gray-600 dark:text-gray-400">
                            {formatDateD(new Date())}
                        </p>
                    </div>

                    {(isAdmin || isManager) && (
                        <Button
                            color="primary"
                            startContent={<Plus className="h-4 w-4" />}
                            onPress={() => navigate('/tests/create')}
                        >
                            Create Test
                        </Button>
                    )}
                </div>

                {/* Stats Overview */}
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                    <StatsCard
                        title="Total Tests"
                        value={stats?.totalTests || 0}
                        icon={FileText}
                        color="blue"
                        loading={statsLoading}
                        change={{ value: 12, trend: 'up' }}
                    />
                    <StatsCard
                        title="Active Tests"
                        value={stats?.activeTests || 0}
                        icon={CheckCircle}
                        color="green"
                        loading={statsLoading}
                        change={{ value: 8, trend: 'up' }}
                    />
                    <StatsCard
                        title="Total Users"
                        value={stats?.totalUsers || 0}
                        icon={Users}
                        color="purple"
                        loading={statsLoading}
                    />
                    <StatsCard
                        title="Average Score"
                        value={`${stats?.averageScore?.toFixed(1) || 0}%`}
                        icon={TrendingUp}
                        color="orange"
                        loading={statsLoading}
                        change={{ value: 3, trend: 'up' }}
                    />
                </div>

                {/* Quick Actions */}
                {(isAdmin || isManager) && <QuickActions />}

                {/* Main Content Grid */}
                <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                    {/* Test Overview */}
                    <div className="lg:col-span-2">
                        <TestOverview />
                    </div>

                    {/* Recent Activity */}
                    <div>
                        <RecentActivity
                            activities={stats?.recentActivity || []}
                            loading={statsLoading}
                        />
                    </div>
                </div>

                {/* Recent Test Results */}
                {!testsLoading && recentTests && recentTests.length > 0 && (
                    <Card>
                        <CardHeader className="flex flex-row items-center justify-between">
                            <div>
                                <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                                    Recent Test Results
                                </h3>
                                <p className="text-sm text-gray-600 dark:text-gray-400">
                                    Your latest test performance
                                </p>
                            </div>
                            <Button
                                variant="ghost"
                                size="sm"
                                endContent={<ArrowRight className="h-4 w-4" />}
                                onPress={() => navigate('/tests/results')}
                            >
                                View All
                            </Button>
                        </CardHeader>
                        <CardBody>
                            <div className="space-y-3">
                                {recentTests.map((test) => (
                                    <div
                                        key={test.id}
                                        className="flex items-center justify-between p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors"
                                    >
                                        <div className="flex items-center gap-3">
                                            <div className={`p-2 rounded-full ${
                                                test.passed
                                                    ? 'bg-green-100 dark:bg-green-900/20'
                                                    : 'bg-red-100 dark:bg-red-900/20'
                                            }`}>
                                                {test.passed ? (
                                                    <CheckCircle className="h-4 w-4 text-green-600 dark:text-green-400" />
                                                ) : (
                                                    <AlertCircle className="h-4 w-4 text-red-600 dark:text-red-400" />
                                                )}
                                            </div>
                                            <div>
                                                <p className="font-medium text-gray-900 dark:text-white">
                                                    {test.testName}
                                                </p>
                                                <p className="text-sm text-gray-600 dark:text-gray-400">
                                                    {formatDateD(new Date(test.completedDate), 'MMM dd, yyyy')}
                                                </p>
                                            </div>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <Chip
                                                color={test.passed ? 'success' : 'danger'}
                                                variant="flat"
                                                size="sm"
                                            >
                                                {test.score}%
                                            </Chip>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </CardBody>
                    </Card>
                )}

                {/* Upcoming Tests */}
                {!upcomingLoading && upcomingTests && upcomingTests.length > 0 && (
                    <Card>
                        <CardHeader>
                            <div className="flex items-center gap-2">
                                <Calendar className="h-5 w-5 text-blue-600 dark:text-blue-400" />
                                <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                                    Upcoming Tests
                                </h3>
                            </div>
                        </CardHeader>
                        <CardBody>
                            <div className="space-y-3">
                                {upcomingTests.map((test) => (
                                    <div
                                        key={test.id}
                                        className="flex items-center justify-between p-3 rounded-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors cursor-pointer"
                                        onClick={() => navigate(`/tests/${test.id}`)}
                                    >
                                        <div className="flex items-center gap-3">
                                            <div className="p-2 rounded-full bg-blue-100 dark:bg-blue-900/20">
                                                <Clock className="h-4 w-4 text-blue-600 dark:text-blue-400" />
                                            </div>
                                            <div>
                                                <p className="font-medium text-gray-900 dark:text-white">
                                                    {test.name}
                                                </p>
                                                <p className="text-sm text-gray-600 dark:text-gray-400">
                                                    Due: {formatDateD(new Date(test.endDate), 'MMM dd, yyyy')}
                                                </p>
                                            </div>
                                        </div>
                                        <Button
                                            size="sm"
                                            color="primary"
                                            variant="flat"
                                        >
                                            Take Test
                                        </Button>
                                    </div>
                                ))}
                            </div>
                        </CardBody>
                    </Card>
                )}
            </div>
        </>
    )
}

export default AdminDashboard