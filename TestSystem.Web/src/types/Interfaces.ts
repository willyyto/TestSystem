// User types
export interface User {
    id: string
    username: string
    name: string
    email: string
    role: 'Administrator' | 'Manager' | 'User'
    firstName?: string
    lastName?: string
    profilePictureUrl?: string
    phone?: string
    department?: string
    jobTitle?: string
    lastLoginAt?: string
    emailVerified: boolean
    twoFactorEnabled: boolean
    timezone?: string
    language?: string
    notificationEmailEnabled: boolean
    notificationSmsEnabled: boolean
    company?: Company
    isArchived: boolean
    isActive: boolean
    isLocked: boolean
}

// Company types
export interface Company {
    id: string
    name: string
    description?: string
    website?: string
    logoUrl?: string
    address?: string
    city?: string
    state?: string
    country?: string
    postalCode?: string
    phone?: string
    email?: string
    contactPerson?: string
    subscriptionTier: string
    subscriptionStart?: string
    subscriptionEnd?: string
    maxUsers: number
    maxTests: number
    maxQuestionsPerTest: number
    customBrandingEnabled: boolean
    advancedReportsEnabled: boolean
    apiAccessEnabled: boolean
    storageLimitMB: number
    storageUsedMB: number
    customDomain?: string
    isActive: boolean
    isArchived: boolean
}

// Test types
export interface Test {
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
    retakePolicy: RetakePolicy
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
    schedules: TestSchedule[]
    questions: Question[]
    isArchived: boolean
    isActive: boolean
}

export interface RetakePolicy {
    allowRetakes: boolean
    maxRetakes: number
    retakeInterval: string
    requirePasswordForRetake: boolean
    resetProgressOnRetake: boolean
    showPreviousResults: boolean
    retakePenalty: number
}

export interface TestSchedule {
    id: string
    startDateTime: string
    endDateTime: string
    timeZone?: string
    isRecurring: boolean
    recurrencePattern?: string
    maxParticipants: number
}

// Question types
export interface Question {
    id: string
    text: string
    type: string
    weight: number
    timeLimit: number
    isRequired: boolean
    imageUrl?: string
    videoUrl?: string
    audioUrl?: string
    explanation?: string
    hint?: string
    displayOrder: number
    allowMultipleAnswers: boolean
    shuffleAnswers: boolean
    correctNumericalAnswer?: number
    numericalTolerance?: number
    numericalUnit?: string
    scaleMin?: number
    scaleMax?: number
    scaleMinLabel?: string
    scaleMaxLabel?: string
    allowedFileTypes?: string
    maxFileSizeKB?: number
    orderingInstructions?: string
    answers: Answer[]
    matchPairs: MatchPair[]
    orderingItems: OrderingItem[]
}

export interface Answer {
    id: string
    text: string
    isCorrect: boolean
    isFillInTheBlank: boolean
    imageUrl?: string
    explanation?: string
    points: number
    isCaseSensitive: boolean
    acceptableAnswers?: string
}

export interface MatchPair {
    id: string
    leftItemId: string
    leftItem: string
    rightItemId: string
    rightItem: string
}

export interface OrderingItem {
    id: string
    text: string
    correctOrder: number
}

// Test Result types
export interface TestResult {
    id: string
    userId: string
    testId: string
    completedDate: string
    score: number
    rawScore: number
    maxPossibleScore: number
    grade: string
    passed: boolean
    timeSpent: string
    questionsAnswered: number
    questionsCorrect: number
    questionsSkipped: number
    comments?: string
    isManuallyGraded: boolean
    gradedBy?: string
    gradedAt?: string
    certificateUrl?: string
    test: Test
    questionResults: QuestionResult[]
}

export interface QuestionResult {
    id: string
    questionId: string
    answer: string
    isCorrect: boolean
    pointsEarned: number
    maxPoints: number
    timeSpent?: string
    isSkipped: boolean
    requiresManualGrading: boolean
    instructorFeedback?: string
    fileSubmissionPath?: string
    question: Question
}

// Test Attempt types
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

// Analytics types
export interface TestAnalytics {
    testId: string
    testName: string
    totalAttempts: number
    completedAttempts: number
    passedAttempts: number
    averageScore: number
    passRate: number
    averageCompletionTime: string
    questionAnalytics: QuestionAnalytics[]
}

export interface QuestionAnalytics {
    questionId: string
    questionText: string
    questionType: string
    totalResponses: number
    correctResponses: number
    successRate: number
    averageTimeSpent: string
    answerAnalytics: AnswerAnalytics[]
}

export interface AnswerAnalytics {
    answerId: string
    answerText: string
    selectionCount: number
    selectionPercentage: number
    isCorrect: boolean
}

// Notification types
export interface Notification {
    id: string
    userId: string
    type: string
    title: string
    message: string
    isRead: boolean
    createdAt: string
    actionUrl?: string
}

// API Response types
export interface ApiResponse<T = any> {
    success: boolean
    data: T
    message?: string
    errors?: string[]
    statusCode?: number
}

export interface PagedResult<T> {
    items: T[]
    totalCount: number
    page: number
    pageSize: number
    totalPages: number
}

// Form types
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
    retakePolicy: RetakePolicy
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

export interface CreateQuestionRequest {
    testId: string
    text: string
    type: string
    weight: number
    timeLimit: number
    isRequired: boolean
    imageUrl?: string
    videoUrl?: string
    audioUrl?: string
    explanation?: string
    hint?: string
    displayOrder: number
    allowMultipleAnswers: boolean
    shuffleAnswers: boolean
    correctNumericalAnswer?: number
    numericalTolerance?: number
    numericalUnit?: string
    scaleMin?: number
    scaleMax?: number
    scaleMinLabel?: string
    scaleMaxLabel?: string
    allowedFileTypes?: string
    maxFileSizeKB?: number
    orderingInstructions?: string
    answers: CreateAnswerRequest[]
    matchPairs: CreateMatchPairRequest[]
    orderingItems: CreateOrderingItemRequest[]
}

export interface CreateAnswerRequest {
    text: string
    isCorrect: boolean
    isFillInTheBlank: boolean
    imageUrl?: string
    explanation?: string
    points: number
    isCaseSensitive: boolean
    acceptableAnswers?: string
}

export interface CreateMatchPairRequest {
    leftItem: string
    rightItem: string
}

export interface CreateOrderingItemRequest {
    text: string
    correctOrder: number
}

// Theme types
export type Theme = 'light' | 'dark' | 'system'