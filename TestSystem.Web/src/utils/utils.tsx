import {format} from "date-fns";

export function capitalize(str: string) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

export function formatDate(dateString: string) {
    const date = new Date(dateString);
    return format(date, 'do MMMM yyyy');
}