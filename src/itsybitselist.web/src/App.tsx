import React from "react";
import "./App.css";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import { Home } from "./pages/Home";
import { Wishlist } from "./pages/Wishlist";
import { Detail } from "./pages/Wishlist/detail";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/mywishlists" element={<Wishlist />} />
        <Route path="/mywishlists/:owner/:id" element={<Detail />} />
      </Routes>
    </Router>
  );
}

export default App;
