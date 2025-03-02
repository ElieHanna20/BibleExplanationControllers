import { DarkThemeToggle, Navbar } from "flowbite-react";
import type { CustomFlowbiteTheme } from "flowbite-react";
import Drawer from "./Drawer";
import sidebarState from "../stores/sidebarState"; // Correct import path

const customTheme: CustomFlowbiteTheme["navbar"] = {
  root: {
    base: "bg-white px-2 py-2.5 dark:border-gray-700 dark:bg-gray-800 sm:px-4 ",
    rounded: {
      on: "rounded",
      off: "",
    },
    bordered: {
      on: "border",
      off: "",
    },
    inner: {
      base: "mx-auto flex flex-wrap items-center justify-between",
      fluid: {
        on: "",
        off: "container",
      },
    },
  },
  brand: {
    base: "flex items-center",
  },
  collapse: {
    base: "w-full md:block md:w-auto",
    list: "mt-4 flex flex-col md:mt-0 md:flex-row md:space-x-8 md:text-sm md:font-medium",
    hidden: {
      on: "hidden",
      off: "",
    },
  },

  toggle: {
    base: "inline-flex items-center rounded-lg p-2 text-sm text-gray-500 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600",
    icon: "h-6 w-6 shrink-0",
  },
};

export default function Header() {
  const { setToggle } = sidebarState(); // Destructure the correct state
  return (
    <>
      <Navbar theme={customTheme} fluid rounded>
        <Navbar.Brand href="https://flowbite-react.com">
          <img
            src="/favicon.ico"
            className="mr-3 h-6 sm:h-9"
            alt="Flowbite React Logo"
          />
          <span className="self-center whitespace-nowrap text-xl font-semibold dark:text-white">
            Flowbite React
          </span>
        </Navbar.Brand>
        <div className="flex items-center gap-4 md:order-2">
          <DarkThemeToggle />

          <Navbar.Toggle onClick={setToggle} />
        </div>
      </Navbar>
      <Drawer />
    </>
  );
}
