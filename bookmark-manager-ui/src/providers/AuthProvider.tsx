import { AuthContext } from "@/contexts/AuthContext"
import { type ReactNode, useState } from "react"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import type { Nullable } from "@/types"
import { useNavigate } from "react-router"
import { authLogin, authLogout, authRegister } from "@/api/auth"
import {
    type LoginResponse,
    type RegistrationResponse,
    SuccessfulLoginApiResponseSchema,
    SuccessfulRegistrationApiResponseSchema
} from "@/api/auth/schema"
import { fetchCurrentUser } from "@/api/users"
import type { CurrentUser } from "@/api/users/schema"

export function AuthProvider({ children }: { children: ReactNode }): ReactNode {
    const { value, setLocalStorageValue } = useLocalStorage<CurrentUser>("AuthUser")
    const [user, setUser] = useState<Nullable<CurrentUser>>(value)

    const navigate = useNavigate()

    function login(email: string, password: string): void {
        authLogin({ email, password })
            .then((response: LoginResponse) => {
                const parsedSuccessful = SuccessfulLoginApiResponseSchema.safeParse(response)
                if (parsedSuccessful.success) {
                    setUser(parsedSuccessful.data)
                    setLocalStorageValue(parsedSuccessful.data)

                    navigate("/", { replace: true })
                }
            })
            .catch((error) => {
                console.log("Login failed:", error)
            })
    }

    function logout(): void {
        authLogout()
            .catch((error) => {
                console.log("Logout failed:", error)
            })
            .finally(() => {
                setUser(null)
                setLocalStorageValue(null)

                navigate("login", { replace: true })
            })
    }

    function register(fullname: string, email: string, password: string): void {
        authRegister({ fullname, email, password })
            .then((response: RegistrationResponse) => {
                const parsedSuccessful = SuccessfulRegistrationApiResponseSchema.safeParse(response)
                if (parsedSuccessful.success) {
                    login(email, password)
                }
            })
            .catch((error) => {
                console.log("Registration failed:", error)
            })
    }
    
    function me(): void {
        fetchCurrentUser()
            .then((response: CurrentUser) => {
                setUser(response)
                setLocalStorageValue(response)
            })
            .catch(() => {                
                setUser(null)
                setLocalStorageValue(null)

                navigate("login", { replace: true })
            })
    }

    return (
        <AuthContext.Provider value={{ authUser: user, login, logout, register, me }}>
            {children}
        </AuthContext.Provider>
    )
}