import { FC, useState, useEffect } from "react";
import { useTheme as useNextTheme } from "next-themes";
import { SunIcon, MoonIcon } from "@heroicons/react/24/solid"; // Make sure to import your icons

export interface ThemeSwitchProps {
    className?: string;
}

export const ThemeSwitch: FC<ThemeSwitchProps> = ({ className }) => {
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
        <button
            onClick={toggleTheme}
            className={`px-px transition-opacity hover:opacity-80 cursor-pointer ${className}`}
        >
            {theme === "light" ? (
                <MoonIcon  className="h-6 w-6" />
            ) : (
                <SunIcon className="h-7 w-7" />
            )}
        </button>
    );
};
