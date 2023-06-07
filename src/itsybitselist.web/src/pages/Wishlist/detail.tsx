import axios, { RawAxiosRequestHeaders } from "axios";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useParams } from "react-router-dom";

interface IItem {
  id: string;
  details: string;
  state: "Wished" | "Promised" | "Verified";
}
interface IWishlistDetailView {
  name: string;
  items: IItem[];
}
interface IFormInput {
  details: string;
}
export const Detail = () => {
  const apiUrl = process.env.REACT_APP_API_URL;
  const [wishlist, setWishlist] = useState<IWishlistDetailView>({
    name: "",
    items: [],
  });

  const { id, owner } = useParams<{ id: string; owner: string }>();
  const { register, handleSubmit, reset } = useForm<IFormInput>();

  useEffect(() => {
    fetchWishlistDetails();
  }, [id]);

  const onSubmit = (data: IFormInput) => {
    const headers = new Headers();
    headers.append("owner", owner!);
    headers.append("Content-Type", "application/json");
    const body = JSON.stringify({ details: data.details });

    fetch(`${apiUrl}/wishlist/${id}/item`, {
      method: "POST",
      headers: headers,
      body: body,
    })
      .then((response) => {
        console.log(response);
      })
      .then((data) => {
        console.log(data);
        reset();
        fetchWishlistDetails();
      });
  };

  const fetchWishlistDetails = () => {
    const headers = new Headers();
    headers.append("owner", owner!);
    fetch(`${apiUrl}/wishlist/${id}`, {
      headers: headers,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => setWishlist(data));
  };

  const promiseItem = (itemId: string) => {
    const headers = new Headers();
    headers.append("Content-Type", "application/json");
    headers.append("owner", owner!);
    fetch(`${apiUrl}/wishlist/${id}/item/${itemId}`, {
      method: "PATCH",
      headers: headers,
    }).then((data) => {
      fetchWishlistDetails();
    });
  };

  return (
    <div>
      <h1>{owner}'s Wishlist</h1>
      {wishlist.items.length === 0 && (
        <h2>Your list is empty! Let's add somithng</h2>
      )}
      <ul>
        {wishlist.items.map((item, idx) => (
          <li key={item.id}>
            {item.details}{" "}
            {item.state === "Wished" ? (
              <a href="#" onClick={() => promiseItem(item.id)}>
                Promise
              </a>
            ) : (
              <div>{item.state}</div>
            )}
          </li>
        ))}
      </ul>
      <form onSubmit={handleSubmit(onSubmit)}>
        <input type="text" {...register("details")} />
        <button type="submit">Add Item</button>
      </form>
    </div>
  );
};
