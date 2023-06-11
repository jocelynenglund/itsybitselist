import { faCheck, faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button } from "react-bootstrap";
import "./item.css";

type ItemProps = {
  item: {
    id: string;
    details: string;
    state: string;
  };
  action: "delete" | "promise";
  callback: (id: string) => void;
};

export const Item = ({ item, action, callback }: ItemProps) => {
  return (
    <div className="item-container">
      <div className="item-details">{item.details}</div>
      {item.state !== "Promised" ? (
        action === "delete" ? (
          <Button
            variant="danger"
            onClick={() => callback(item.id)}
            className="delete-button"
          >
            <FontAwesomeIcon icon={faTrashAlt} />
          </Button>
        ) : (
          <Button
            variant="primary"
            onClick={() => callback(item.id)}
            className="promise-button"
          >
            <FontAwesomeIcon icon={faCheck} />
          </Button>
        )
      ) : (
        <div>{item.state}</div>
      )}
    </div>
  );
};
