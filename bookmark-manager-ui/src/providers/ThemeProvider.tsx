import { type ReactNode, useEffect } from "react"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import { type Theme, ThemeContext } from "@/contexts/ThemeContext"

export function ThemeProvider({ children }: { children: ReactNode }): ReactNode {
    const { value, setLocalStorageValue } = useLocalStorage<Theme>("theme")

    useEffect((): void => {
        const theme: Theme = value ?? (window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light")
        if (theme === "dark") {
            document.documentElement.classList.add("dark")
        } else {
            document.documentElement.classList.remove("dark")
        }

        if (value) {
            setLocalStorageValue(theme)
        }
    }, [value])

    return (
        <ThemeContext.Provider value={{ theme: value, setTheme: setLocalStorageValue }}>
            {children}
        </ThemeContext.Provider>
    )
}