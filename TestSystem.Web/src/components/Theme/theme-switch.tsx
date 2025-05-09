import {FC, useEffect, useState} from "react";
import {useTheme as useNextTheme} from "next-themes";
import {Icon} from "@iconify/react";
import {Link} from "@heroui/link";
import {cn, Switch} from "@heroui/react";

export interface ThemeSwitchProps {
    className?: string;
    isSidebar?: boolean;
}

export const ThemeSwitch: FC<ThemeSwitchProps> = ({className, isSidebar = false}) => {
    const { theme, setTheme } = useNextTheme();
    const [isMounted, setIsMounted] = useState(false);

    useEffect(() => {
        setIsMounted(true);
    }, []);

    // Prevent Hydration Mismatch
    if (!isMounted) return <div className="w-6 h-6" />;

    const toggleTheme = () => {
        setTheme(theme === "light" ? "dark" : "light");
    };

    return (
        <>
            {isSidebar == true ? (
                <Link
                    className="text-default-800 active:bg-none max-w-full"
                >
                    <Switch
                        color="warning"
                        defaultSelected={theme === "light"}
                        onChange={toggleTheme}
                        classNames={{
                            base: cn(
                                "inline-flex flex-row-reverse w-full max-w-md bg-content1 hover:bg-content2 items-center",
                                "justify-between cursor-pointer rounded-lg gap-2 p-4 border-2 border-transparent",
                                "data-[selected=true]:border-none"
                            ),
                            wrapper: "p-0 h-4 overflow-visible",
                            thumb: cn(
                                "w-6 h-6 border-2 shadow-lg",
                                "group-data-[hover=true]:border-warning",
                                // selected
                                "group-data-[selected=true]:ml-6",
                                // pressed
                                "group-data-[pressed=true]:w-7",
                                "group-data-[selected]:group-data-[pressed]:ml-4"
                            )
                        }}
                    >
                        <div className="flex flex-col gap-1">
                            <p className="text-medium">{theme === "light" ? (

                                "Dark Mode"
                            ) : (
                                "Light Mode"
                            )}</p>
                        </div>
                    </Switch>
                    {/*<div
                        className="hover:bg-default-100 flex gap-2 w-full min-h-[38px] h-full items-center px-3  rounded-xl cursor-pointer transition-all duration-150 active:scale-[0.98]"
                        onClick={toggleTheme}
                    >
                        {theme === "light" ? (
                            <Icon icon="solar:moon-bold" className="h-7 w-7"/>
                        ) : (
                            <Icon icon="solar:sun-2-bold" className="h-7 w-7"/>
                        )}
                        <span className="text-default-800 font-normal">{theme === "light" ? (
                            "Dark Mode"
                        ) : (
                            "Light Mode"
                        )}</span>
                    </div>*/}
                </Link>
            ) : (
                <button
                    onClick={toggleTheme}
                    className={`px-px transition-opacity hover:opacity-80 cursor-pointer ${className}`}
                >
                    {theme === "light" ? (
                        <Icon icon="solar:moon-bold" className="h-7 w-7"/>
                    ) : (
                        <Icon icon="solar:sun-2-bold" className="h-7 w-7"/>
                    )}
                </button>
            )}
        </>


    );
};
