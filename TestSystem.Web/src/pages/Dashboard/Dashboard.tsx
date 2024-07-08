// Dashboard.tsx
import { useEffect, useState } from 'react';
import { Button, Table, TableBody, TableCell, TableColumn, TableHeader, TableRow } from '@nextui-org/react';
import { useNavigate } from 'react-router-dom';
import { format } from 'date-fns';
import { Icon } from "@iconify/react";
import { fetchTests, fetchResults } from 'contexts/ApiService';
import {Test, Result} from "types/Interfaces.ts";


const Dashboard = () => {
    const [tests, setTests] = useState<Test[]>([]);
    const [results, setResults] = useState<Result[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            const testsData = await fetchTests();
            const testsWithEndDate = testsData.map((test: Test) => ({
                ...test,
                endDate: '2024-12-31' // Hardcoded end date
            }));
            setTests(testsWithEndDate);

            const resultsData = await fetchResults();
            setResults(resultsData);
        };

        fetchData();
    }, []);

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return format(date, 'do MMMM yyyy');
    };

    return (
        <div className="p-4">
            <div className="mb-2 flex justify-between items-center">
                <h2 className="text-3xl font-bold mb-4">Tests Table</h2>
                <Button color="primary" onClick={() => navigate('/createtest')}>
                    Create Test
                </Button>
            </div>
            <Table aria-label="Tests table">
                <TableHeader>
                    <TableColumn>Title</TableColumn>
                    <TableColumn>Questions</TableColumn>
                    <TableColumn>End Date</TableColumn>
                    <TableColumn>Action</TableColumn>
                </TableHeader>
                <TableBody>
                    {tests.map((test) => (
                        <TableRow key={test.id}>
                            <TableCell>{test.title}</TableCell>
                            <TableCell>{test.questions.length}</TableCell>
                            <TableCell>{formatDate(test.endDate)}</TableCell>
                            <TableCell>
                                <Button color="success" size="sm" onClick={() => navigate(`/quiz/${test.id}`)}>
                                    Start
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>

            <div className="mt-16">
                <div className="mb-2 flex justify-between items-center">
                    <h2 className="text-3xl font-bold mb-4">Results Table</h2>
                </div>

                <Table aria-label="Results table">
                    <TableHeader>
                        <TableColumn>Test</TableColumn>
                        <TableColumn>Score</TableColumn>
                        <TableColumn>Attempt Date</TableColumn>
                        <TableColumn>Action</TableColumn>
                    </TableHeader>
                    <TableBody>
                        {results.map((result) => (
                            <TableRow key={result.id}>
                                <TableCell>{result.test.title}</TableCell>
                                <TableCell>{result.score + "/" + result.test.questions.length}</TableCell>
                                <TableCell>{formatDate(result.attemptDate)}</TableCell>
                                <TableCell>
                                    <Button color="primary" size="sm" onClick={() => navigate(`/result/${result.id}`)}
                                            startContent={<Icon icon="mdi:search"/>}>
                                        View Result
                                    </Button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </div>
        </div>
    );
};

export default Dashboard;
