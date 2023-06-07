import { useForm } from "react-hook-form";
import { Form, Button, Container } from "react-bootstrap";
import styles from "./index.module.css";
import { useNavigate } from "react-router-dom";

interface IFormInput {
  email: string;
  wishlistName: string;
}
const apiUrl = process.env.REACT_APP_API_URL;

export const Home = () => {
  let navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<IFormInput>();

  const onSubmit = (data: IFormInput) => {
    const headers = new Headers();
    headers.append("owner", data.email);
    headers.append("Content-Type", "application/json");
    const body = JSON.stringify({ name: data.wishlistName });

    fetch(`${apiUrl}/wishlist/`, {
      method: "POST",
      headers: headers,
      body: body,
    }).then((response) => {
      const location = response.headers.get("Location");
      const guid = location?.substring(location.lastIndexOf("/") + 1);
      navigate(`/mywishlists/${data.email}/${guid}`);
    });
  };
  return (
    <Container className={styles.container}>
      <h1 className={styles.title}>Create a New Wishlist</h1>
      <Form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
        <Form.Group controlId="formBasicEmail" className={styles.formGroup}>
          <Form.Label className={styles.label}>Email address</Form.Label>
          <Form.Control
            type="email"
            placeholder="Enter email"
            {...register("email", { required: true })}
            className={styles.input}
          />
          {errors.email && (
            <span className={styles.error}>This field is required</span>
          )}
          <Form.Text className={styles.text}>
            We'll never share your email with anyone else.
          </Form.Text>
        </Form.Group>

        <Form.Group
          controlId="formBasicWishlistName"
          className={styles.formGroup}
        >
          <Form.Label className={styles.label}>Wishlist Name</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter wishlist name"
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
