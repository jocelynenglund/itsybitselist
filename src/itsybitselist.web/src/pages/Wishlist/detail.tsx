import { useCallback, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Form, useParams } from "react-router-dom";
import "./detail.css";
import {
  Navbar,
  Button,
  Modal,
  InputGroup,
  FormControl,
  Nav,
  Alert,
} from "react-bootstrap";
import { faPlus, faShare } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Item } from "./components/item";

import appenv from "../../appenv";

const apiUrl = appenv[process.env.NODE_ENV].apiUrl;
interface IItem {
  id: string;
  description: string;
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
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
  });

  const { id, owner } = useParams<{ id: string; owner: string }>();
  const { register, handleSubmit, reset } = useForm<IFormInput>();
  const [showModal, setShowModal] = useState(false);
  const [showAlert, setShowAlert] = useState(false);

  const fetchWishlistDetails = useCallback(() => {
    const headers = new Headers();
    fetch(`${apiUrl}/wishlist/${id}`, {
      headers: headers,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => setWishlist(data));
  }, [id]);

  useEffect(() => {
    fetchWishlistDetails();
  }, [fetchWishlistDetails]);

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
      .then((response) => {})
      .then((data) => {
        reset();
        fetchWishlistDetails();
        setShowModal(false);
      });
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

  const handleShare = () => {
    const shareUrl = `${window.location.origin}/wishlist/public/${id}`;
    navigator.clipboard.writeText(shareUrl);
    setShowAlert(true);
    setTimeout(() => {
      setShowAlert(false);
    }, 2000);
  };

  return (
    <div className="container">
      {showAlert && (
        <Alert
          variant="success"
          className="shareAlert fade show position-fixed"
        >
          Link copied to clipboard!
        </Alert>
      )}
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
          <Navbar.Toggle aria-controls="navbar-nav" />
          <Navbar.Collapse id="navbar-nav">
            <Nav className="me-auto"></Nav>
            <Nav>
              <Button
                variant="outline-light"
                className="shareButton"
                onClick={() => handleShare()}
              >
                <FontAwesomeIcon icon={faShare} />
              </Button>
            </Nav>
          </Navbar.Collapse>
        </Navbar>
      </div>
      {wishlist.items.length === 0 && (
        <h2>Your list is empty! Let's add something</h2>
      )}
      {wishlist.items.map((item, idx) => (
        <Item key={idx} item={item} action="delete" callback={deleteItem} />
      ))}

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
