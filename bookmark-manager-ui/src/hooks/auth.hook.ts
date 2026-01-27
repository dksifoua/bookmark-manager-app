import { useContext, useState } from "react"
import { AuthContext, type AuthContextType, type AuthContextUser } from "@/contexts/AuthContext"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import type { Nullable } from "@/types"
import { useNavigate } from "react-router"

export function useAuthContext(): AuthContextType {
    const context = useContext(AuthContext)
    if (context === null) {
        throw new Error("AuthContext is not available")
    }
    
    return context
}

export function useAuth(): AuthContextType {    
    const { value, setLocalStorageValue } = useLocalStorage<AuthContextUser>("AuthUser")
    const [userData, setUserData] = useState<Nullable<AuthContextUser>>(value)
    
    const navigate = useNavigate()
    
    function login(email: string,  password: string): void {
        if (email === "dimitri.sifoua@gmail.com" && password === "Password123") {
            const userData = {
                fullname: "Dimitri Sifoua",
                email,
                token: "generated_token",
                expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000)
            }
            
            setUserData(userData)
            setLocalStorageValue(userData)
            
            navigate("/", { replace: true })
        }
    }
    
    function logout(): void {
        setUserData(null)
        setLocalStorageValue(null)

        navigate("login", { replace: true })
    }

    return {
        authUser: userData,
        login,
        logout
    }
}