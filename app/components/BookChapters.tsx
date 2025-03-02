import { ListGroup } from "flowbite-react";
import modalState from "~/stores/modalState";

const BookChapters = () => {
  const { setOpenModal } = modalState();
  return (
    <div className="flex justify-center">
      <ListGroup className="w-48">
        <ListGroup.Item
          onClick={() => {
            setOpenModal();
            console.log("hello");
          }}
          href="#">
          1
        </ListGroup.Item>
      </ListGroup>
    </div>
  );
};

export default BookChapters;
