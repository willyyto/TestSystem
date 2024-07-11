import React, {useEffect, useState} from 'react';
import {Card, CardBody, CardHeader, RadioGroup, Textarea} from '@nextui-org/react';
import {useParams} from 'react-router-dom';
import {CustomRadio} from 'components/Test/CustomRadio';
import apiService from 'contexts/UserApiService.tsx';
import {Result} from 'types/Interfaces'

const UserResult: React.FC = () => {
    const { resultId } = useParams<{ resultId: string }>();
    const [result, setResult] = useState<Result | null>(null);

    useEffect(() => {
        const fetchUserResult = async () => {
            try {
                const data = await apiService.fetchUserResultById(resultId);
                setResult(data);
            } catch (error) {
                console.error("Error fetching result:", error);
            }
        };

        fetchUserResult();
    }, [resultId]);

    if (!result) return <p>Loading...</p>;

    return (
        <div className="container mx-auto p-4 space-y-6">
            <Card radius="sm" fullWidth className="p-4 border-2 border-gray-200">
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
                    className={`p-4 border-2 ${questionResult.isCorrect ? 'border-success' : 'border-danger'}`}
                >
                    <CardHeader>
                        <p>
                            Question <b>{index + 1}</b> of {result.test.questions.length}
                        </p>
                    </CardHeader>
                    <CardBody>
                        <h4 className="text-xl mb-4">{questionResult.question.text}</h4>
                        {questionResult.question.type === 'MultipleChoice' && (
                            <RadioGroup value={questionResult.answer || ''}
                                        color={questionResult.isCorrect ? "success" : "danger"}>
                                {questionResult.question.answers.map((option) => (
                                    <CustomRadio
                                        key={option.id}
                                        value={option.text}
                                        isCorrect={option.isCorrect}
                                    >
                                        {option.text}
                                    </CustomRadio>
                                ))}
                            </RadioGroup>
                        )}
                        {questionResult.question.type === 'TrueFalse' && (
                            <RadioGroup value={questionResult.answer || ''}
                                        color={questionResult.isCorrect ? "success" : "danger"}>
                                {questionResult.question.answers.map((option) => (
                                    <CustomRadio
                                        key={option.id}
                                        value={option.text}
                                        isCorrect={option.isCorrect}
                                    >
                                        {option.text}
                                    </CustomRadio>
                                ))}
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

export default UserResult;
