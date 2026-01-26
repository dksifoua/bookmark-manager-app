import { AuthContext } from "@/contexts/AuthContext"
import type { ReactNode } from "react"
import { useAuth } from "@/hooks/auth.hook"

export function AuthProvider({ children }: { children: ReactNode }): ReactNode {
    const context = useAuth()

    return (
        <AuthContext.Provider value={context}>
            {children}
        </AuthContext.Provider>
    )
}