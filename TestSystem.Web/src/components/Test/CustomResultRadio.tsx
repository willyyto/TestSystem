import {cn, Radio, RadioProps} from "@nextui-org/react";
import React, {ReactNode} from "react";

// Define the interface for the props
interface CustomRadioProps extends RadioProps {
    children: ReactNode;
    isCorrect?: boolean;
}

export const CustomResultRadio: React.FC<CustomRadioProps> = ({children, isCorrect, ...otherProps}) => {
    const classNames = cn(
        "inline-flex m-0 bg-content1 hover:bg-content2 items-center justify-between",
        "flex-row-reverse max-w-[300px] cursor-pointer rounded-lg gap-4 p-4 border-2 border-transparent",
        isCorrect
            ? "data-[selected=true]:border-success"
            : "data-[selected=true]:border-danger"
    );

    return (
        <Radio {...otherProps} classNames={{ base: classNames }}>
            {children}
        </Radio>
    );
};
