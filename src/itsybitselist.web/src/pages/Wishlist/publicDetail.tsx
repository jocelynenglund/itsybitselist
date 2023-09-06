import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./detail.css";
import { Navbar, Button, Nav, Alert } from "react-bootstrap";
import { faShare } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Item } from "./components/item";
import { WishlistDetails } from "./components/wishlistDetails";
import {
  fetchWishlistDetails,
  promiseItem,
} from "../../services/WishlistService";
import Loader, { ThreeDots } from "react-loader-spinner";

interface IItem {
  id: string;
  description: string;
  state: "Wished" | "Promised" | "Verified";
}
interface IWishlistDetailView {
  name: string;
  items: IItem[];
  description?: string | undefined;
}
interface PromiseKeys {
  [key: string]: string;
}
const promiseDictionary: PromiseKeys = {};
export const PublicDetail = () => {
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
    description: undefined,
  });

  const { id } = useParams<{ id: string }>();
  const [showAlert, setShowAlert] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const fetchWishlist = useCallback(() => {
    if (!id) {
      return;
    }
    fetchWishlistDetails(encodeURIComponent(id)).then((data) => {
      setWishlist(data)
      setIsLoading(false)
    }
    );
  }, [id]);

  useEffect(() => {
    fetchWishlist();
  }, [fetchWishlist]);

  useEffect(() => {
    document.title = `ItsyBitsyList - ${wishlist.name}`;
  }, [id, wishlist.name]);

  const handlePromiseItem = (itemId: string, promiseKey?: string) => {
    if (id === undefined) return;
    promiseItem(encodeURIComponent(id), itemId, promiseKey).then((data) => {
      promiseDictionary[itemId] = data;
      fetchWishlist();
    });
  };

  const handleShare = () => {
    if (id === undefined) return;
    const shareUrl = `${
      window.location.origin
    }/wishlist/public/${encodeURIComponent(id)}`;
    navigator.clipboard.writeText(shareUrl);
    setShowAlert(true);
    setTimeout(() => {
      setShowAlert(false);
    }, 2000);
  };

  return (
    <div className="container">
      {isLoading && (
        <div className="loader-container">
          <ThreeDots 
height="80" 
width="80" 
radius="9"
color="#942323" 
ariaLabel="three-dots-loading"
wrapperStyle={{}}
visible={true}
 />
        </div>
      )}
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
      <WishlistDetails description={wishlist.description} />
      <hr />
      <div className="wishlist">
        {wishlist.items.length === 0 && (
          <h2>The list is still empty, check again later!</h2>
        )}
        {wishlist.items.map((item, idx) => {
          return (
            <Item
              key={idx}
              item={item}
              callback={handlePromiseItem}
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
