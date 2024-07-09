import {Link} from "@nextui-org/link";
import React from "react";
import { useSidebarContext } from "layouts/SideLayoutContext";
import clsx from "clsx";

interface Props {
  title: string;
  icon: React.ReactNode;
  isActive?: boolean;
  href?: string;
}

export const SidebarItem = ({ icon, title, isActive, href = "" }: Props) => {
  const { collapsed, setCollapsed } = useSidebarContext();

  const handleClick = () => {
    if (window.innerWidth < 768) {
      setCollapsed();
    }
  };
  return (
    <Link
      href={href}
      className="text-default-900 active:bg-none max-w-full"
    >
      <div
        className={clsx(
          isActive
            ? "bg-primary-400 [&_svg_path]:fill-white"
            : "hover:bg-default-200",
          "flex gap-2 w-full min-h-[48px] h-full items-center px-3.5  rounded-xl cursor-pointer transition-all duration-150 active:scale-[0.98]"
        )}
        onClick={handleClick}
      >
          {icon} 
        <span className={clsx(
            isActive
                ? "text-white"
                : "text-default-900",
            "font-semibold"
        )}>{title}</span>
      </div>
    </Link>
  );
};