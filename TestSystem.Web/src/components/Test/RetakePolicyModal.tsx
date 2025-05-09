import React from 'react';
import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button } from '@heroui/react';
import {RetakePolicy} from "types/Interfaces.ts";

interface RetakePolicyModalProps {
  isOpen: boolean;
  onClose: () => void;
  retakePolicy: RetakePolicy;
}

const RetakePolicyModal: React.FC<RetakePolicyModalProps> = ({ isOpen, onClose, retakePolicy }) => {
  return (
    <Modal size="sm" isOpen={isOpen} onOpenChange={onClose}>
      <ModalContent>
        {() => (
          <>
            <ModalHeader className="flex flex-col gap-1">Retake Policy</ModalHeader>
              <ModalBody>
                  <p><b>Max Retakes:</b> {retakePolicy.maxRetakes}</p>
                  <p><b>Retake Interval:</b> {retakePolicy.allowRetakes? "True": "False"}</p>
                  <p><b>Retake Interval:</b> {retakePolicy.retakeInterval}</p>
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

export default RetakePolicyModal;
