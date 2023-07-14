import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./detail.css";
import { Navbar, Button, Nav, Alert } from "react-bootstrap";
import { faShare } from "@fortawesome/free-solid-svg-icons";
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
interface PromiseKeys {
  [key: string]: string;
}
const promiseDictionary: PromiseKeys = {};
export const PublicDetail = () => {
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
  });

  const { id } = useParams<{ id: string }>();
  const [showAlert, setShowAlert] = useState(false);

  const fetchWishlistDetails = useCallback(() => {
    const headers = new Headers();
    fetch(`${apiUrl}/public/${id}`, {
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

  useEffect(() => {
    document.title = `ItsyBitsyList - ${wishlist.name}`;
  }, [id, wishlist.name]);

  const promiseItem = (itemId: string, promiseKey?: string) => {
    const headers = new Headers();
    headers.append("Content-Type", "application/json");
    const body = promiseKey
      ? JSON.stringify({ promiseKey: promiseKey, state: "Wished" })
      : JSON.stringify({ state: "Promised" });

    fetch(`${apiUrl}/public/${id}/item/${itemId}`, {
      method: "PATCH",
      headers: headers,
      body: body,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        promiseDictionary[itemId] = data;
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
      <div className="wishlist">
        {wishlist.items.length === 0 && (
          <h2>The list is still empty, check again later!</h2>
        )}
        {wishlist.items.map((item, idx) => {
          console.log(promiseDictionary[item.id], "key is");

          return (
            <Item
              key={idx}
              item={item}
              callback={promiseItem}
              promiseKey={promiseDictionary[item.id]}
              action="promise"
            />
          );
        })}
      </div>

      <footer>
        <b>
          Create your own wishlist <a href="/">here</a>
        </b>
      </footer>
    </div>
  );
};
