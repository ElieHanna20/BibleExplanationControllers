import { Drawer } from "flowbite-react";
import Sidebar from "./Sidebar";
import sidebarState from "../stores/sidebarState"; // Correct import path

export default function Component() {
  const { toggle, setToggle } = sidebarState(); // Destructure the correct state

  return (
    <Drawer open={toggle} onClose={setToggle}>
      <Drawer.Header title="MENU" titleIcon={() => <></>} />
      <Drawer.Items>
        <Sidebar />
      </Drawer.Items>
    </Drawer>
  );
}
