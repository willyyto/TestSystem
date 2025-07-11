"use client";
import React, {useEffect, useState} from "react";
import {
    Avatar,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownSection,
    DropdownTrigger,
    Skeleton,
} from "@heroui/react";
import {Icon} from "@iconify/react";
import {useAuth} from "../../auth/AuthContext.tsx";
import {capitalize} from "utils/utils.tsx"; // Adjust the import path as needed

interface User {
    name: string;
    role: string;
    logo: React.ReactNode;
}

export const UsersDropdown = () => {
    const {userRole, userGivenName} = useAuth();
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        if (userGivenName && userRole) {
            setUser({
                name: capitalize(userGivenName),
                role: capitalize(userRole),
                logo: <Avatar src="https://i.pravatar.cc/150?u=a042581f4e29026704d" size="sm"/>,
            });
            setLoading(false);
        } else {
            setLoading(true);
        }
    }, [userGivenName, userRole]);

    return (
        <Dropdown
            classNames={{
                base: "w-full min-w-[260px]",
            }}
        >
            <DropdownTrigger className="cursor-pointer">
                {loading ? (
                    <div className="relative flex items-center gap-2 w-full">
                        <Skeleton className="w-10 h-10 rounded-full"/>
                        <div className="flex flex-col gap-3 pl-1 pr-4">
                            <Skeleton className="w-24 h-4"/>
                            <Skeleton className="w-16 h-3"/>
                        </div>
                    </div>
                ) : (
                    <div className="relative flex items-center gap-2 w-full">
                        {user?.logo}
                        <div className="flex flex-col gap-3 pl-1 pr-4">
                            <h3 className="text-md font-medium m-0 text-default-700 -mb-4 whitespace-nowrap">
                                {user?.name}
                            </h3>
                            <span className="text-xs font-light text-default-500">
                                {user?.role}
                            </span>
                        </div>
                        <Icon icon="solar:alt-arrow-down-outline" className="absolute right-0 h-4 w-4 flex-shrink-0"/>
                    </div>
                )}
            </DropdownTrigger>
            <DropdownMenu aria-label="Avatar Actions">
                <DropdownSection title="Users">
                    <DropdownItem
                        color="primary"
                        startContent={<Icon icon="solar:user-outline" className="h-5 w-5"/>}
                        classNames={{
                            base: "py-2",
                            title: "text-base",
                        }}
                    >
                        Profile
                    </DropdownItem>
                    <DropdownItem
                        color="danger"
                        startContent={<Icon icon="solar:minus-circle-outline" className="h-5 w-5"/>}
                        classNames={{
                            base: "py-2",
                            title: "text-base",
                        }}
                        href="/logout"
                    >
                        Log Out
                    </DropdownItem>
                </DropdownSection>
            </DropdownMenu>
        </Dropdown>
    );
};
