import api, { ApiResponse } from 'libs/api'

// Types
interface Test {
    id: string
    name: string
    company: string
    description?: string
    instructions?: string
    startDate: string
    endDate: string
    duration: string
    passMark: number
    isTimed: boolean
    shuffleQuestions: boolean
    maximumAttempts: number
    visibility: string
    testType: string
    feedback: string
    testAccessControl: string
    gradingScheme: string
    retakePolicy: any
    showProgressBar: boolean
    allowBackNavigation: boolean
    showQuestionNumbers: boolean
    autoSubmit: boolean
    requirePassword: boolean
    showResultsImmediately: boolean
    showCorrectAnswers: boolean
    showScorePercentage: boolean
    emailResults: boolean
    welcomeMessage?: string
    completionMessage?: string
    failureMessage?: string
    isPublic: boolean
    inviteCode?: string
    availableFrom?: string
    availableUntil?: string
    randomQuestionCount?: number
    randomizeFromPool: boolean
    preventCopyPaste: boolean
    fullScreenMode: boolean
    disableRightClick: boolean
    trackTabSwitches: boolean
    maxTabSwitches: number
    requireWebcam: boolean
    requireMicrophone: boolean
    enableScreenRecording: boolean
    isScheduled: boolean
    schedules: any[]
    questions: any[]
    isArchived: boolean
    isActive: boolean
}

export interface TestResult {
    id: string
    testName: string
    score: number
    completedDate: string
    passed: boolean
}

export interface TestAttempt {
    id: string
    testId: string
    userId: string
    startedAt: string
    completedAt?: string
    timeSpent?: string
    isCompleted: boolean
    isAbandoned: boolean
    attemptNumber: number
    tabSwitchCount: number
}

export interface PagedResult<T> {
    items: T[]
    totalCount: number
    page: number
    pageSize: number
    totalPages: number
}

export interface TestSearchParams {
    searchTerm?: string
    testTypes?: string[]
    statuses?: string[]
    companyId?: string
    createdAfter?: string
    createdBefore?: string
    tags?: string[]
    sortBy?: string
    sortDirection?: string
    page?: number
    pageSize?: number
}

export interface TestSubmission {
    testId: string
    answers: Record<string, string>
    matchingAnswers: Record<string, Record<string, string>>
    orderingAnswers: Record<string, string[]>
    numericalAnswers: Record<string, number>
    scaleAnswers: Record<string, number>
    multipleSelectAnswers: Record<string, string[]>
    fileSubmissions: Record<string, string>
    questionTimes: Record<string, string>
    password?: string
}

export interface CreateTestRequest {
    name: string
    description?: string
    instructions?: string
    companyId: string
    startDate: string
    endDate: string
    duration: string
    passMark: number
    isTimed: boolean
    shuffleQuestions: boolean
    maximumAttempts: number
    visibility: string
    testType: string
    feedback: string
    testAccessControl: string
    gradingScheme: string
    retakePolicy: any
    showProgressBar: boolean
    allowBackNavigation: boolean
    showQuestionNumbers: boolean
    autoSubmit: boolean
    requirePassword: boolean
    password?: string
    showResultsImmediately: boolean
    showCorrectAnswers: boolean
    showScorePercentage: boolean
    emailResults: boolean
    welcomeMessage?: string
    completionMessage?: string
    failureMessage?: string
    isPublic: boolean
    availableFrom?: string
    availableUntil?: string
    randomQuestionCount?: number
    randomizeFromPool: boolean
    disableCopyPaste: boolean
    fullScreenMode: boolean
    disableRightClick: boolean
    trackTabSwitches: boolean
    maxTabSwitches: number
    requireWebcam: boolean
    requireMicrophone: boolean
    enableScreenRecording: boolean
}

export interface TestAnalytics {
    testId: string
    testName: string
    totalAttempts: number
    completedAttempts: number
    passedAttempts: number
    averageScore: number
    passRate: number
    averageCompletionTime: string
    questionAnalytics: any[]
}

export const testsService = {
    // Get all tests (admin/manager)
    getTests: async (params?: TestSearchParams): Promise<PagedResult<Test>> => {
        const searchParams = new URLSearchParams()
        if (params) {
            Object.entries(params).forEach(([key, value]) => {
                if (value !== undefined && value !== null) {
                    if (Array.isArray(value)) {
                        value.forEach(v => searchParams.append(key, v))
                    } else {
                        searchParams.append(key, String(value))
                    }
                }
            })
        }

        const response = await api.get<ApiResponse<PagedResult<Test>>>(`/admin/admintest?${searchParams.toString()}`)
        return response.data.data
    },

    // Get available tests for user
    getAvailableTests: async (): Promise<Test[]> => {
        const response = await api.get<ApiResponse<Test[]>>('/user/usertest/available')
        return response.data.data
    },

    // Get test by ID
    getTest: async (id: string, password?: string): Promise<Test> => {
        const url = password
            ? `/user/test/${id}?password=${encodeURIComponent(password)}`
            : `/user/test/${id}`
        const response = await api.get<ApiResponse<Test>>(url)
        return response.data.data
    },

    // Get test for admin/manager
    getTestAdmin: async (id: string): Promise<Test> => {
        const response = await api.get<ApiResponse<Test>>(`/admin/admintest/${id}`)
        return response.data.data
    },

    // Create test
    createTest: async (data: CreateTestRequest): Promise<Test> => {
        const response = await api.post<ApiResponse<Test>>('/admin/admintest', data)
        return response.data.data
    },

    // Update test
    updateTest: async (id: string, data: CreateTestRequest): Promise<Test> => {
        const response = await api.put<ApiResponse<Test>>(`/admin/admintest/${id}`, data)
        return response.data.data
    },

    // Delete test
    deleteTest: async (id: string): Promise<void> => {
        await api.delete(`/admin/test/${id}`)
    },

    // Duplicate test
    duplicateTest: async (id: string, newName: string, targetCompanyId?: string): Promise<void> => {
        await api.post(`/admin/test/${id}/duplicate`, { newName, targetCompanyId })
    },

    // Start test attempt
    startTest: async (id: string, password?: string): Promise<TestAttempt> => {
        const response = await api.post<ApiResponse<TestAttempt>>(`/user/usertest/${id}/start`, { password })
        return response.data.data
    },

    // Submit test
    submitTest: async (id: string, submission: TestSubmission): Promise<TestResult> => {
        const response = await api.post<ApiResponse<TestResult>>(`/user/usertest/${id}/submit`, submission)
        return response.data.data
    },

    // Save test progress
    saveProgress: async (attemptId: string, answers: Record<string, string>): Promise<void> => {
        await api.post(`/user/usertest/attempts/${attemptId}/save-progress`, answers)
    },

    // Abandon test
    abandonTest: async (attemptId: string): Promise<void> => {
        await api.post(`/user/usertest/attempts/${attemptId}/abandon`)
    },

    // Get test results for user
    getUserTestResults: async (): Promise<TestResult[]> => {
        const response = await api.get<ApiResponse<TestResult[]>>('/user/usertestresult')
        return response.data.data
    },

    // Get specific test result
    getTestResult: async (id: string): Promise<TestResult> => {
        const response = await api.get<ApiResponse<TestResult>>(`/user/usertestresult/${id}`)
        return response.data.data
    },

    // Get test analytics
    getTestAnalytics: async (id: string): Promise<TestAnalytics> => {
        const response = await api.get<ApiResponse<TestAnalytics>>(`/admin/admintest/${id}/analytics`)
        return response.data.data
    },

    // Export test results
    exportTestResults: async (id: string, format = 'csv'): Promise<Blob> => {
        const response = await api.get(`/admin/admintest/${id}/export?format=${format}`, {
            responseType: 'blob'
        })
        return response.data
    },

    // Generate certificate
    generateCertificate: async (resultId: string): Promise<string> => {
        const response = await api.post<ApiResponse<string>>(`/user/usertestresult/${resultId}/certificate`)
        return response.data.data
    },

    // Bulk operations
    bulkUpdateTestStatus: async (testIds: string[], isActive: boolean): Promise<void> => {
        await api.put('/admin/admintest/bulk-status', { testIds, isActive })
    },
}