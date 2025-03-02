// store/sidebarState.ts
import { create } from "zustand";

type SidebarState = {
  toggle: boolean;
  setToggle: () => void;
};

const sidebarState = create<SidebarState>((set) => ({
  toggle: false, // Default value
  setToggle: () => set((state) => ({ toggle: !state.toggle })),
}));

export default sidebarState;
