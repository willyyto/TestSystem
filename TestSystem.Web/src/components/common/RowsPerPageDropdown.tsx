import React from "react";
import { Dropdown, DropdownTrigger, DropdownMenu, DropdownItem, Button } from "@heroui/react";
import {Icon} from "@iconify/react";

interface RowsPerPageDropdownProps {
    rowsPerPage: number;
    onRowsPerPageChange: (value: number) => void;
}

const RowsPerPageDropdown: React.FC<RowsPerPageDropdownProps> = ({ rowsPerPage, onRowsPerPageChange }) => {
    const [selectedKeys, setSelectedKeys] = React.useState(new Set([rowsPerPage.toString()]));

    const selectedValue = React.useMemo(
        () => Array.from(selectedKeys).join(", ").replaceAll("_", " "),
        [selectedKeys]
    );

    React.useEffect(() => {
        onRowsPerPageChange(Number(selectedValue));
    }, [selectedValue, onRowsPerPageChange]);

    return (
        <Dropdown>
            <DropdownTrigger>
                <Button variant="flat" size="sm" className="p-0"  endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>}>
                    {selectedValue}
                </Button>
            </DropdownTrigger>
            <DropdownMenu
                aria-label="Rows per page selection"
                variant="flat"
                disallowEmptySelection
                selectionMode="single"
                selectedKeys={selectedKeys}
                onSelectionChange={setSelectedKeys}
            >
                <DropdownItem key="5">5</DropdownItem>
                <DropdownItem key="10">10</DropdownItem>
                <DropdownItem key="15">15</DropdownItem>
            </DropdownMenu>
        </Dropdown>
    );
};

export default RowsPerPageDropdown;
