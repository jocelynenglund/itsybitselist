import { useForm } from "react-hook-form";
import { Form, Button, Container } from "react-bootstrap";
import styles from "./index.module.css";
import { useNavigate } from "react-router-dom";
import headerImage from "../../assets/appheader.png";
import appenv from "../../appenv";
import { WishlistSettings } from "../../services/WishlistDetails";
import { postWishlistDetails } from "../../services/WishlistService";

const apiUrl = appenv[process.env.NODE_ENV].apiUrl;
export const Home = () => {
  let navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<WishlistSettings>();

  const onSubmit = (data: WishlistSettings) => {
    postWishlistDetails(data).then((created) => {
      navigate(created.url);
    });
  };
  return (
    <Container className={styles.container}>
      <img src={headerImage} alt="Wishlist" className={styles.headerImage} />
      <h1 className={styles.title}>Create a New Wishlist</h1>
      <Form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
        <Form.Group
          controlId="formBasicWishlistName"
          className={styles.formGroup}
        >
          <Form.Label className={styles.label}>Wishlist Name</Form.Label>
          <Form.Control
            type="text"
            placeholder="e.g. My Birthday Wishlist"
            {...register("name", { required: true })}
            className={styles.input}
          />
          <Form.Control
            as="textarea"
            rows={3}
            placeholder="Enter description"
            {...register("description", { required: false })}
            className={styles.input}
          />

          {errors.name && (
            <span className={styles.error}>This field is required</span>
          )}
        </Form.Group>

        <Button variant="primary" type="submit" className={styles.button}>
          Create Wishlist
        </Button>
      </Form>
    </Container>
  );
};
