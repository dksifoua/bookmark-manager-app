import { useContext, useState } from "react"
import { AuthContext, type AuthContextType, type AuthContextUser } from "@/contexts/AuthContext"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import type { Nullable } from "@/types"
import { useNavigate } from "react-router"
import { authLogin, authLogout } from "@/api/auth"
import { type LoginResponse, SuccessfulLoginApiResponseSchema } from "@/api/auth/schema"

export function useAuthContext(): AuthContextType {
    const context = useContext(AuthContext)
    if (context === null) {
        throw new Error("AuthContext is not available")
    }

    return context
}

export function useAuth(): AuthContextType {
    const { value, setLocalStorageValue } = useLocalStorage<AuthContextUser>("AuthUser")
    const [user, setUser] = useState<Nullable<AuthContextUser>>(value)

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
            .then(() => {
                setUser(null)
                setLocalStorageValue(null)

                navigate("login", { replace: true })
            })
            .catch((error) => {
                console.log("Logout failed:", error)
            })
    }

    return {
        authUser: user,
        login,
        logout
    }
}