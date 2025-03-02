import { create } from "zustand";

type ModalState = {
  openModal: boolean;
  setOpenModal: () => void;
};

const modalState = create<ModalState>((set) => ({
  openModal: false, // Default value
  setOpenModal: () => set((state) => ({ openModal: !state.openModal })),
}));

export default modalState;
