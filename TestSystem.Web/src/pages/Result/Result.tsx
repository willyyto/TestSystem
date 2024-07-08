import React, { useState, useEffect } from 'react';
import { Card, CardHeader, CardBody, RadioGroup, Textarea } from '@nextui-org/react';
import { useParams } from 'react-router-dom';
import { CustomRadio } from 'components/Test/CustomRadio';
import apiService from 'contexts/ApiService';
import {Result} from 'types/Interfaces'




const ResultPage: React.FC = () => {
    const { resultId } = useParams<{ resultId: string }>();
    const [result, setResult] = useState<Result | null>(null);

    useEffect(() => {
        const fetchResult = async () => {
            try {
                const data = await apiService.fetchResultById(resultId);
                setResult(data);
            } catch (error) {
                console.error("Error fetching result:", error);
            }
        };

        fetchResult();
    }, [resultId]);

    if (!result) return <p>Loading...</p>;

    return (
        <div className="container mx-auto p-4 space-y-6">
            <Card radius="sm" fullWidth className="p-4 border-2 border-gray-300">
                <CardHeader>
                    <h2 className="text-2xl font-bold mb-4">Test Result</h2>
                </CardHeader>
                <CardBody>
                    <h3 className="text-xl font-bold mb-4">{result.test.title}</h3>
                    <p>Score: {result.score}</p>
                    <p>Attempt Date: {new Date(result.attemptDate).toLocaleString()}</p>
                </CardBody>
            </Card>

            {result.questionResults.map((questionResult, index) => (
                <Card
                    key={questionResult.id}
                    radius="sm"
                    fullWidth
                    className={`p-4 border-2 ${questionResult.isCorrect ? 'border-green-500' : 'border-red-500'}`}
                >
                    <CardHeader>
                        <p>
                            Question <b>{index + 1}</b> of {result.test.questions.length}
                        </p>
                    </CardHeader>
                    <CardBody>
                        <h4 className="text-xl mb-4">{questionResult.question.text}</h4>
                        {questionResult.question.type === 'MultipleChoice' && (
                            <RadioGroup value={questionResult.answer || ''} >
                                {questionResult.question.answers.map((option) => (
                                    <CustomRadio
                                        key={option}
                                        value={option}
                                    >
                                        {option}
                                    </CustomRadio>
                                ))}
                            </RadioGroup>
                        )}
                        {questionResult.question.type === 'TrueFalse' && (
                            <RadioGroup value={questionResult.answer || ''} >
                                <CustomRadio
                                    value="True"
                                >
                                    True
                                </CustomRadio>
                                <CustomRadio
                                    value="False"
                                >
                                    False
                                </CustomRadio>
                            </RadioGroup>
                        )}
                        {questionResult.question.type === 'ShortAnswer' && (
                            <Textarea
                                placeholder="Your answer"
                                value={questionResult.answer || ''}
                                readOnly
                            />
                        )}
                    </CardBody>
                </Card>
            ))}
        </div>
    );
};

export default ResultPage;
