import React from 'react';
import {
    Button,
    Card,
    CardBody,
    CardHeader,
    Modal,
    ModalBody,
    ModalContent,
    ModalFooter,
    ModalHeader,
    RadioGroup,
    Textarea
} from '@nextui-org/react';
import {Question, Test} from 'types/Interfaces.ts';
import {CustomResultRadio} from "components/Test/CustomResultRadio.tsx";

interface ViewTestModalProps {
    isOpen: boolean;
    onClose: () => void;
    test: Test | null;
}

const ViewTestModal: React.FC<ViewTestModalProps> = ({ isOpen, onClose, test }) => {
    const renderQuestion = (question: Question, index: number) => {
        return (
            <div className="py-4">
                <Card key={question.id} radius="sm" shadow="none" isBlurred fullWidth className="p-2 bg-gray-200">
                    <CardHeader>
                        <p>
                            Question <b>{index + 1}</b> of {test?.questions.length}
                        </p>
                    </CardHeader>
                    <CardBody>
                        <h4 className="text-xl mb-4">{question.text}</h4>
                        {question.type === 'MultipleChoice' && (
                            <RadioGroup value={question.answers.find((a) => a.isCorrect)?.text || ''} color={"success"}>
                                {question.answers.map((option) => (
                                    <CustomResultRadio key={option.id} value={option.text} isCorrect={option.isCorrect}>
                                        {option.text}
                                    </CustomResultRadio>
                                ))}
                            </RadioGroup>
                        )}
                        {question.type === 'TrueFalse' && (
                            <RadioGroup value={question.answers.find((a) => a.isCorrect)?.text || ''} color={"success"}>
                                {question.answers.map((option) => (
                                    <CustomResultRadio key={option.id} value={option.text} isCorrect={option.isCorrect}>
                                        {option.text}
                                    </CustomResultRadio>
                                ))}
                            </RadioGroup>
                        )}
                        {question.type === 'ShortAnswer' && (
                            <Textarea
                                placeholder="Correct answer"
                                value={question.answers.find((a) => a.isCorrect)?.text || ''}
                                readOnly
                            />
                        )}
                    </CardBody>
                </Card>
            </div>
            
        );
    };

    return (
        <Modal size="5xl" scrollBehavior="inside" isOpen={isOpen} onClose={onClose} backdrop="blur">
            <ModalContent>
                {(onClose) => (
                    <>
                        <ModalHeader className="flex flex-col gap-1">
                            {test?.title}
                        </ModalHeader>
                        <ModalBody>
                            {test ? (
                                <div>
                                    <p><strong>Company:</strong> {test.company}</p>
                                    <p><strong>Status:</strong> {test.isActive ? "Active" : "Inactive"}</p>
                                    <div className="mt-4">
                                        {test.questions.map((question, index) => renderQuestion(question, index))}
                                    </div>
                                </div>
                            ) : (
                                <p>Loading...</p>
                            )}
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" variant="light" onPress={onClose}>
                                Close
                            </Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    );
};

export default ViewTestModal;
