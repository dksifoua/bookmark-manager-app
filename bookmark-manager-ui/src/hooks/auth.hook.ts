import { useContext } from "react"
import { AuthContext, type AuthContextType } from "@/contexts/AuthContext"

export function useAuthContext(): AuthContextType {
    const context = useContext(AuthContext)
    if (context === null) {
        throw new Error("AuthContext is not available")
    }

    return context
}