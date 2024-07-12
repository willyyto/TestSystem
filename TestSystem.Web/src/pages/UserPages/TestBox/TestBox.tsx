import React, { useEffect, useState } from 'react';
import { Button, Card, CardHeader, RadioGroup, Textarea, Input, Select, SelectItem } from '@nextui-org/react';
import { useParams } from 'react-router-dom';
import { CustomTestRadio } from 'components/Test/CustomTestRadio';
import apiService from 'contexts/UserApiService';
import { Test, Question } from 'types/Interfaces';

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
    const [timeLeft, setTimeLeft] = useState<number>(0);
    const [timerActive, setTimerActive] = useState<boolean>(false);
    const [showInstructions, setShowInstructions] = useState<boolean>(true);

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
        setAnswers({
            ...answers,
            [questionId]: event.target.value,
        });
    };

    const handleTextareaChange = (questionId: string, event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setAnswers({
            ...answers,
            [questionId]: event.target.value,
        });
    };

    const handleSelectChange = (questionId: string, pairId: string, value: string) => {
        setAnswers({
            ...answers,
            [`${questionId}-${pairId}`]: value,
        });
    };

    const handleNextQuestion = () => {
        setCurrentQuestionIndex(currentQuestionIndex + 1);
    };

    const handleStartTest = () => {
        setShowInstructions(false);
        setTimerActive(true);
    };

    const handleSubmit = async () => {
        try {
            const response = await apiService.submitTest(test?.id as string, answers);
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
                <CardHeader>
                    <h2 className="text-2xl font-bold mb-4">{test.name}</h2>
                </CardHeader>
                <div className="container mx-auto p-4">
                    <h4 className="text-xl mb-4">Instructions:</h4>
                    <p>{test.instructions}</p>
                    <Button color="primary" onClick={handleStartTest}>Start Test</Button>
                </div>
            </Card>
        );
    }

    const currentQuestion = shuffledQuestions[currentQuestionIndex];

    return (
        <Card radius="sm" fullWidth className="p-4">
            <CardHeader className="flex justify-between">
                <p>
                    Question <b>{currentQuestionIndex + 1}</b> of {shuffledQuestions.length}
                </p>
                <p className="text-right">Time left: {formatTime(timeLeft)}</p>
            </CardHeader>
            <div className="container mx-auto p-4">
                <h2 className="text-2xl font-bold mb-4">{test.name}</h2>
                <h4 className="text-xl mb-4">{currentQuestion.text}</h4>

                {currentQuestion.type === 'MultipleChoice' && (
                    <RadioGroup
                        value={answers[currentQuestion.id] || ''}
                        onChange={(event) => handleAnswerChange(currentQuestion.id, event)}
                    >
                        {currentQuestion.answers.map((answer) => (
                            <CustomTestRadio key={answer.id} value={answer.id}>
                                {answer.text}
                            </CustomTestRadio>
                        ))}
                    </RadioGroup>
                )}

                {currentQuestion.type === 'TrueFalse' && (
                    <RadioGroup
                        value={answers[currentQuestion.id] || ''}
                        onChange={(event) => handleAnswerChange(currentQuestion.id, event)}
                    >
                        <CustomTestRadio value="True">
                            True
                        </CustomTestRadio>
                        <CustomTestRadio value="False">
                            False
                        </CustomTestRadio>
                    </RadioGroup>
                )}

                {currentQuestion.type === 'ShortAnswer' && (
                    <Textarea
                        placeholder="Your answer"
                        value={answers[currentQuestion.id] || ''}
                        onChange={(e) => handleTextareaChange(currentQuestion.id, e)}
                    />
                )}

                {currentQuestion.type === 'FillInTheBlank' && (
                    <Input
                        placeholder="Your answer"
                        value={answers[currentQuestion.id] || ''}
                        onChange={(e) => handleAnswerChange(currentQuestion.id, e)}
                    />
                )}

                {currentQuestion.type === 'Matching' && (
                    <div>
                        {currentQuestion.matchPairs && currentQuestion.matchPairs.map((pair) => (
                            <div key={pair.id} className="mb-4">
                                <p>{pair.leftItem}</p>
                                <Select
                                    placeholder="Select match"
                                    value={answers[`${currentQuestion.id}-${pair.id}`] || ''}
                                    onChange={(e) => handleSelectChange(currentQuestion.id, pair.id, e.target.value)}
                                    className="max-w-xs"
                                >
                                    {currentQuestion.matchPairs.map((optionPair) => (
                                        <SelectItem key={optionPair.id} value={optionPair.rightItem}>
                                            {optionPair.rightItem}
                                        </SelectItem>
                                    ))}
                                </Select>
                            </div>
                        ))}
                    </div>
                )}

                <div className="mt-6">
                    {currentQuestionIndex < shuffledQuestions.length - 1 ? (
                        <Button color="primary" onClick={handleNextQuestion}>Next</Button>
                    ) : (
                        <Button color="success" onClick={handleSubmit}>Submit</Button>
                    )}
                </div>
            </div>
        </Card>
    );
};

export default TestBox;
