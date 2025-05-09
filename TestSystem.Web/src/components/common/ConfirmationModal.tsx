// ConfirmDeleteModal.tsx
import React from 'react';
import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button, useDisclosure } from '@heroui/react';

interface ConfirmationModalProps {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => void;
}

const ConfirmationModal: React.FC<ConfirmationModalProps> = ({ isOpen, onClose, onConfirm }) => {
    return (
        <Modal isOpen={isOpen} onClose={onClose} isDismissable={false} isKeyboardDismissDisabled={true}>
            <ModalContent>
                <>
                    <ModalHeader className="flex flex-col gap-1">Confirm Delete</ModalHeader>
                    <ModalBody>
                        <p>Are you sure you want to delete this test? This action cannot be undone.</p>
                    </ModalBody>
                    <ModalFooter>
                        <Button color="danger" variant="light" onPress={onClose}>
                            Cancel
                        </Button>
                        <Button color="primary" onPress={onConfirm}>
                            Confirm
                        </Button>
                    </ModalFooter>
                </>
            </ModalContent>
        </Modal>
    );
};

export default ConfirmationModal;
