import { faCheck, faTrashAlt, faUndo } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button } from "react-bootstrap";
import "./item.css";

type ItemProps = {
  item: {
    id: string;
    description: string;
    state: string;
  };
  promiseKey?: string | undefined;
  action: "delete" | "promise";
  callback: (id: string, promiseKey?: string) => void;
};

export const Item = ({ item, action, promiseKey, callback }: ItemProps) => {
  return (
    <div className="item-container">
      <div className="item-details">{item.description}</div>
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
        <div>
          {promiseKey && (
            <Button
              variant="primary"
              onClick={() => callback(item.id, promiseKey)}
              className="promise-button"
            >
              <FontAwesomeIcon icon={faUndo} />
            </Button>
          )}
          {item.state}
        </div>
      )}
    </div>
  );
};
