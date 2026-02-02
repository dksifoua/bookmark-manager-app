import type { Nullable } from "@/types"
import { createContext } from "react"
import type { UnauthorizedApiResponse } from "@/api/errors/schema"

export type AuthenticatedUser = { fullname: string, email: string }

export type AuthContextType = {
    authenticatedUser: Nullable<AuthenticatedUser>,
    login: (email: string, password: string) => void,
    logout: () => void,
    error: Nullable<UnauthorizedApiResponse>
}

export const AuthContext = createContext<Nullable<AuthContextType>>(null)