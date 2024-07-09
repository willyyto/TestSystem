"use client";

import React from "react";
import { useLockedBody } from "../hooks/useBodyLock";
import { SidebarWrapper } from "components/Sidebar/Sidebar";
import { SidebarContext } from "./SideLayoutContext";
import {DefaultLayout} from "./index.tsx";

interface Props {
    children: React.ReactNode;
}

export const SideLayout = ({ children }: Props) => {
    const [sidebarOpen, setSidebarOpen] = React.useState(false);
    const [_, setLocked] = useLockedBody(false);
    const handleToggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
        setLocked(!sidebarOpen);
    };

    return (
        <DefaultLayout>
            <SidebarContext.Provider
                value={{
                    collapsed: sidebarOpen,
                    setCollapsed: handleToggleSidebar,
                }}>
                <section className='flex'>
                    <SidebarWrapper />
                    <div className="relative flex flex-col flex-1 overflow-y-auto overflow-x-hidden">
                        <main className="container mx-auto max-w-7xl px-6 flex-grow pt-16">
                            {children}
                        </main>
                    </div>
                </section>
            </SidebarContext.Provider>
        </DefaultLayout>
        
    );
};