import React, {useState} from 'react';
import {
    Button,
    Card,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownTrigger,
    Input,
    Select,
    SelectItem,
    Spacer
} from '@nextui-org/react';
import {useNavigate} from 'react-router-dom';
import apiService from 'contexts/AdminApiService.tsx';
import {CreateQuestion} from "types/Interfaces.ts";

const questionTypes = ['Multiple Choice', 'True/False', 'Short Answer'];

const CreateTest: React.FC = () => {
    const [name, setName] = useState('');
    const [questions, setQuestions] = useState<CreateQuestion[]>([]);
    const navigate = useNavigate();

    const handleAddQuestion = () => {
        setQuestions([...questions, { id: '', text: '', type: 'Multiple Choice', options: [], correctOption: '' }]);
    };

    const handleRemoveQuestion = (index: number) => {
        const updatedQuestions = [...questions];
        updatedQuestions.splice(index, 1);
        setQuestions(updatedQuestions);
    };

    const handleRemoveOption = (questionIndex: number, optionIndex: number) => {
        const updatedQuestions = [...questions];
        updatedQuestions[questionIndex].options.splice(optionIndex, 1);
        setQuestions(updatedQuestions);
    };

    const handleCreateTest = async () => {
        try {
            await apiService.createTest({ name, questions });
            navigate('/dashboard');
        } catch (error) {
            console.error('Failed to create test', error);
        }
    };

    return (
        <div className="p-6 max-w-4xl mx-auto">
            <h2 className="text-3xl font-bold mb-6">Create Test</h2>
            <Input
                label="Name"
                placeholder="Test name"
                fullWidth
                value={name}
                onChange={(e) => setName(e.target.value)}
                className="mb-4"
            />
            <Button onClick={handleAddQuestion} className="mb-4">
                Add Question
            </Button>
            {questions.map((question, index) => (
                <Card key={index} className="mt-4 p-4">
                    <Input
                        label={`Question ${index + 1}`}
                        placeholder="Question text"
                        fullWidth
                        value={question.text}
                        onChange={(e) => {
                            const updatedQuestions = [...questions];
                            updatedQuestions[index].text = e.target.value;
                            setQuestions(updatedQuestions);
                        }}
                        className="mb-4"
                    />
                    <Dropdown>
                        <DropdownTrigger>
                            <Button variant="bordered">
                                {question.type}
                            </Button>
                        </DropdownTrigger>
                        <DropdownMenu
                            aria-label="Question Type"
                            onAction={(key) => {
                                const updatedQuestions = [...questions];
                                updatedQuestions[index].type = key as string;
                                if (key !== 'Multiple Choice') {
                                    updatedQuestions[index].options = [];
                                }
                                setQuestions(updatedQuestions);
                            }}
                        >
                            {questionTypes.map((type) => (
                                <DropdownItem key={type}>{type}</DropdownItem>
                            ))}
                        </DropdownMenu>
                    </Dropdown>
                    <Spacer y={1} />
                    {question.type === 'Multiple Choice' && (
                        <div>
                            <Button onClick={() => {
                                const updatedQuestions = [...questions];
                                updatedQuestions[index].options.push('');
                                setQuestions(updatedQuestions);
                            }} className="mb-2">
                                Add Option
                            </Button>
                            {question.options.map((option, optionIndex) => (
                                <div key={optionIndex} className="mt-2 flex items-center">
                                    <Input
                                        placeholder={`Option ${optionIndex + 1}`}
                                        fullWidth
                                        value={option}
                                        onChange={(e) => {
                                            const updatedQuestions = [...questions];
                                            updatedQuestions[index].options[optionIndex] = e.target.value;
                                            setQuestions(updatedQuestions);
                                        }}
                                        className="mr-2"
                                    />
                                    <Button
                                        color="danger"
                                        onClick={() => handleRemoveOption(index, optionIndex)}
                                    >
                                        Remove
                                    </Button>
                                </div>
                            ))}
                            <Spacer y={1} />
                            <Select
                                label="Correct Option"
                                placeholder="Choose correct option"
                                value={question.correctOption}
                                onChange={(value) => {
                                    const updatedQuestions = [...questions];
                                    updatedQuestions[index].correctOption = value;
                                    setQuestions(updatedQuestions);
                                }}
                            >
                                {question.options.map((option, optionIndex) => (
                                    <SelectItem key={optionIndex} value={option}>
                                        {option}
                                    </SelectItem>
                                ))}
                            </Select>
                        </div>
                    )}
                    {question.type === 'True/False' && (
                        <Select
                            label="Correct Option"
                            placeholder="Choose correct option"
                            value={question.correctOption}
                            onChange={(value) => {
                                const updatedQuestions = [...questions];
                                updatedQuestions[index].correctOption = value;
                                setQuestions(updatedQuestions);
                            }}
                        >
                            <SelectItem key="True" value="True">
                                True
                            </SelectItem>
                            <SelectItem key="False" value="False">
                                False
                            </SelectItem>
                        </Select>
                    )}
                    {question.type === 'Short Answer' && (
                        <Input
                            label="Correct Answer"
                            placeholder="Correct answer"
                            fullWidth
                            value={question.correctOption}
                            onChange={(e) => {
                                const updatedQuestions = [...questions];
                                updatedQuestions[index].correctOption = e.target.value;
                                setQuestions(updatedQuestions);
                            }}
                        />
                    )}
                    <Spacer y={1} />
                    <Button color="danger" onClick={() => handleRemoveQuestion(index)}>
                        Remove Question
                    </Button>
                </Card>
            ))}
            <Spacer y={1} />
            <div className="flex justify-between">
                <Button color="danger" onClick={() => navigate('/dashboard')}>
                    Cancel
                </Button>
                <Button onClick={handleCreateTest}>
                    Create
                </Button>
            </div>
        </div>
    );
};

export default CreateTest;
