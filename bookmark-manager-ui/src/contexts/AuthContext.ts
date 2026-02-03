import type { Nullable } from "@/types"
import { createContext } from "react"
import type { ErrorApiResponse } from "@/api/errors/schema"

export type AuthenticatedUser = { fullname: string, email: string }

export type AuthContextType = {
    authenticatedUser: Nullable<AuthenticatedUser>
    login: (email: string, password: string) => void
    logout: () => void
    register: (fullname: string, email: string, password: string) => void
    error: {
        login: Nullable<ErrorApiResponse>
        register: Nullable<ErrorApiResponse> 
    }
    isLoading: boolean
}

export const AuthContext = createContext<Nullable<AuthContextType>>(null)