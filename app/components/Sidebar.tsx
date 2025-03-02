// import React, { Suspense } from "react";
import { CustomFlowbiteTheme, Sidebar } from "flowbite-react";
import modalState from "../stores/modalState";
// import NavigationData from "./FetchNavigationData";

const customTheme: CustomFlowbiteTheme["sidebar"] = {
  root: {
    base: "text-transparent h-full",
    collapsed: {
      on: "w-16",
      off: "w-64",
    },
    inner:
      "h-full overflow-y-auto overflow-x-hidden rounded bg-gray-50 px-3 py-4 dark:bg-gray-800",
  },
  collapse: {
    button:
      "group flex w-full items-center rounded-lg p-2 text-base font-normal text-gray-900 transition duration-75 hover:bg-gray-100 dark:text-white dark:hover:bg-gray-700",
    icon: {
      base: "h-6 w-6 text-gray-500 transition duration-75 group-hover:text-gray-900 dark:text-gray-400 dark:group-hover:text-white",
      open: {
        off: "",
        on: "text-gray-900",
      },
    },
    label: {
      base: "ml-3 flex-1 whitespace-nowrap text-left",
      icon: {
        base: "h-6 w-6 transition delay-0 ease-in-out",
        open: {
          on: " rotate-180",
          off: "",
        },
      },
    },
    list: "space-y-2 py-2",
  },
  item: {
    base: "flex items-center rounded-lg p-2 text-base font-normal text-gray-900 hover:bg-gray-100 dark:text-white dark:hover:bg-gray-700",
    active: "bg-gray-100 dark:bg-gray-700",
    collapsed: {
      insideCollapse: "group w-full pl-8 transition duration-75",
      noIcon: "pl-2",
    },
    content: {
      base: "flex-1 whitespace-nowrap px-3",
    },
    icon: {
      base: "h-6 w-6 flex-shrink-0 text-gray-500 transition duration-75 group-hover:text-gray-900 dark:text-gray-400 dark:group-hover:text-white",
      active: "text-gray-700 dark:text-gray-100",
    },
  },
};

const Component: React.FC = () => {
  const { setOpenModal } = modalState();

  return (
    <Sidebar
      theme={customTheme}
      aria-label="Sidebar with multi-level dropdown example"
      className="[&>div]:bg-transparent [&>div]:p-0">
      <div className="flex h-full flex-col justify-between py-2">
        <div>
          <Sidebar.Items>
            <button className="text-black outline" onClick={setOpenModal}>
              click me
            </button>
            {/* <Suspense
              fallback={
                <div className="text-red-700">Loading navigation data...</div>
              }>
              <NavigationData />
            </Suspense> */}
          </Sidebar.Items>
        </div>
      </div>
    </Sidebar>
  );
};

export default Component;
