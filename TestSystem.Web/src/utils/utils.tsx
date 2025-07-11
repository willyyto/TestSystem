import { type ClassValue, clsx } from 'clsx'
import { twMerge } from 'tailwind-merge'
import { format, formatDistanceToNow, isValid, parseISO } from 'date-fns'


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

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs))
}

// Date formatting utilities
export const formatDateD = (date: string | Date, formatString = 'PPP'): string => {
    try {
        const dateObj = typeof date === 'string' ? parseISO(date) : date
        return isValid(dateObj) ? format(dateObj, formatString) : 'Invalid date'
    } catch {
        return 'Invalid date'
    }
}

export const formatRelativeTime = (date: string | Date): string => {
    try {
        const dateObj = typeof date === 'string' ? parseISO(date) : date
        return isValid(dateObj) ? formatDistanceToNow(dateObj, { addSuffix: true }) : 'Invalid date'
    } catch {
        return 'Invalid date'
    }
}

// Duration formatting
export const formatDurationD = (duration: string | number): string => {
    try {
        if (typeof duration === 'string') {
            // Parse ISO 8601 duration format (e.g., "PT1H30M")
            const match = duration.match(/PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?/)
            if (match) {
                const hours = parseInt(match[1] || '0')
                const minutes = parseInt(match[2] || '0')
                const seconds = parseInt(match[3] || '0')

                const parts = []
                if (hours > 0) parts.push(`${hours}h`)
                if (minutes > 0) parts.push(`${minutes}m`)
                if (seconds > 0) parts.push(`${seconds}s`)

                return parts.join(' ') || '0s'
            }
        }

        // Handle duration in seconds
        const totalSeconds = typeof duration === 'string' ? parseInt(duration) : duration
        const hours = Math.floor(totalSeconds / 3600)
        const minutes = Math.floor((totalSeconds % 3600) / 60)
        const seconds = totalSeconds % 60

        const parts = []
        if (hours > 0) parts.push(`${hours}h`)
        if (minutes > 0) parts.push(`${minutes}m`)
        if (seconds > 0) parts.push(`${seconds}s`)

        return parts.join(' ') || '0s'
    } catch {
        return 'Invalid duration'
    }
}

// Score utilities
export const getScoreColor = (score: number): 'success' | 'warning' | 'danger' => {
    if (score >= 80) return 'success'
    if (score >= 60) return 'warning'
    return 'danger'
}

export const getGradeLetter = (score: number): string => {
    if (score >= 90) return 'A'
    if (score >= 80) return 'B'
    if (score >= 70) return 'C'
    if (score >= 60) return 'D'
    return 'F'
}

// File utilities
export const formatFileSize = (bytes: number): string => {
    const sizes = ['Bytes', 'KB', 'MB', 'GB']
    if (bytes === 0) return '0 Bytes'
    const i = Math.floor(Math.log(bytes) / Math.log(1024))
    return Math.round(bytes / Math.pow(1024, i) * 100) / 100 + ' ' + sizes[i]
}

export const getFileExtension = (filename: string): string => {
    return filename.slice((filename.lastIndexOf('.') - 1 >>> 0) + 2)
}

export const isImageFile = (filename: string): boolean => {
    const imageExtensions = ['jpg', 'jpeg', 'png', 'gif', 'webp', 'svg']
    const extension = getFileExtension(filename).toLowerCase()
    return imageExtensions.includes(extension)
}

export const isVideoFile = (filename: string): boolean => {
    const videoExtensions = ['mp4', 'avi', 'mov', 'wmv', 'flv', 'webm']
    const extension = getFileExtension(filename).toLowerCase()
    return videoExtensions.includes(extension)
}

export const isAudioFile = (filename: string): boolean => {
    const audioExtensions = ['mp3', 'wav', 'ogg', 'aac', 'flac']
    const extension = getFileExtension(filename).toLowerCase()
    return audioExtensions.includes(extension)
}

// String utilities
export const truncateText = (text: string, maxLength: number): string => {
    if (text.length <= maxLength) return text
    return text.slice(0, maxLength) + '...'
}

export const slugify = (text: string): string => {
    return text
        .toLowerCase()
        .replace(/[^\w ]+/g, '')
        .replace(/ +/g, '-')
}

export const capitalizeFirst = (text: string): string => {
    return text.charAt(0).toUpperCase() + text.slice(1)
}

export const camelCaseToTitle = (text: string): string => {
    return text
        .replace(/([A-Z])/g, ' $1')
        .replace(/^./, str => str.toUpperCase())
        .trim()
}

// Number utilities
export const formatNumber = (num: number, decimals = 0): string => {
    return new Intl.NumberFormat('en-US', {
        minimumFractionDigits: decimals,
        maximumFractionDigits: decimals,
    }).format(num)
}

export const formatPercentage = (value: number, total: number): string => {
    if (total === 0) return '0%'
    return `${Math.round((value / total) * 100)}%`
}

// Validation utilities
export const isValidEmail = (email: string): boolean => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return emailRegex.test(email)
}

export const isValidUrl = (url: string): boolean => {
    try {
        new URL(url)
        return true
    } catch {
        return false
    }
}

// Array utilities
export const groupBy = <T, K extends keyof any>(
    list: T[],
    getKey: (item: T) => K
): Record<K, T[]> => {
    return list.reduce((previous, currentItem) => {
        const group = getKey(currentItem)
        if (!previous[group]) previous[group] = []
        previous[group].push(currentItem)
        return previous
    }, {} as Record<K, T[]>)
}

export const unique = <T>(array: T[]): T[] => {
    return Array.from(new Set(array))
}

export const chunk = <T>(array: T[], size: number): T[][] => {
    const chunks = []
    for (let i = 0; i < array.length; i += size) {
        chunks.push(array.slice(i, i + size))
    }
    return chunks
}

// Local storage utilities
export const getStorageItem = <T>(key: string, defaultValue: T): T => {
    try {
        const item = localStorage.getItem(key)
        return item ? JSON.parse(item) : defaultValue
    } catch {
        return defaultValue
    }
}

export const setStorageItem = <T>(key: string, value: T): void => {
    try {
        localStorage.setItem(key, JSON.stringify(value))
    } catch (error) {
        console.warn('Failed to save to localStorage:', error)
    }
}

export const removeStorageItem = (key: string): void => {
    try {
        localStorage.removeItem(key)
    } catch (error) {
        console.warn('Failed to remove from localStorage:', error)
    }
}

// Debounce utility
export const debounce = <T extends (...args: any[]) => any>(
    func: T,
    wait: number
): ((...args: Parameters<T>) => void) => {
    let timeout: NodeJS.Timeout
    return (...args: Parameters<T>) => {
        clearTimeout(timeout)
        timeout = setTimeout(() => func(...args), wait)
    }
}

// Random utilities
export const generateId = (): string => {
    return Math.random().toString(36).substr(2, 9)
}

export const generatePassword = (length = 12): string => {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*'
    let password = ''
    for (let i = 0; i < length; i++) {
        password += chars.charAt(Math.floor(Math.random() * chars.length))
    }
    return password
}

// Color utilities
export const getInitials = (name: string): string => {
    return name
        .split(' ')
        .map(word => word.charAt(0))
        .join('')
        .toUpperCase()
        .slice(0, 2)
}

export const getAvatarColor = (name: string): string => {
    const colors = [
        'bg-red-500',
        'bg-orange-500',
        'bg-amber-500',
        'bg-yellow-500',
        'bg-lime-500',
        'bg-green-500',
        'bg-emerald-500',
        'bg-teal-500',
        'bg-cyan-500',
        'bg-sky-500',
        'bg-blue-500',
        'bg-indigo-500',
        'bg-violet-500',
        'bg-purple-500',
        'bg-fuchsia-500',
        'bg-pink-500',
        'bg-rose-500',
    ]

    const index = name.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0)
    return colors[index % colors.length]
}
