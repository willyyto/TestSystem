import { cn, Radio, RadioProps } from "@heroui/react";
import React, { ReactNode } from "react";

interface CustomRadioProps extends RadioProps {
    children: ReactNode;
}

export const CustomTestRadio: React.FC<CustomRadioProps> = ({children, ...otherProps }) => {
    const classNames = cn(
        "inline-flex m-0 bg-content1 hover:bg-content2 items-center justify-between",
        "flex-row-reverse max-w-[300px] cursor-pointer rounded-lg gap-4 p-4 border-2 border-transparent data-[selected=true]:border-primary"
    );

    return (
        <Radio {...otherProps} classNames={{ base: classNames }}>
            {children}
        </Radio>
    );
};
