import type { Nullable } from "@/types"
import { createContext } from "react"

export type AuthContextUserData = { email: string, token: string, expiresAt: Date }

export type AuthContextType = {
    isAuthenticated: boolean,
    userData: Nullable<AuthContextUserData>,
    login: (email: string, password: string) => void,
    logout: () => void,
}

export const AuthContext = createContext<Nullable<AuthContextType>>(null)