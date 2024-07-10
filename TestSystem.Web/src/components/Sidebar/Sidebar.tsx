import {Sidebar} from "components/Sidebar/SidebarStyles";
import {SidebarItem} from "components/Sidebar/SidebarItem";
import {useSidebarContext} from "layouts/SideLayoutContext";
import {useLocation} from "react-router-dom";
import {Input, Link} from "@nextui-org/react";
import {Icon} from '@iconify/react';
import {UsersDropdown} from "./UserDropdown.tsx";
import AppRoutes from "navigation/AppRoutes.ts";
import {SidebarMenu} from "./SidebarMenu.tsx";
import {ThemeSwitch} from "../Theme/theme-switch.tsx";

export const SidebarWrapper = () => {
    const pathname = useLocation().pathname;
    const {collapsed, setCollapsed} = useSidebarContext();

    return (
        <aside className="h-screen z-[20] sticky top-0">
            {collapsed ? (
                <div className={Sidebar.Overlay()} onClick={setCollapsed}/>
            ) : null}
            <div
                className={Sidebar({
                    collapsed: collapsed,
                })}
            >
                <div className={Sidebar.Header()}>
                    <Link
                        className="flex justify-start items-center gap-1"
                        color="foreground"
                        href="/"
                    >
                        <p className="font-bold text-inherit text-xl">TESTSYSTEM <span className="text-sm text-primary-500">ADMIN</span></p>
                    </Link>
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

                        <SidebarMenu title="Overview">
                            <SidebarItem
                                title="Home"
                                icon={<Icon icon="solar:home-2-outline" className="h-6 w-6"/>}
                                isActive={pathname === AppRoutes.root}
                                href={AppRoutes.root}
                            />
                            <SidebarItem
                                title="Dashboard"
                                icon={<Icon icon="solar:widget-2-outline" className="h-6 w-6"/>}
                                isActive={pathname === AppRoutes.admindashboard}
                                href={AppRoutes.admindashboard}
                            />
                            <SidebarItem
                                title="Test"
                                icon={<Icon icon="solar:checklist-minimalistic-outline" className="h-6 w-6"/>}
                                isActive={pathname === AppRoutes.admintest}
                                href={AppRoutes.admintest}
                            />
                            <SidebarItem
                                title="Company"
                                icon={<Icon icon="solar:buildings-outline" className="h-6 w-6"/>}
                                isActive={pathname === "#"}
                                href="#"
                            />
                            <SidebarItem
                                title="Result"
                                icon={<Icon icon="solar:archive-minimalistic-outline" className="h-6 w-6"/>}
                                isActive={pathname === AppRoutes.dashboard}
                                href={AppRoutes.dashboard}
                            />
                        </SidebarMenu>
                        <SidebarMenu title="General">
                            <SidebarItem
                                title="User View"
                                icon={<Icon icon="solar:user-id-outline" className="h-6 w-6"/>}
                                isActive={pathname === AppRoutes.dashboard}
                                href={AppRoutes.dashboard}
                            />
                            <SidebarItem
                                title="Analytics"
                                icon={<Icon icon="solar:chart-outline" className="h-6 w-6"/>}
                                isActive={pathname === "#"}
                                href="#"
                            />
                            <SidebarItem
                                title="Account Management"
                                icon={<Icon icon="solar:users-group-rounded-outline" className="h-6 w-6"/>}
                                isActive={pathname === "#"}
                                href="#"
                            />
                            
                        </SidebarMenu>


                        {/*<CollapseItems
                            icon={<HomeIcon className="h-5 w-5" />}
                            items={["Banks Accounts", "Credit Cards", "Loans"]}
                            title="Balances"
                        />*/}
                        {/*<SidebarMenu title="Main Menu">
                            <SidebarItem
                                isActive={pathname === "/accounts"}
                                title="Accounts"
                                icon={<HomeIcon className="h-5 w-5"/>}
                                href="accounts"
                            />
                        </SidebarMenu>*/}

                    </div>
                    <div className={Sidebar.Footer()}>
                        <ThemeSwitch isSidebar={true}/>
                        <SidebarItem
                            title="Settings"
                            size="38px"
                            icon={<Icon icon="solar:settings-outline" className="h-6 w-6"/>}
                            isActive={pathname === "#"}
                            href="#"
                        />
                        <SidebarItem
                            title="Help & Information"
                            size="38px"
                            icon={<Icon icon="solar:info-square-outline" className="h-6 w-6"/>}
                            isActive={pathname === "#"}
                            href="#"
                        />
                        <SidebarItem
                            title="Log Out"
                            size="38px"
                            color="danger"
                            icon={<Icon icon="solar:minus-circle-outline" className="h-6 w-6"/>}
                            isActive={pathname === "#"}
                            href="/logout"
                        />
                    </div>
                </div>
            </div>
        </aside>
    );
};