import React, { useState, useEffect } from "react";
import {
    Modal,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Button,
    Input,
    SelectItem,
    Select
} from "@heroui/react";
import { v4 as uuidv4 } from 'uuid';
import apiService from 'contexts/AdminApiService.tsx'; // Assuming this is the service for API calls

interface AddUserModalProps {
    isOpen: boolean;
    onClose: () => void;
}

interface Company {
    id: string;
    name: string;
}

const AddUserModal: React.FC<AddUserModalProps> = ({ isOpen, onClose }) => {
    const [name, setName] = useState<string>("");
    const [username, setUsername] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [companyId, setCompanyId] = useState<string>("");
    const [role, setRole] = useState<string>("user");
    const [password, setPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");
    const [companies, setCompanies] = useState<Company[]>([]);
    const roles: string[] = ["admin", "user"];

    useEffect(() => {
        const fetchCompanies = async () => {
            try {
                const data: Company[] = await apiService.fetchAdminCompanies(); // Assuming this is the API call to fetch companies
                setCompanies(data);
            } catch (error) {
                console.error('Error fetching companies:', error);
            }
        };

        fetchCompanies();
    }, []);

    const handleCreateUser = async () => {
        if (password !== confirmPassword) {
            alert("Passwords do not match!");
            return;
        }

        const newUser = {
            username,
            password,
            name,
            email,
            role,
            companyId,
        };

        try {
            await apiService.addAdminUser(newUser); // Assuming this is the API call to add a new user
            alert("User created successfully!");
            onClose();
        } catch (error) {
            console.error('Error creating user:', error);
            alert("Failed to create user. Please try again.");
        }
    };

    return (
        <Modal isOpen={isOpen} onOpenChange={onClose} placement="top-center">
            <ModalContent>
                {(onClose) => (
                    <>
                        <ModalHeader className="flex flex-col gap-1">Add New User</ModalHeader>
                        <ModalBody>
                            <Input
                                autoFocus
                                label="Full Name"
                                placeholder="Enter the name"
                                variant="bordered"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                            />
                            <Input
                                label="Username"
                                placeholder="Enter the username"
                                variant="bordered"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                            />
                            <Input
                                label="Email"
                                placeholder="Enter the email"
                                variant="bordered"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                            />
                            <Select
                                label="Company"
                                placeholder="Select the company"
                                variant="bordered"
                                value={companyId}
                                onChange={(e) => setCompanyId(e.target.value)}
                            >
                                {companies.map((company) => (
                                    <SelectItem key={company.id} value={company.id} className="capitalize">
                                        {company.name}
                                    </SelectItem>
                                ))}
                            </Select>
                            <Select
                                label="Role"
                                placeholder="Select the role"
                                variant="bordered"
                                value={role}
                                onChange={(e) => setRole(e.target.value)}
                            >
                                {roles.map((role) => (
                                    <SelectItem key={role} value={role} className="capitalize">
                                        {role}
                                    </SelectItem>
                                ))}
                            </Select>
                            <Input
                                label="Password"
                                placeholder="Enter the password"
                                type="password"
                                variant="bordered"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                            <Input
                                label="Confirm Password"
                                placeholder="Confirm the password"
                                type="password"
                                variant="bordered"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                            />
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" variant="flat" onPress={onClose}>
                                Close
                            </Button>
                            <Button color="primary" onPress={handleCreateUser}>
                                Create User
                            </Button>
                        </ModalFooter>
                    </>
                )}
            </ModalContent>
        </Modal>
    );
};

export default AddUserModal;
