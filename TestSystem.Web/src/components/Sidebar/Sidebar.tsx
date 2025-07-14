import {Sidebar} from "components/Sidebar/SidebarStyles";
import {SidebarItem} from "components/Sidebar/SidebarItem";
import {useSidebarContext} from "layouts/SideLayoutContext";
import {useLocation} from "react-router-dom";
import {Input, Link} from "@heroui/react";
import {Icon} from '@iconify/react';
import {UsersDropdown} from "./UserDropdown";
import {SidebarMenu} from "./SidebarMenu";
import {ThemeSwitch} from "components/Theme/theme-switch";
import {siteConfig} from "config/site.ts";

export const SidebarWrapper = () => {
    const pathname = useLocation().pathname;
    const {collapsed, setCollapsed} = useSidebarContext();

    return (
        <>
            {/* Mobile menu button */}
            <div className="md:hidden fixed top-4 left-4 z-[203]">
                <button
                    onClick={setCollapsed}
                    className="p-2 rounded-lg bg-background border border-divider shadow-sm"
                    aria-label="Toggle menu"
                >
                    <Icon
                        icon={collapsed ? "solar:close-square-outline" : "solar:hamburger-menu-outline"}
                        className="w-6 h-6"
                    />
                </button>
            </div>

            <aside className="h-screen z-[20] sticky top-0">
                {/* Mobile overlay - only show on mobile when sidebar is open */}
                {collapsed && (
                    <div className={Sidebar.Overlay()} onClick={setCollapsed}/>
                )}

                <div className={Sidebar({ collapsed: collapsed })}>
                    <div className={Sidebar.Header()}>
                        <Link
                            className="flex justify-start items-center gap-1"
                            color="foreground"
                            href="/"
                        >
                            <p className="font-bold text-inherit text-xl">
                                TESTSYSTEM
                            </p>
                        </Link>

                        {/* Mobile close button in header */}
                        <button
                            onClick={setCollapsed}
                            className="md:hidden p-2 rounded-lg hover:bg-default-100 ml-auto"
                            aria-label="Close menu"
                        >
                            <Icon icon="solar:close-square-outline" className="w-5 h-5" />
                        </button>
                    </div>

                    <div className="flex flex-col justify-between h-full">
                        <div className={Sidebar.Body()}>
                            <div className="flex items-center">
                                <UsersDropdown/>
                            </div>

                            <Input
                                isClearable
                                className="w-full pt-2 pb-6"
                                size="md"
                                placeholder="Search..."
                                startContent={<Icon icon="solar:minimalistic-magnifer-outline" className="w-4 h-4"/>}
                            />

                            {/* Render menu items from siteConfig */}
                            {siteConfig.sidebarItems.map((menu) => (
                                <SidebarMenu key={menu.title} title={menu.title}>
                                    {menu.items.map((item) => (
                                        <SidebarItem
                                            key={item.href}
                                            title={item.title}
                                            icon={<Icon icon={item.icon} className="h-6 w-6"/>}
                                            isActive={pathname === item.href}
                                            href={item.href}
                                        />
                                    ))}
                                </SidebarMenu>
                            ))}
                        </div>

                        <div className={Sidebar.Footer()}>
                            <ThemeSwitch isSidebar={true}/>
                            <SidebarItem
                                title="Settings"
                                icon={<Icon icon="solar:settings-outline" className="h-6 w-6"/>}
                                isActive={pathname === "/settings"}
                                href="/settings"
                            />
                            <SidebarItem
                                title="Help & Information"
                                icon={<Icon icon="solar:info-square-outline" className="h-6 w-6"/>}
                                isActive={pathname === "/help"}
                                href="/help"
                            />
                        </div>
                    </div>
                </div>
            </aside>
        </>
    );
};