import { useContext } from "react"
import { ThemeContext, type ThemeContextType } from "@/contexts/ThemeContext"

export function useThemeContext(): ThemeContextType {
    const context = useContext(ThemeContext)
    if (context === null) {
        throw new Error("ThemeContext is not available")
    }

    return context
}