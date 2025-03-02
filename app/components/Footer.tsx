import { Footer as FlowbiteFooter } from "flowbite-react";
import {
  BsDribbble,
  BsFacebook,
  BsGithub,
  BsInstagram,
  BsTwitter,
} from "react-icons/bs";

const Footer = () => {
  return (
    <FlowbiteFooter container>
      <div className="w-full sm:flex sm:items-center sm:justify-between">
        <FlowbiteFooter.Copyright href="#" by="Flowbite™" year={2022} />
        <div className="mt-4 flex space-x-6 sm:mt-0 sm:justify-center">
          <FlowbiteFooter.Icon href="#" icon={BsFacebook} />
          <FlowbiteFooter.Icon href="#" icon={BsInstagram} />
          <FlowbiteFooter.Icon href="#" icon={BsTwitter} />
          <FlowbiteFooter.Icon href="#" icon={BsGithub} />
          <FlowbiteFooter.Icon href="#" icon={BsDribbble} />
        </div>
      </div>
    </FlowbiteFooter>
  );
};

export default Footer;
