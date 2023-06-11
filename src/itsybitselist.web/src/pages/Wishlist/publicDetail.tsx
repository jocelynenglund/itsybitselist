import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./detail.css";
import { Navbar, Button, Nav, Alert } from "react-bootstrap";
import { faShare } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Item } from "./components/item";

interface IItem {
  id: string;
  details: string;
  state: "Wished" | "Promised" | "Verified";
}
interface IWishlistDetailView {
  name: string;
  items: IItem[];
}

export const PublicDetail = () => {
  const apiUrl = process.env.REACT_APP_API_URL;
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
  });

  const { id } = useParams<{ id: string }>();
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
  }, [id, apiUrl]);

  useEffect(() => {
    fetchWishlistDetails();
  }, [fetchWishlistDetails]);

  const promiseItem = (itemId: string) => {
    const headers = new Headers();
    headers.append("Content-Type", "application/json");
    fetch(`${apiUrl}/wishlist/${id}/item/${itemId}`, {
      method: "PATCH",
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
        <h2>Your list is empty! Let's add somithng</h2>
      )}
      {wishlist.items.map((item, idx) => (
        <Item key={idx} item={item} callback={promiseItem} action="promise" />
      ))}
    </div>
  );
};
