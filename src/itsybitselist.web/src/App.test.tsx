import React from "react";
import { render, screen } from "@testing-library/react";
import App from "./App";
import { MemoryRouter } from "react-router-dom";

test("renders Create a new Wishlist", () => {
  render(
    <MemoryRouter initialEntries={["/"]}>
      <App />
    </MemoryRouter>
  );
    const element = screen.getByText(/Create a New Wishlist/i);
    expect(element).toBeInTheDocument();
});
