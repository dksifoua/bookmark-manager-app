import type { Nullable } from "@/types"
import { createContext } from "react"

export type Theme = "dark" | "light"

export type ThemeContextType = {
    theme: Nullable<Theme>
    setTheme: (theme: Nullable<Theme>) => void
}

export const ThemeContext = createContext<Nullable<ThemeContextType>>(null)