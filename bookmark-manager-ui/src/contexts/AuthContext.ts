import type { Nullable } from "@/types"
import { createContext } from "react"

export type AuthContextUser = { fullname: string, email: string }

export type AuthContextType = {
    authUser: Nullable<AuthContextUser>,
    login: (email: string, password: string) => void,
    logout: () => void,
    register: (fullname: string,  email: string, password: string) => void,
}

export const AuthContext = createContext<Nullable<AuthContextType>>(null)