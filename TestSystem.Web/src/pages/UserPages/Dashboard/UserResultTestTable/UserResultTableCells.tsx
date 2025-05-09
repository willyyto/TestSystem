import { Tooltip } from "@heroui/react";
import React from "react";
import { Result } from"types/Interfaces";
import {Icon} from "@iconify/react";
import {formatDate} from "utils/utils.tsx";
import {useNavigate} from "react-router-dom";

interface Props {
    result: Result;
    columnKey: string | React.Key;
}

export const UserResultTableCells = ({ result, columnKey }: Props) => {
    const navigate = useNavigate();
    if (!result || !columnKey || columnKey === "test") {
        return null;
    }
    const cellValue = result[columnKey];

    switch (columnKey) {
        case "completedDate":
            return (
                <div className="flex items-center gap-4 ">
                    {formatDate(cellValue)}
                </div>
            );
        case "actions":
            return (
                <div className="flex items-center gap-4 ">
                    <div>
                        <Tooltip content="View Result" color="secondary">
                            <button onClick={() => navigate(`/result/${result.id}`)}>
                                <Icon icon="solar:eye-linear" className="h-6 w-6 text-default"/>
                            </button>
                        </Tooltip>
                    </div>
                </div>
            );
        default:
            return cellValue;
    }
};