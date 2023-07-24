import { useForm } from "react-hook-form";
import { WishlistSettings } from "../../../services/WishlistDetails";
import { ModalProps } from "./modalProps";
import { Button, Form, FormControl, Modal } from "react-bootstrap";
import { PatchWishlistDetails } from "../../../services/WishlistService";
import "./editWishlistModal.css";

export const EditWishlistModal = ({
  show,
  onHide,
  id,
  data,
}: ModalProps<WishlistSettings>) => {
  const { register, handleSubmit, reset } = useForm<WishlistSettings>();
  const onSubmit = async (data: WishlistSettings) => {
    await PatchWishlistDetails(id, data);
    reset();
    onHide();
  };
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Edit Wishlist</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleSubmit(onSubmit)}>
          <FormControl
            className="formControl"
            {...register("name")}
            defaultValue={data?.name}
            placeholder="Name"
          />
          <textarea
            className="editDescription"
            {...register("description")}
            placeholder="Description"
            defaultValue={data?.description}
          />
          <Button variant="secondary" className="button" onClick={onHide}>
            Close
          </Button>
          <Button type="submit" variant="primary" className="button">
            Edit Wishlist
          </Button>
        </Form>
      </Modal.Body>
    </Modal>
  );
};
