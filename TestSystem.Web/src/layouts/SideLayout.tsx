"use client";
import React, { useEffect } from "react";
import {useLockedBody} from "../hooks/useBodyLock";
import {SidebarWrapper} from "components/Sidebar/Sidebar";
import {SidebarContext} from "./SideLayoutContext";
import {DefaultLayout} from "./index.tsx";

interface Props {
    children: React.ReactNode;
}

export const SideLayout = ({ children }: Props) => {
    const [sidebarOpen, setSidebarOpen] = React.useState(false);
    const [_, setLocked] = useLockedBody(false);
    const [isMobile, setIsMobile] = React.useState(false);

    // Check if we're on mobile
    useEffect(() => {
        const checkMobile = () => {
            const mobile = window.innerWidth < 768;
            setIsMobile(mobile);

            // Auto-close sidebar on mobile when switching from desktop
            if (mobile && sidebarOpen) {
                setSidebarOpen(false);
                setLocked(false);
            }
        };

        // Check on mount
        checkMobile();

        // Add resize listener
        window.addEventListener('resize', checkMobile);

        return () => {
            window.removeEventListener('resize', checkMobile);
        };
    }, [sidebarOpen, setLocked]);

    const handleToggleSidebar = () => {
        setSidebarOpen(!sidebarOpen);
        // Only lock body on mobile
        if (isMobile) {
            setLocked(!sidebarOpen);
        }
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
                        <main className="container mx-auto max-w-full px-6 flex-grow pt-16 md:pt-16">
                            {/* Add top padding on mobile to account for the toggle button */}
                            <div className="my-14 lg:px-6 mx-auto w-full flex flex-col gap-4 pt-12 md:pt-0">
                                {children}
                            </div>
                        </main>
                    </div>
                </section>
            </SidebarContext.Provider>
        </DefaultLayout>
    );
};