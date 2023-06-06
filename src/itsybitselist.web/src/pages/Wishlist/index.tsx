import { useEffect, useState } from "react";
import axios from "axios";
interface Wishlist {
  id: number;
  title: string;
  numberOfItems: number;
}
export const Wishlist = () => {
  const [wishlists, setWishlists] = useState<Wishlist[]>([]);

  useEffect(() => {
    axios
      .get<Wishlist[]>("https://localhost:7137/WishlistCollection/me")
      .then((response) => setWishlists(response.data));
  }, []);

  return (
    <div>
      <h1>Wishlist</h1>
      <ul>
        {wishlists.map((wishlist) => (
          <li key={wishlist.id}>{wishlist.title}</li>
        ))}
      </ul>
    </div>
  );
};
