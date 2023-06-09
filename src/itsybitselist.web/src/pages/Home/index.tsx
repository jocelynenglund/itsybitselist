import { useForm } from "react-hook-form";
import { Form, Button, Container } from "react-bootstrap";
import styles from "./index.module.css";
import { useNavigate } from "react-router-dom";
import headerImage from "../../assets/appheader.png";
import appenv from "../../appenv";

const apiUrl = appenv[process.env.NODE_ENV].apiUrl;
interface IFormInput {
  wishlistName: string;
}

export const Home = () => {
  let navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<IFormInput>();

  const onSubmit = (data: IFormInput) => {
    const headers = new Headers();
    headers.append("Content-Type", "application/json");
    const body = JSON.stringify({ name: data.wishlistName });

    fetch(`${apiUrl}/wishlist/`, {
      method: "POST",
      headers: headers,
      body: body,
    }).then((response) => {
      const location = response.headers.get("Location");
      const guid = location?.substring(location.lastIndexOf("/") + 1);
      navigate(`/wishlist/${guid}`);
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
            {...register("wishlistName", { required: true })}
            className={styles.input}
          />
          {errors.wishlistName && (
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
