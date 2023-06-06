import axios, { RawAxiosRequestHeaders } from "axios";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

interface IItem {
  id: string;
  details: string;
}
interface IWishlistDetailView {
  name: string;
  items: IItem[];
}
export const Detail = () => {
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
  });

  const { id, owner } = useParams<{ id: string; owner: string }>();
  const apiUrl = process.env.REACT_APP_API_URL;

  useEffect(() => {
    const headers = new Headers();
    headers.append("owner", owner!);
    fetch(`${apiUrl}/wishlist/${id}`, {
      headers: headers,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => setWishlist(data));
  }, [id]);
  return (
    <div>
      <h1>{owner}'s Wishlist</h1>
      <ul>
        {wishlist.items.map((item, idx) => (
          <li key={item.id}>{item.details}</li>
        ))}
      </ul>
    </div>
  );
};
