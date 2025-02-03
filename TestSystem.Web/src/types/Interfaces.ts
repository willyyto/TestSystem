import {AccessControl, FeedbackType, GradingScheme, QuestionType, TestType, TestVisibility} from "./Enum.ts";

export interface Test {
    id: string;
    name: string;
    company: string;
    questions: Question[];
    isActive: boolean;
    startDate: Date;
    endDate: Date;
    duration: number;
    passMark: number;
    isTimed: boolean;
    shuffleQuestions: boolean;
    maximumAttempts: number;
    visibility: TestVisibility;
    testType: TestType;
    instructions: string;
    feedback: FeedbackType;
    testAccessControl: AccessControl;
    gradingScheme: GradingScheme;
    retakePolicy: RetakePolicy;
}

export interface User {
    id: string;
    name: string;
    username: string;
    email: string;
    company: Company;
    role: string;
    isActive: boolean;
    isArchived: boolean;
    isLocked: boolean;
}

export interface Company {
    id: string;
    name: string;
    isActive: boolean;
}

export interface Question {
    id: string;
    text: string;
    type: QuestionType;
    answers: Answer[];
    matchPairs?: MatchPair[]; // Optional, only for Matching questions
    weight: number;
}

export interface Answer {
    id: string;
    text: string;
    isCorrect: boolean;
    isFillInTheBlank?: boolean; // Optional, only for FillInTheBlank answers
}

export interface MatchPair {
    id: string;
    leftItemId: string;
    leftItem: string;
    rightItemId: string;
    rightItem: string;
}

export interface QuestionResult {
    id: string;
    questionId: string;
    isCorrect: boolean;
    question: Question;
    answer: string;
}

export interface Result {
    id: string;
    userId: string;
    testId: string;
    completedDate: string;
    score: number;
    test: Test;
    questionResults: QuestionResult[];
}

export interface CreateQuestion {
    id: string;
    text: string;
    type: QuestionType;
    options: string[];
    correctOption: string;
}

export interface RetakePolicy {
    allowRetakes: boolean;
    maxRetakes: number;
    retakeInterval: number;
}

