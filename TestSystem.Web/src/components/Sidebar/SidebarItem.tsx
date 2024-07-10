import {Link} from "@nextui-org/link";
import React from "react";
import {useSidebarContext} from "layouts/SideLayoutContext";
import clsx from "clsx";

interface Props {
  title: string;
  icon: React.ReactNode;
  color?: string;
  size?: string;
  isActive?: boolean;
  href?: string;
}

export const SidebarItem = ({ icon, title, color = "default-100", size = "46px", isActive, href = "" }: Props) => {
  const { collapsed, setCollapsed } = useSidebarContext();
  const isColorDefault = color === "default-100";

  const handleClick = () => {
    if (window.innerWidth < 768) {
      setCollapsed();
    }
  };

  const containerClasses = clsx(
      isActive
          ? "bg-primary-400 [&_svg_path]:fill-white text-white"
          : `hover:bg-${color}`,
      "flex gap-2 w-full",
      `min-h-[${size}] h-full items-center px-3 rounded-xl cursor-pointer transition-all duration-150 active:scale-[0.98]`,
      !isColorDefault && "group hover:text-white"
  );

  const iconClasses = clsx(!isColorDefault && "group-hover:text-white");

  const textClasses = clsx(
      isActive ? "text-white" : "text-default-800",
      !isColorDefault && "group-hover:text-white",
      "font-normal"
  );

  return (
      <Link href={href} className="text-default-800 active:bg-none max-w-full">
        <div className={containerClasses} onClick={handleClick}>
          <div className={iconClasses}>{icon}</div>
          <span className={textClasses}>{title}</span>
        </div>
      </Link>
  );
};
