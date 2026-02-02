import { AuthContext, type AuthenticatedUser } from "@/contexts/AuthContext"
import { type ReactNode, useState } from "react"
import type { Nullable } from "@/types"
import { useNavigate } from "react-router"
import { authLogin, authLogout } from "@/api/auth"
import type { AuthApiResponse } from "@/api/auth/schema"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import type { UnauthorizedApiResponse } from "@/api/errors/schema"

export function AuthProvider({ children }: { children: ReactNode }): ReactNode {
    const { value, setLocalStorageValue } = useLocalStorage<AuthenticatedUser>("AuthenticatedUser")
    const [authenticatedUser, setAuthenticatedUser] = useState<Nullable<AuthenticatedUser>>(value)
    const [error, setError] = useState<Nullable<UnauthorizedApiResponse>>(null)

    const navigate = useNavigate()

    function login(email: string, password: string): void {
        authLogin({ email, password })
            .then((response: AuthApiResponse) => {
                setAuthenticatedUser(response)
                setLocalStorageValue(response)

                navigate("/", { replace: true })
            })
            .catch((error) => {
                console.log("Login failed:", error)
                if (error instanceof UnauthorizedApiError) {
                    setError(error.response)
                }
            })
    }

    function logout(): void {
        authLogout()
            .catch((error) => {
                console.log("Logout failed:", error)
            })
            .finally(() => {
                setAuthenticatedUser(null)
                setLocalStorageValue(null)

                navigate("login", { replace: true })
            })
    }

    return (
        <AuthContext.Provider value={{ authenticatedUser, login, logout, error }}>
            {children}
        </AuthContext.Provider>
    )
}