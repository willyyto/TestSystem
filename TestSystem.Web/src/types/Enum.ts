export enum TestVisibility {
    Public = "Public",
    Private = "Private",
    Restricted = "Restricted",
}

export enum TestType {
    Quiz = "Quiz",
    Exam = "Exam",
    Survey = "Survey",
}

export enum FeedbackType {
    None = "None",
    Immediate = "Immediate",
    AfterCompletion = "AfterCompletion",
}

export enum AccessControl {
    Open = "Open",
    InviteOnly = "InviteOnly",
    PasswordProtected = "PasswordProtected",
}

export enum GradingScheme {
    PassFail = "PassFail",
    Percentage = "Percentage",
    LetterGrade = "LetterGrade",
}

export enum QuestionType {
    MultipleChoice = "MultipleChoice",
    TrueFalse = "TrueFalse",
    ShortAnswer = "ShortAnswer",
    Essay = "Essay",
    FillInTheBlank = "FillInTheBlank",
    Matching = "Matching",
}
