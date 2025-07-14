import {tv} from "@heroui/react";

export const SidebarWrapper = tv({
    base: "bg-background transition-transform h-full w-72 shrink-0 overflow-y-auto border-r border-divider flex-col py-6 px-3 " +
        // Mobile styles - hidden by default, overlay when open
        "fixed -translate-x-full z-[202] flex " +
        // Desktop styles - always visible and positioned
        "md:static md:translate-x-0",

    variants: {
        collapsed: {
            true: "translate-x-0", // Show on mobile when collapsed=true
        },
    },
});

export const Overlay = tv({
    base: "bg-[rgb(15_23_42/0.3)] fixed inset-0 z-[201] opacity-80 transition-opacity " +
        // Only show overlay on mobile
        "md:hidden",
});

export const Header = tv({
    base: "flex gap-8 items-center px-6",
});

export const Body = tv({
    base: "flex flex-col gap-2 mt-9 px-4",
});

export const Footer = tv({
    base: "flex flex-col gap-2 mt-9 px-5",
});

export const Sidebar = Object.assign(SidebarWrapper, {
    Header,
    Body,
    Overlay,
    Footer,
});