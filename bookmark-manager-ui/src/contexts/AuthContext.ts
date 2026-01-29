import type { Nullable } from "@/types"
import { createContext } from "react"
import type { CurrentUser } from "@/api/users/schema"

export type AuthContextType = {
    authUser: Nullable<CurrentUser>,
    login: (email: string, password: string) => void,
    logout: () => void,
    register: (fullname: string,  email: string, password: string) => void
    me: () => void
}

export const AuthContext = createContext<Nullable<AuthContextType>>(null)