import { Modal } from "flowbite-react";
import BookChapters from "./BookChapters";
import modalState from "../stores/modalState";

const BookModal = () => {
  const { openModal, setOpenModal } = modalState();

  return (
    <Modal show={openModal} size="md" onClose={() => setOpenModal()} popup>
      <Modal.Header />
      <Modal.Body>
        <BookChapters />
      </Modal.Body>
    </Modal>
  );
};
export default BookModal;
