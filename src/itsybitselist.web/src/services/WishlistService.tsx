import appenv from "../appenv";

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
export interface ItemDetails {
  details: string;
  link: string;
}
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
