import appenv from "../appenv";
import { ItemDetails } from "./ItemDetails";
import { WishlistSettings } from "./WishlistDetails";

const apiUrl = appenv[process.env.NODE_ENV].apiUrl;

export const fetchWishlistDetails = async (id: string) => {
  const headers = new Headers();
  const response = await fetch(`${apiUrl}/wishlist/${id}`, {
    headers: headers,
  });
  return await response.json();
};

export const fetchPublicWishlistDetails = async (id: string) => {
  const headers = new Headers();
  const response = await fetch(`${apiUrl}/public/${id}`, {
    headers: headers,
  });
  return await response.json();
};
export interface CreatedWishlist {
  id: string;
  url: string;
}

export const postWishlistDetails = async (
  details: WishlistSettings
): Promise<CreatedWishlist> => {
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  const body = JSON.stringify(details);

  const response = await fetch(`${apiUrl}/wishlist/`, {
    method: "POST",
    headers: headers,
    body: body,
  });
  const location = response.headers.get("Location");
  const id = location?.substring(location.lastIndexOf("/") + 1) ?? "";
  const url = `/wishlist/${id}`;

  return { id, url };
};

export const PatchWishlistDetails = async (
  id: string,
  details: WishlistSettings
) => {
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  const body = JSON.stringify(details);

  await fetch(`${apiUrl}/wishlist/${id}`, {
    method: "PATCH",
    headers: headers,
    body: body,
  });
};
export const postWishlistItemDetails = async (
  id: string,
  item: ItemDetails
) => {
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  const body = JSON.stringify(item);

  await fetch(`${apiUrl}/wishlist/${id}/item`, {
    method: "POST",
    headers: headers,
    body: body,
  });
};

export const promiseItem = async (
  id: string,
  itemId: string,
  promiseKey?: string
) => {
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  const body = promiseKey
    ? JSON.stringify({ promiseKey: promiseKey, state: "Wished" })
    : JSON.stringify({ state: "Promised" });

  var response = await fetch(`${apiUrl}/public/${id}/item/${itemId}`, {
    method: "PATCH",
    headers: headers,
    body: body,
  });
  return await response.json();
};
export const deleteItem = async (id: string, itemId: string) => {
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  await fetch(`${apiUrl}/wishlist/${id}/item/${itemId}`, {
    method: "DELETE",
    headers: headers,
  });

  return;
};
