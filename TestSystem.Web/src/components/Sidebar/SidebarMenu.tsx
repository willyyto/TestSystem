import React from "react";

interface Props {
    title: string;
    children?: React.ReactNode;
}

export const SidebarMenu = ({ title, children }: Props) => {
    return (
        <div className="flex gap-2 flex-col py-1">
            <span className="text-xs text-tiny font-light text-foreground-500">{title}</span>
            {children}
        </div>
    );
};