import { AuthContext, type AuthenticatedUser } from "@/contexts/AuthContext"
import { type ReactNode, useState } from "react"
import type { Nullable } from "@/types"
import { useNavigate } from "react-router"
import { authLogin, authLogout, authRegister } from "@/api/auth"
import type { AuthApiResponse } from "@/api/auth/schema"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import type { ErrorApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"

export function AuthProvider({ children }: { children: ReactNode }): ReactNode {
    const { value, setLocalStorageValue } = useLocalStorage<AuthenticatedUser>("AuthenticatedUser")
    const [authenticatedUser, setAuthenticatedUser] = useState<Nullable<AuthenticatedUser>>(value)
    const [error, setError] = useState<{
        login: Nullable<ErrorApiResponse>
        register: Nullable<ErrorApiResponse>
    }>({ login: null, register: null })
    const [isLoading, setIsLoading] = useState<boolean>(false)

    const navigate = useNavigate()

    function login(email: string, password: string): void {
        setIsLoading(true)
        authLogin({ email, password })
            .then((response: AuthApiResponse) => {
                setAuthenticatedUser(response)
                setLocalStorageValue(response)
                setError({ ...error, login: null })

                navigate("/", { replace: true })
            })
            .catch((e) => {
                console.log("Login failed:", error)
                if (e instanceof ApiError) {
                    setError({ ...error, login: e.response })
                }
            })
            .finally((): void => setIsLoading(false))
    }

    function logout(): void {
        setIsLoading(true)
        authLogout()
            .catch((): void => {
                console.log("Logout failed:", error)
            })
            .finally((): void => {
                setAuthenticatedUser(null)
                setLocalStorageValue(null)
                setIsLoading(false)
                setError({ login: null, register: null })

                navigate("login", { replace: true })
            })
    }

    function register(fullname: string, email: string, password: string): void {
        setIsLoading(true)
        authRegister({ fullname, email, password })
            .then((response: AuthApiResponse): void => {
                login(response.email, password)
            })
            .catch((e) => {
                console.log("Registration failed:", e)
                if (e instanceof ApiError) {
                    setError({ ...error, register: e.response })
                }
            })
            .finally((): void => setIsLoading(false))
    }

    return (
        <AuthContext.Provider value={{ authenticatedUser, login, logout, register, error, isLoading }}>
            {children}
        </AuthContext.Provider>
    )
}