import axios, { RawAxiosRequestHeaders } from "axios";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Form, useParams } from "react-router-dom";
import "./detail.css";
import {
  Navbar,
  Button,
  Modal,
  InputGroup,
  FormControl,
} from "react-bootstrap";
import { faPlus, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

interface IItem {
  id: string;
  details: string;
  state: "Wished" | "Promised" | "Verified";
}
interface IWishlistDetailView {
  name: string;
  items: IItem[];
}
interface IFormInput {
  details: string;
}
export const Detail = () => {
  const apiUrl = process.env.REACT_APP_API_URL;
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
  });

  const { id, owner } = useParams<{ id: string; owner: string }>();
  const { register, handleSubmit, reset } = useForm<IFormInput>();
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    fetchWishlistDetails();
  }, [id]);

  const onSubmit = (data: IFormInput) => {
    const headers = new Headers();
    headers.append("owner", owner!);
    headers.append("Content-Type", "application/json");
    const body = JSON.stringify({ details: data.details });

    fetch(`${apiUrl}/wishlist/${id}/item`, {
      method: "POST",
      headers: headers,
      body: body,
    })
      .then((response) => {
        console.log(response);
      })
      .then((data) => {
        console.log(data);
        reset();
        fetchWishlistDetails();
        setShowModal(false);
      });
  };

  const fetchWishlistDetails = () => {
    const headers = new Headers();
    fetch(`${apiUrl}/wishlist/${id}`, {
      headers: headers,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => setWishlist(data));
  };

  const deleteItem = (itemId: string) => {
    const headers = new Headers();
    headers.append("Content-Type", "application/json");
    headers.append("owner", owner!);
    fetch(`${apiUrl}/wishlist/${id}/item/${itemId}`, {
      method: "DELETE",
      headers: headers,
    }).then((data) => {
      fetchWishlistDetails();
    });
  };

  return (
    <div className="container">
      <div>
        <Navbar className="custom-navbar">
          <Navbar.Brand href="#" className="navbar-title">
            {wishlist.name}
          </Navbar.Brand>
          <Button
            variant="outline-light"
            className="addButton"
            onClick={() => setShowModal(true)}
          >
            <FontAwesomeIcon icon={faPlus} />
          </Button>
        </Navbar>
      </div>
      {wishlist.items.length === 0 && (
        <h2>Your list is empty! Let's add somithng</h2>
      )}
      <ul>
        {wishlist.items.map((item, idx) => (
          <li
            key={item.id}
            className="d-flex justify-content-between align-items-center"
          >
            {item.details}{" "}
            {item.state !== "Promised" ? (
              <Button variant="danger" onClick={() => deleteItem(item.id)}>
                <FontAwesomeIcon icon={faTrashAlt} />
              </Button>
            ) : (
              <div>{item.state}</div>
            )}
          </li>
        ))}
      </ul>

      <Modal show={showModal} onHide={() => setShowModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Add Item</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit(onSubmit)}>
            <InputGroup>
              <FormControl {...register("details")} />
              <Button type="submit" variant="primary">
                Add Item
              </Button>
              <Button variant="secondary" onClick={() => setShowModal(false)}>
                Close
              </Button>
            </InputGroup>
          </Form>
        </Modal.Body>
      </Modal>
    </div>
  );
};
