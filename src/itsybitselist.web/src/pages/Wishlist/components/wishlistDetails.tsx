import "./wishlistDetails.css";
type WishlistProps = {
  description?: string | undefined;
};

export const WishlistDetails = ({ description }: WishlistProps) => {
  return <div className="description">{description}</div>;
};
