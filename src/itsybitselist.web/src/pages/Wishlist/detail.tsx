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

  useEffect(() => {
    const headers = new Headers();
    headers.append("owner", owner!);
    fetch(`https://localhost:7137/wishlist/${id}`, {
      headers: headers,
    })
      .then((response) => response.json())
      .then((data) => setWishlist(data));
  }, []);
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
