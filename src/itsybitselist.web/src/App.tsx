import React from "react";
import "./App.css";
import {
  Route,
  createBrowserRouter,
  Routes,
  RouterProvider,
} from "react-router-dom";
import { Home } from "./pages/Home";
import { Detail } from "./pages/Wishlist/detail";
import "bootstrap/dist/css/bootstrap.min.css";
import { PublicDetail } from "./pages/Wishlist/publicDetail";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Home />,
    // errorElement: <ErrorPage />,
  },
  {
    path: "/wishlist/:id",
    element: <Detail />,
  },
  {
    path: "/wishlist/public/:id",
    element: <PublicDetail />,
  },
]);
function App() {
  return <RouterProvider router={router} />;
}

export default App;
