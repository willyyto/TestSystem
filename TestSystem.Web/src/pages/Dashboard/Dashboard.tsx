import { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, CardHeader, CardBody } from '@nextui-org/react';
import { useNavigate } from 'react-router-dom';

interface Question {
    id: string;
    text: string;
    type: string;
    answers: string[];
}

interface Test {
    id: string;
    title: string;
    questions: Question[];
}

interface Result {
    id: string;
    test: Test;
    score: number;
    attemptDate: string;
}

const Dashboard = () => {
    const [tests, setTests] = useState<Test[]>([]);
    const [results, setResults] = useState<Result[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            const testResponse = await axios.get('https://localhost:44395/api/test');
            setTests(testResponse.data);
            const resultResponse = await axios.get('https://localhost:44395/api/testresult');
            setResults(resultResponse.data);
        };

        fetchData();
    }, []);

    return (
        <div className="p-4">
            <div className="mb-4">
                <h2 className="text-2xl font-bold">My Tests</h2>
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                {tests.map((test) => (
                    <div key={test.id} onClick={() => navigate(`/quiz/${test.id}`)} className="cursor-pointer">
                        <Card className="shadow-md">
                            <CardHeader>
                                <p className="font-semibold">{test.title}</p>
                            </CardHeader>
                            <CardBody>
                                <p>Number of Questions: {test.questions.length}</p>
                            </CardBody>
                        </Card>
                    </div>
                ))}
            </div>
            <div className="mt-8 mb-4">
                <h2 className="text-2xl font-bold">My Results</h2>
            </div>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                {results.map((result) => (
                    <Card key={result.id} className="shadow-md">
                        <CardHeader>
                            <p className="font-semibold">Test: {result.test.title}</p>
                        </CardHeader>
                        <CardBody>
                            <p>Score: {result.score}</p>
                            <p>Attempt Date: {new Date(result.attemptDate).toLocaleString()}</p>
                        </CardBody>
                    </Card>
                ))}
            </div>
        </div>
    );
};

export default Dashboard;
