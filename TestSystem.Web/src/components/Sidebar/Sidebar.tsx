import {Sidebar} from "components/Sidebar/SidebarStyles";
import {CompaniesDropdown} from "components/Sidebar/CompaniesDropdown";
import {DocumentDuplicateIcon, HomeIcon} from "@heroicons/react/24/solid";
import {SidebarItem} from "components/Sidebar/SidebarItem";
import {SidebarMenu} from "components/Sidebar/SidebarMenu";
import {useSidebarContext} from "layouts/SideLayoutContext";
import {useLocation} from "react-router-dom";
import {Link} from "@nextui-org/link";
import {UsersDropdown} from "./UserDropdown.tsx";
import {CollapseItems} from "./CollapseItems.tsx";
import {Input} from "@nextui-org/react";
import {MagnifyingGlassIcon} from "@heroicons/react/24/outline";
import React from "react";

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
                        <div className="flex items-center pl-3 py-2 border-1 rounded-2xl border-default-200">
                            <CompaniesDropdown/>
                        </div>
                        <Input
                            isClearable
                            className="w-full pb-10 pt-2"
                            size="lg"
                            placeholder="Search"
                            startContent={<MagnifyingGlassIcon className=" w-5 h-5 text-gray-500"/>}
                        />
                        
                        <SidebarItem
                            title="Dashboard"
                            icon={<HomeIcon className="h-5 w-5" />}
                            isActive={pathname === "/admin/dashboard"}
                            href="/admin/dashboard"
                        />
                        <SidebarItem
                            title="Test"
                            icon={<DocumentDuplicateIcon className="h-5 w-5" />}
                            isActive={pathname === "/admin/test"}
                            href="/admin/test"
                        />
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
                        <UsersDropdown/>
                    </div>
                </div>
            </div>
        </aside>
    );
};