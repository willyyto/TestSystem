import { Tooltip } from "@nextui-org/react";
import React from "react";
import { Test } from"types/Interfaces";
import {Icon} from "@iconify/react";
import {formatDate} from "utils/utils.tsx";
import {useNavigate} from "react-router-dom";

interface Props {
    test: Test;
    columnKey: string | React.Key;
}

export const RenderCell = ({ test, columnKey }: Props) => {
    const navigate = useNavigate();
    if (!test || !columnKey || columnKey === "questions") {
        return null;
    }
    const cellValue = test[columnKey];

    switch (columnKey) {
        case "startDate":
            return (
                <div className="flex items-center gap-4 ">
                    {formatDate(cellValue)}
                </div>
            );
        case "endDate":
            return (
                <div className="flex items-center gap-4 ">
                    {formatDate(cellValue)}
                </div>
            );
        case "actions":
            return (
                <div className="flex items-center gap-4 ">
                    <div>
                        <Tooltip content="Start Test" color="success">
                            <button onClick={() => navigate(`/quiz/${test.id}`)}>
                                <Icon icon="solar:alarm-bold" className="h-6 w-6 text-success"/>
                            </button>
                        </Tooltip>
                    </div>
                </div>
            );
        default:
            return cellValue;
    }
};