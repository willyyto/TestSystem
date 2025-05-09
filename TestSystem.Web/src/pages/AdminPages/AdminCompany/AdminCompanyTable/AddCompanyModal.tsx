import React, { useState } from "react";
import {
    Modal,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Button,
    Input
} from "@heroui/react";
import { v4 as uuidv4 } from 'uuid';
import apiService, {addAdminCompany} from 'contexts/AdminApiService.tsx'; // Assuming this is the service for API calls

interface AddCompanyModalProps {
    isOpen: boolean;
    onClose: () => void;
}

const AddCompanyModal: React.FC<AddCompanyModalProps> = ({ isOpen, onClose }) => {
    const [name, setName] = useState<string>("");
    const [isActive, setIsActive] = useState<boolean>(true);

    const handleCreateCompany = async () => {
        const newCompany = {
            name
        };

        try {
            await apiService.addAdminCompany(newCompany); // Assuming this is the API call to add a new company
            alert("Company created successfully!");
            onClose();
        } catch (error) {
            console.error('Error creating company:', error);
            alert("Failed to create company. Please try again.");
        }
    };

    return (
        <Modal isOpen={isOpen} onOpenChange={onClose} placement="top-center">
            <ModalContent>
                {(onClose) => (
                    <>
                        <ModalHeader className="flex flex-col gap-1">Add New Company</ModalHeader>
                        <ModalBody>
                            <Input
                                autoFocus
                                label="Company Name"
                                placeholder="Enter the company name"
                                variant="bordered"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                            />
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" variant="flat" onPress={onClose}>
                                Close
                            </Button>
                            <Button color="primary" onPress={handleCreateCompany}>
                                Create Company
                            </Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    );
};

export default AddCompanyModal;
