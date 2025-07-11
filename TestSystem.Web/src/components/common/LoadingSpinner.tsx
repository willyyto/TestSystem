import {Spinner} from '@heroui/react'
import {cn} from "utils/utils.tsx";

interface LoadingSpinnerProps {
    size?: 'sm' | 'md' | 'lg'
    fullScreen?: boolean
    text?: string
    className?: string
}

export const LoadingSpinner = ({
                                   size = 'lg',
                                   fullScreen = true,
                                   text,
                                   className
                               }: LoadingSpinnerProps) => {
    const content = (
        <div className={cn("flex flex-col items-center justify-center gap-4",className)}>
            <Spinner size={size} color="primary" classNames={{ circle1: "border-b-primary", circle2: "border-b-primary", }} />
            {text && ( <p className="text-sm text-gray-600 dark:text-gray-400 animate-pulse"> {text} </p>
            )}
        </div>
    )

    if (fullScreen) {
        return (
            <div className="fixed inset-0 bg-white/80 dark:bg-gray-900/80 backdrop-blur-sm z-50 flex items-center justify-center">
                {content}
            </div>
        )
    }

    return content
}
