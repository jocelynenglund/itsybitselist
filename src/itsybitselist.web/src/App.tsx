import React from "react";
import "./App.css";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import { Home } from "./pages/Home";
import { Wishlist } from "./pages/Wishlist";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/mywishlists" element={<Wishlist />} />
      </Routes>
    </Router>
  );
}

export default App;
