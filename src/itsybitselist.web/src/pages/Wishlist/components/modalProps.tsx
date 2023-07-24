export type ModalProps<T> = {
  id: string;
  show: boolean;
  onHide: () => void;
  data?: T;
};
