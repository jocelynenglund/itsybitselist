import { Button, FormControl, Modal, Form } from "react-bootstrap";
import { useForm } from "react-hook-form";
import { postWishlistItemDetails } from "../../../services/WishlistService";
import { ItemDetails } from "../../../services/ItemDetails";
import { ModalProps } from "./modalProps";

export const AddItemModal = ({ show, onHide, id }: ModalProps<ItemDetails>) => {
  const { register, handleSubmit, reset } = useForm<ItemDetails>();
  const onSubmit = (data: ItemDetails) => {
    postWishlistItemDetails(id, data).then((data) => {
      reset();
      onHide();
    });
  };
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Add Item</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleSubmit(onSubmit)}>
          <FormControl
            className="formControl"
            {...register("details")}
            placeholder="Description"
          />
          <FormControl
            className="formControl"
            {...register("link")}
            placeholder="Link to item or store"
          />

          <Button variant="secondary" className="button" onClick={onHide}>
            Close
          </Button>
          <Button type="submit" variant="primary" className="button">
            Add Item
          </Button>
        </Form>
      </Modal.Body>
    </Modal>
  );
};
