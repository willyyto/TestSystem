export interface Test {
    id: string;
    title: string;
    questions: Question[];
    endDate?: string;
}

export interface Question {
    id: string;
    text: string;
    type: string;
    answers: string[];
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