import {format} from "date-fns";

export function capitalize(str: string) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

export function formatDate(dateString: string) {
    const date = new Date(dateString);
    return format(date, 'dd/MM/yyyy');
}

// utils/formatDuration.ts
export const formatDuration = (duration: string): string => {
    const [hours, minutes] = duration.split(':');
    return `${hours} hr, ${minutes} min`;
};
