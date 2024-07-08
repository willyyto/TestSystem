export interface Test {
    id: string;
    title: string;
    questions: Question[];
    isActive: boolean;
    company: string
    endDate?: string;
}

export interface Question {
    id: string;
    text: string;
    type: string;
    answers: Answer[];
}

export interface Answer {
    id: string;
    text: string;
    isCorrect: boolean;
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
    attemptDate: string;
    score: number;
    test: Test;
    questionResults: QuestionResult[];
}

export interface CreateQuestion {
    id: string;
    text: string;
    type: string;
    options: string[];
    correctOption: string;
}