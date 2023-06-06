import { useEffect, useState } from "react";
import axios from "axios";
interface IWishlistListView {
  id: number;
  title: string;
  numberOfItems: number;
}
export const Wishlist = () => {
  const [wishlists, setWishlists] = useState<IWishlistListView[]>([]);

  useEffect(() => {
    const headers = new Headers();
    headers.append("owner", "me");
    fetch("https://localhost:7137/WishlistCollection", { headers: headers })
      .then((response) => response.json())
      .then((data) => setWishlists(data));
  }, []);

  return (
    <div>
      <h1>Wishlist</h1>
      <ul>
        {wishlists.map((wishlist) => (
          <li key={wishlist.id}>
            <a href={`/mywishlists/me/${wishlist.id}`}>{wishlist.title}</a> -{" "}
            {wishlist.numberOfItems}
          </li>
        ))}
      </ul>
    </div>
  );
};
