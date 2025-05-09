import React, { useEffect, useState } from 'react';
import { Button, Card, CardHeader, Input, RadioGroup, Select, SelectItem, Textarea } from '@heroui/react';
import { useParams } from 'react-router-dom';
import { CustomTestRadio } from 'components/Test/CustomTestRadio';
import apiService from 'contexts/UserApiService';
import { Question, Test } from 'types/Interfaces';

const shuffleArray = (array: any[]) => {
    return array.sort(() => Math.random() - 0.5);
};

const parseDuration = (duration: string) => {
    const [hours, minutes, seconds] = duration.split(':').map(Number);
    return (hours * 3600) + (minutes * 60) + seconds;
};

const formatTime = (seconds: number) => {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${remainingSeconds < 10 ? `0${remainingSeconds}` : remainingSeconds}`;
};

const TestBox: React.FC = () => {
    const { testId } = useParams<{ testId: string }>();
    const [test, setTest] = useState<Test | null>(null);
    const [shuffledQuestions, setShuffledQuestions] = useState<Question[]>([]);
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
    const [answers, setAnswers] = useState<{ [key: string]: string }>({});
    const [matchingAnswers, setMatchingAnswers] = useState<{ [key: string]: { [leftItemId: string]: string } }>({});
    const [feedback, setFeedback] = useState<{ [key: string]: boolean }>({});
    const [timeLeft, setTimeLeft] = useState<number>(0);
    const [timerActive, setTimerActive] = useState<boolean>(false);
    const [showInstructions, setShowInstructions] = useState<boolean>(true);
    const [showFeedback, setShowFeedback] = useState<boolean>(false);

    useEffect(() => {
        const fetchUserTest = async () => {
            try {
                const data = await apiService.fetchUserTestById(testId);
                if (data.shuffleQuestions) {
                    data.questions = shuffleArray(data.questions);
                }
                setTest(data);
                setShuffledQuestions(data.questions);
                setTimeLeft(parseDuration(data.duration)); // Parse duration correctly
            } catch (error) {
                console.error("Error fetching test:", error);
            }
        };

        fetchUserTest();
    }, [testId]);

    const handleAnswerChange = (questionId: string, event: React.ChangeEvent<HTMLInputElement>) => {
        const answerValue = event.target.value;
        setAnswers({
            ...answers,
            [questionId]: answerValue,
        });
    };

    const handleTextareaChange = (questionId: string, event: React.ChangeEvent<HTMLTextAreaElement>) => {
        const answerValue = event.target.value;
        setAnswers({
            ...answers,
            [questionId]: answerValue,
        });
    };

    const handleSelectChange = (questionId: string, leftItemId: string, value: string) => {
        setMatchingAnswers({
            ...matchingAnswers,
            [questionId]: {
                ...matchingAnswers[questionId],
                [leftItemId]: value,
            },
        });
    };

    const evaluateFeedback = (questionId: string) => {
        const currentQuestion = shuffledQuestions.find(q => q.id === questionId);
        if (!currentQuestion) return;

        let isCorrect = false;
        if (currentQuestion.type === 'MultipleChoice' || currentQuestion.type === 'TrueFalse') {
            isCorrect = currentQuestion.answers.some(answer => answer.id === answers[questionId] && answer.isCorrect);
        } else if (currentQuestion.type === 'ShortAnswer' || currentQuestion.type === 'FillInTheBlank' || currentQuestion.type === 'Essay') {
            isCorrect = currentQuestion.answers.some(answer => answer.text === answers[questionId] && answer.isCorrect);
        } else if (currentQuestion.type === 'Matching') {
            isCorrect = currentQuestion.matchPairs.every(pair =>
                matchingAnswers[questionId]?.[pair.leftItemId] === pair.rightItemId
            );
        }

        setFeedback({
            ...feedback,
            [questionId]: isCorrect,
        });
    };

    const handleNextQuestion = () => {
        const questionId = shuffledQuestions[currentQuestionIndex].id;
        if (!showFeedback && test?.feedback === 'Immediate') {
            evaluateFeedback(questionId);
            setShowFeedback(true);
        } else {
            setShowFeedback(false);
            setCurrentQuestionIndex(currentQuestionIndex + 1);
        }
    };

    const handleStartTest = () => {
        setShowInstructions(false);
        setTimerActive(true);
    };

    const handleSubmit = async () => {
        try {
            const payload = {
                testId: test?.id,
                answers: answers,
                matchingAnswers: matchingAnswers
            };

            const response = await apiService.submitTest(payload);
            alert(response.message);
            window.location.href = '/dashboard';
        } catch (error) {
            console.error("Error submitting test:", error);
        }
    };


    useEffect(() => {
        if (timerActive && timeLeft > 0) {
            const timer = setInterval(() => {
                setTimeLeft(prevTimeLeft => prevTimeLeft - 1);
            }, 1000);
            return () => clearInterval(timer);
        } else if (timeLeft === 0 && timerActive) {
            handleSubmit();
        }
    }, [timeLeft, timerActive]);

    if (!test) return <p>Loading...</p>;

    if (showInstructions) {
        return (
            <Card radius="sm" fullWidth className="p-4">
                <CardHeader className="flex justify-between">
                    <h2 className="text-2xl font-bold mb-4">{test.name}</h2>
                    <h2 className="text-right">Total Time: <b>{formatTime(timeLeft)}</b></h2>
                </CardHeader>
                <div className="container mx-auto p-4">
                    <h4 className="text-xl mb-4">Instructions:</h4>
                    <p className="mb-4 mt-2">{test.instructions}</p>
                    <Button color="primary" onClick={handleStartTest}>Start Test</Button>
                </div>
            </Card>
        );
    }

    const currentQuestion = shuffledQuestions[currentQuestionIndex];
    const currentQuestionId = currentQuestion.id;
    const currentFeedback = feedback[currentQuestionId];

    return (
        <Card radius="sm" fullWidth className="p-4">
            <CardHeader className="flex justify-between">
                <p>
                    Question <b>{currentQuestionIndex + 1}</b> of {shuffledQuestions.length}
                </p>
                <h2 className="text-right">Time left: <b>{formatTime(timeLeft)}</b></h2>
            </CardHeader>
            <div className="container mx-auto p-4">
                <h2 className="text-2xl font-bold mb-4">{test.name}</h2>
                <h4 className="text-xl mb-4">{currentQuestion.text}</h4>

                {currentQuestion.type === 'MultipleChoice' && (
                    <RadioGroup
                        value={answers[currentQuestionId] || ''}
                        isReadOnly={showFeedback}
                        isDisabled={showFeedback}
                        onChange={(event) => handleAnswerChange(currentQuestionId, event)}
                    >
                        {currentQuestion.answers.map((answer) => (
                            <CustomTestRadio key={answer.id} value={answer.id} className={showFeedback && currentFeedback !== undefined && answer.isCorrect ? 'border-green-500' : ''}>
                                {answer.text}
                            </CustomTestRadio>
                        ))}
                    </RadioGroup>
                )}

                {currentQuestion.type === 'TrueFalse' && (
                    <RadioGroup
                        value={answers[currentQuestionId] || ''}
                        isReadOnly={showFeedback}
                        isDisabled={showFeedback}
                        onChange={(event) => handleAnswerChange(currentQuestionId, event)}
                    >
                        {currentQuestion.answers.map((answer) => (
                            <CustomTestRadio key={answer.id} value={answer.id} className={showFeedback && currentFeedback !== undefined && answer.isCorrect ? 'border-green-500' : ''}>
                                {answer.text}
                            </CustomTestRadio>
                        ))}
                    </RadioGroup>
                )}

                {currentQuestion.type === 'ShortAnswer' && (
                    <Textarea
                        placeholder="Your answer"
                        value={answers[currentQuestionId] || ''}
                        onChange={(e) => handleTextareaChange(currentQuestionId, e)}
                        className={showFeedback && currentFeedback !== undefined && currentQuestion.answers.find(a => a.text === answers[currentQuestionId] && a.isCorrect) ? 'border-green-500' : ''}
                    />
                )}

                {currentQuestion.type === 'Essay' && (
                    <Textarea
                        placeholder="Your answer"
                        value={answers[currentQuestionId] || ''}
                        onChange={(e) => handleTextareaChange(currentQuestionId, e)}
                        className={showFeedback && currentFeedback !== undefined && currentQuestion.answers.find(a => a.text === answers[currentQuestionId] && a.isCorrect) ? 'border-green-500' : ''}
                    />
                )}

                {currentQuestion.type === 'FillInTheBlank' && (
                    <Input
                        placeholder="Your answer"
                        value={answers[currentQuestionId] || ''}
                        onChange={(e) => handleAnswerChange(currentQuestionId, e)}
                        className={showFeedback && currentFeedback !== undefined && currentQuestion.answers.find(a => a.text === answers[currentQuestionId] && a.isCorrect) ? 'border-green-500' : ''}
                    />
                )}

                {currentQuestion.type === 'Matching' && (
                    <div>
                        {currentQuestion.matchPairs && currentQuestion.matchPairs.map((pair) => (
                            <div key={pair.id} className="mb-4">
                                <p>{pair.leftItem}</p>
                                <Select
                                    placeholder="Select match"
                                    value={matchingAnswers[currentQuestionId]?.[pair.leftItemId] || ''}
                                    onChange={(e) => handleSelectChange(currentQuestionId, pair.leftItemId, e.target.value)}
                                    className="max-w-xs"
                                    isReadOnly={showFeedback}
                                    isDisabled={showFeedback}
                                >
                                    {currentQuestion.matchPairs.map((optionPair) => (
                                        <SelectItem key={optionPair.rightItemId} value={optionPair.rightItemId}>
                                            {optionPair.rightItem}
                                        </SelectItem>
                                    ))}
                                </Select>
                            </div>
                        ))}
                    </div>
                )}

                {test?.feedback === 'Immediate' && showFeedback && currentFeedback !== undefined && (
                    <div className={`mt-4 ${currentFeedback ? 'text-green-500' : 'text-red-500'}`}>
                        {currentFeedback ? 'Correct!' : 'Incorrect'}
                    </div>
                )}

                <div className="mt-6">
                    {currentQuestionIndex < shuffledQuestions.length - 1 ? (
                        <Button color="primary" onClick={handleNextQuestion}>Next</Button>
                    ) : (
                        showFeedback ? (
                            <Button color="success" onClick={handleSubmit}>Submit</Button>
                        ) : (
                            <Button color="primary" onClick={() => {
                                evaluateFeedback(currentQuestionId);
                                setShowFeedback(true);
                            }}>Next</Button>
                        )
                    )}
                </div>
            </div>
        </Card>
    );
};

export default TestBox;
