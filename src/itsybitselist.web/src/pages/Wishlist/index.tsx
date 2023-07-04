import { useEffect, useState } from "react";
import appenv from "../../appenv";

const apiUrl = appenv[process.env.NODE_ENV].apiUrl;
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
    const requestUrl = `${apiUrl}/WishlistCollection`;
    fetch(requestUrl, { headers: headers })
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
