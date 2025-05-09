import {Button, Tooltip} from "@heroui/react";
import React from "react";
import {Test} from "types/Interfaces";
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
                        <Tooltip content="Start Test" color="success" className="text-white">
                            <Button color="success" size="sm" onClick={() => navigate(`/quiz/${test.id}`)}
                                    endContent={<Icon icon="solar:alarm-bold" className="h-4 w-4 text-white"/>}
                                    className="text-white">
                                Start
                            </Button>
                        </Tooltip>
                    </div>
                </div>
            );
        default:
            return cellValue;
    }
};