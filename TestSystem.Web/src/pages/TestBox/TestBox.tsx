import React, { useState, useEffect } from 'react';
import { Button, Card, CardHeader, RadioGroup, Textarea } from '@nextui-org/react';
import { useParams } from 'react-router-dom';
import { CustomRadio } from 'components/Test/CustomRadio';
import apiService from 'contexts/ApiService';
import {Test} from "types/Interfaces.ts";

const TestBox: React.FC = () => {
    const { testId } = useParams<{ testId: string }>();
    const [test, setTest] = useState<Test | null>(null);
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
    const [answers, setAnswers] = useState<{ [key: string]: string }>({});

    useEffect(() => {
        const fetchTest = async () => {
            try {
                const data = await apiService.fetchTestById(testId);
                setTest(data);
            } catch (error) {
                console.error("Error fetching test:", error);
            }
        };

        fetchTest();
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

    const handleNextQuestion = () => {
        setCurrentQuestionIndex(currentQuestionIndex + 1);
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

    if (!test) return <p>Loading...</p>;

    const currentQuestion = test.questions[currentQuestionIndex];

    return (
        <Card radius="sm" fullWidth className="p-4">
            <CardHeader>
                <p>
                    Question <b>{currentQuestionIndex + 1}</b> of {test.questions.length}
                </p>
            </CardHeader>
            <div className="container mx-auto p-4">
                <h2 className="text-2xl font-bold mb-4">{test.title}</h2>
                <h4 className="text-xl mb-4">{currentQuestion.text}</h4>

                {currentQuestion.type === 'MultipleChoice' && (
                    <RadioGroup
                        value={answers[currentQuestion.id] || ''}
                        onChange={(event) => handleAnswerChange(currentQuestion.id, event)}
                    >
                        {currentQuestion.answers.map((answer) => (
                            <CustomRadio key={answer.text} value={answer.text}>
                                {answer.text}
                            </CustomRadio>
                        ))}
                    </RadioGroup>
                )}

                {currentQuestion.type === 'TrueFalse' && (
                    <RadioGroup
                        value={answers[currentQuestion.id] || ''}
                        onChange={(event) => handleAnswerChange(currentQuestion.id, event)}
                    >
                        <CustomRadio description="" value="True">
                            True
                        </CustomRadio>
                        <CustomRadio description="" value="False">
                            False
                        </CustomRadio>
                    </RadioGroup>
                )}

                {currentQuestion.type === 'ShortAnswer' && (
                    <Textarea
                        placeholder="Your answer"
                        value={answers[currentQuestion.id] || ''}
                        onChange={(e) => handleTextareaChange(currentQuestion.id, e)}
                    />
                )}

                <div className="mt-6">
                    {currentQuestionIndex < test.questions.length - 1 ? (
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
