import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./detail.css";
import { Navbar, Button, Nav, Alert } from "react-bootstrap";
import { faCog, faPlus, faShare } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Item } from "./components/item";

import { Url } from "url";
import { WishlistDetails } from "./components/wishlistDetails";
import {
  deleteItem,
  fetchWishlistDetails,
} from "../../services/WishlistService";
import { ItemDetails } from "../../services/ItemDetails";
import { AddItemModal } from "./components/addItemModal";
import { EditWishlistModal } from "./components/editWishlistModal";
import { WishlistSettings } from "../../services/WishlistDetails";

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

  const [settings, setSettings] = useState<WishlistSettings>();

  const { id } = useParams<{ id: string }>();
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showAlert, setShowAlert] = useState(false);

  const fetchWishlist = useCallback(() => {
    if (!id) {
      return;
    }
    fetchWishlistDetails(id).then((data) => setWishlist(data));
  }, [id]);
  useEffect(() => {
    setSettings({ name: wishlist.name, description: wishlist.description });
  }, [wishlist]);
  useEffect(() => {
    document.title = `ItsyBitsyList - ${wishlist.name}`;
  }, [id, wishlist.name]);

  useEffect(() => {
    fetchWishlist();
  }, [fetchWishlist]);

  const handleDeleteItem = (itemId: string) => {
    if (!id) {
      return;
    }
    deleteItem(id, itemId).then((data) => {
      fetchWishlist();
    });
  };
  const handleHideModal = () => {
    if (showAddModal) setShowAddModal(false);
    else if (showEditModal) setShowEditModal(false);
    fetchWishlist();
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
    setShowEditModal(true);
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
            onClick={() => setShowAddModal(true)}
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
      {id && (
        <AddItemModal
          id={id}
          show={showAddModal}
          onHide={() => handleHideModal()}
        />
      )}
      {id && (
        <EditWishlistModal
          id={id}
          show={showEditModal}
          onHide={() => handleHideModal()}
          data={settings}
        />
      )}
      <footer>
        <b>
          Create a new wishlist <a href="/">here</a>. <br />
          Make sure you bookmark this page if you want to edit this later!
        </b>
      </footer>
    </div>
  );
};
