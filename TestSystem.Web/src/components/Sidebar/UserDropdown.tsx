"use client";
import {
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownSection,
    DropdownTrigger,
} from "@nextui-org/react";
import React, { useState } from "react";
import { UserIcon, ChevronUpDownIcon } from "@heroicons/react/24/solid";

interface User {
    name: string;
    role: string;
    logo: React.ReactNode;
}

export const UsersDropdown = () => {
    const [User, setUser] = useState<User>({
        name: "John Smith",
        role: "Admin",
        logo: <UserIcon className="h-5 w-5" />,
    });
    return (
        <Dropdown
            classNames={{
                base: "w-full min-w-[260px]",
            }}
        >
            <DropdownTrigger className="cursor-pointer">
                <div className="flex items-center gap-2">
                    {User.logo}
                    <div className="flex flex-col gap-4 pr-4">
                        <h3 className="text-xl font-medium m-0 text-default-900 -mb-4 whitespace-nowrap">
                            {User.name}
                        </h3>
                        <span className="text-xs font-medium text-default-500">
                          {User.role}
                        </span>
                    </div>
                    <ChevronUpDownIcon className="h-8 w-8 flex-shrink-0" />
                </div>
            </DropdownTrigger>
            <DropdownMenu
                onAction={(e) => {
                    if (e === "1") {
                        setUser({
                            name: "John Doe",
                            role: "Admin",
                            logo: <UserIcon className="h-5 w-5" />,
                        });
                    }
                    if (e === "2") {
                        setUser({
                            name: "Whitney Fitts",
                            role: "User",
                            logo: <UserIcon className="h-5 w-5" />,
                        });
                    }
                    if (e === "3") {
                        setUser({
                            name: "Henry Lawson",
                            role: "User",
                            logo: <UserIcon className="h-5 w-5" />,
                        });
                    }
                }}
                aria-label="Avatar Actions"
            >
                <DropdownSection title="Companies">
                    <DropdownItem
                        key="1"
                        startContent={<UserIcon className="h-5 w-5" />}
                        description="Admin"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        John Doe
                    </DropdownItem>
                    <DropdownItem
                        key="2"
                        startContent={<UserIcon className="h-5 w-5"  />}
                        description="User"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        Whitney Fitts
                    </DropdownItem>
                    <DropdownItem
                        key="3"
                        startContent={<UserIcon className="h-5 w-5" />}
                        description="User"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        Henry Lawson
                    </DropdownItem>

                </DropdownSection>
            </DropdownMenu>
        </Dropdown>
    );
};