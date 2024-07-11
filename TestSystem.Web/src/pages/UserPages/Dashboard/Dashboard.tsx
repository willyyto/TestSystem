// Dashboard.tsx
import React, {useEffect, useState} from 'react';
import {Button, Table, TableBody, TableCell, TableColumn, TableHeader, TableRow} from '@nextui-org/react';
import {useNavigate} from 'react-router-dom';
import {format} from 'date-fns';
import {fetchUserResults, fetchUserTests} from '../../../contexts/UserApiService.tsx';
import {Result, Test} from "../../../types/Interfaces.ts";
import {MagnifyingGlassIcon} from "@heroicons/react/24/outline";


const Dashboard = () => {
    const [tests, setUserTests] = useState<Test[]>([]);
    const [results, setUserResults] = useState<Result[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUserData = async () => {
            const testsData = await fetchUserTests();
            const testsWithEndDate = testsData.map((test: Test) => ({
                ...test,
                endDate: '2024-12-31' // Hardcoded end date
            }));
            setUserTests(testsWithEndDate);

            const resultsData = await fetchUserResults();
            setUserResults(resultsData);
        };

        fetchUserData();
    }, []);

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return format(date, 'do MMMM yyyy');
    };

    return (
        <div className="p-4">
            <div className="mb-2 flex justify-between items-center">
                <h1 className="text-5xl py-8 font-bold mb-4">User Tests</h1>
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
                    <h1 className="text-5xl py-8 font-bold mb-4">User Results</h1>
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
                                            startContent={<MagnifyingGlassIcon
                                                className="h-4 w-4 text-white"/>}>
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
