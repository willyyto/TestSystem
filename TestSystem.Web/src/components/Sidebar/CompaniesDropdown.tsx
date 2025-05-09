"use client";
import {Dropdown, DropdownItem, DropdownMenu, DropdownSection, DropdownTrigger,} from "@heroui/react";
import React, {useState} from "react";
import {BuildingStorefrontIcon, ChevronUpDownIcon} from "@heroicons/react/24/solid";

interface Company {
    name: string;
    location: string;
    logo: React.ReactNode;
}

export const CompaniesDropdown = () => {
    const [company, setCompany] = useState<Company>({
        name: "Acme Co.",
        location: "Palo Alto, CA",
        logo: <BuildingStorefrontIcon className="h-6 w-6"/>,
    });

    return (
        <Dropdown
            classNames={{
                base: "w-full min-w-[260px]",
            }}
        >
            <DropdownTrigger className="cursor-pointer">
                <div className="relative flex items-center gap-2 w-full px-3">
                    {company.logo}
                    <div className="flex flex-col gap-4 pr-4">
                        <h3 className="text-lg font-medium m-0 text-default-900 -mb-4 whitespace-nowrap">
                            {company.name}
                        </h3>
                        <span className="text-xs font-medium text-default-500">
                            {company.location}
                        </span>
                    </div>
                    <ChevronUpDownIcon className="absolute right-0 h-6 w-6 flex-shrink-0" />
                </div>
            </DropdownTrigger>
            <DropdownMenu
                onAction={(e) => {
                    if (e === "1") {
                        setCompany({
                            name: "Facebook",
                            location: "San Francisco, CA",
                            logo: <BuildingStorefrontIcon className="h-6 w-6"/>,
                        });
                    }
                    if (e === "2") {
                        setCompany({
                            name: "Instagram",
                            location: "Austin, TX",
                            logo: <BuildingStorefrontIcon className="h-6 w-6"/>,
                        });
                    }
                    if (e === "3") {
                        setCompany({
                            name: "Twitter",
                            location: "Brooklyn, NY",
                            logo: <BuildingStorefrontIcon className="h-6 w-6"/>,
                        });
                    }
                    if (e === "4") {
                        setCompany({
                            name: "Acme Co.",
                            location: "Palo Alto, CA",
                            logo: <BuildingStorefrontIcon className="h-6 w-6"/>,
                        });
                    }
                }}
                aria-label="Avatar Actions"
            >
                <DropdownSection title="Companies">
                    <DropdownItem
                        key="1"
                        startContent={<BuildingStorefrontIcon className="h-6 w-6"/>}
                        description="San Francisco, CA"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        Facebook
                    </DropdownItem>
                    <DropdownItem
                        key="2"
                        startContent={<BuildingStorefrontIcon className="h-6 w-6"/>}
                        description="Austin, TX"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        Instagram
                    </DropdownItem>
                    <DropdownItem
                        key="3"
                        startContent={<BuildingStorefrontIcon className="h-6 w-6"/>}
                        description="Brooklyn, NY"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        Twitter
                    </DropdownItem>
                    <DropdownItem
                        key="4"
                        startContent={<BuildingStorefrontIcon className="h-6 w-6"/>}
                        description="Palo Alto, CA"
                        classNames={{
                            base: "py-4",
                            title: "text-base font-semibold",
                        }}
                    >
                        Acme Co.
                    </DropdownItem>
                </DropdownSection>
            </DropdownMenu>
        </Dropdown>
    );
};
