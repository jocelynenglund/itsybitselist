import { useCallback, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Form, useParams } from "react-router-dom";
import "./detail.css";
import {
  Navbar,
  Button,
  Modal,
  FormControl,
  Nav,
  Alert,
} from "react-bootstrap";
import { faCog, faPlus, faShare } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Item } from "./components/item";

import appenv from "../../appenv";
import { Url } from "url";
import { WishlistDetails } from "./components/wishlistDetails";
import {
  ItemDetails,
  deleteItem,
  fetchWishlistDetails,
  postWishlistItemDetails,
} from "../../services/WishlistService";

interface IItem {
  id: string;
  description: string;
  state: "Wished" | "Promised" | "Verified";
  link: Url;
}
interface IWishlistDetailView {
  name: string;
  items: IItem[];
  publicId?: string | undefined;
  description?: string | undefined;
}

export const Detail = () => {
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
    publicId: undefined,
  });

  const { id, owner } = useParams<{ id: string; owner: string }>();
  const { register, handleSubmit, reset } = useForm<ItemDetails>();
  const [showModal, setShowModal] = useState(false);
  const [showAlert, setShowAlert] = useState(false);

  const fetchWishlist = useCallback(() => {
    if (!id) {
      return;
    }
    fetchWishlistDetails(id).then((data) => setWishlist(data));
  }, [id]);

  useEffect(() => {
    document.title = `ItsyBitsyList - ${wishlist.name}`;
  }, [id, wishlist.name]);

  useEffect(() => {
    fetchWishlist();
  }, [fetchWishlist]);

  const onSubmit = (data: ItemDetails) => {
    if (!id) {
      return;
    }
    postWishlistItemDetails(id, data).then((data) => {
      reset();
      fetchWishlist();
      setShowModal(false);
    });
  };

  const handleDeleteItem = (itemId: string) => {
    if (!id) {
      return;
    }
    deleteItem(id, itemId).then((data) => {
      fetchWishlist();
    });
  };

  const handleShare = () => {
    const shareUrl = `${window.location.origin}/wishlist/public/${wishlist.publicId}`;
    navigator.clipboard.writeText(shareUrl);
    setShowAlert(true);
    setTimeout(() => {
      setShowAlert(false);
    }, 2000);
  };
  const handleSettings = () => {
    alert("Not implemented yet");
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
                className="settingsButton"
                onClick={() => handleSettings()}
              >
                <FontAwesomeIcon icon={faCog} />
              </Button>
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
      <div className="wishlist">
        <WishlistDetails description={wishlist.description} />
        <hr />
        {wishlist.items.length === 0 && (
          <div className="empty">
            <h2>Your list is empty! Let's add something</h2>
          </div>
        )}
        {wishlist.items.map((item, idx) => (
          <Item
            key={idx}
            item={item}
            action="delete"
            callback={handleDeleteItem}
          />
        ))}
      </div>
      <Modal show={showModal} onHide={() => setShowModal(false)}>
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

            <Button
              variant="secondary"
              className="button"
              onClick={() => setShowModal(false)}
            >
              Close
            </Button>
            <Button type="submit" variant="primary" className="button">
              Add Item
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
      <footer>
        <b>
          Create a new wishlist <a href="/">here</a>. <br />
          Make sure you bookmark this page if you want to edit this later!
        </b>
      </footer>
    </div>
  );
};
