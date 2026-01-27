import type { Nullable } from "@/types"
import { createContext } from "react"

export type AuthContextUser = { fullname: string, email: string, token: string, expiresAt: Date }

export type AuthContextType = {
    authUser: Nullable<AuthContextUser>,
    login: (email: string, password: string) => void,
    logout: () => void,
}

export const AuthContext = createContext<Nullable<AuthContextType>>(null)