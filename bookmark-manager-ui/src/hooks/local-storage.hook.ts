import type { Nullable } from "@/types"
import { useState } from "react"

export function useLocalStorage<T>(key: string, initialValue: Nullable<T> = null): {
    value: Nullable<T>,
    setLocalStorageValue: (newValue: Nullable<T>) => void,
    getLocalStorageValue: () => Nullable<T>
} {
    const [value, setValue] = useState<Nullable<T>>(initialValue)

    function setLocalStorageValue(newValue: Nullable<T>) {
        setValue(newValue)
        localStorage.setItem(key, JSON.stringify(newValue))
    }

    function getLocalStorageValue(): Nullable<T> {
        const storedValue = localStorage.getItem(key)
        if (storedValue === null) return initialValue
        return JSON.parse(storedValue) as Nullable<T>
    }

    return { value, setLocalStorageValue, getLocalStorageValue }
}