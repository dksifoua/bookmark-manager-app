import { useState } from "react"
import { type AuthContextType, type AuthContextUserData } from "@/contexts/AuthContext"
import { useLocalStorage } from "@/hooks/local-storage.hook"
import type { Nullable } from "@/types"

export function useAuth(): AuthContextType {    
    // const context = useContext(AuthContext)
    // if (context === null) {
    //     throw new Error("AuthContext is not available")
    // }

    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false)
    const [userData, setUserData] = useState<Nullable<AuthContextUserData>>(null)
    
    const { setLocalStorageValue } = useLocalStorage<AuthContextUserData>("auth")
    
    function login(email: string,  password: string): void {
        if (email === "dimitri.sifoua@gmail.com" && password === "Password123") {
            const userData = {
                email,
                token: "generated_token",
                expiresAt: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000)
            }
            
            setIsAuthenticated(true)
            setUserData(userData)
            setLocalStorageValue(userData)
        }
    }
    
    function logout(): void {
        setIsAuthenticated(false)
        setUserData(null)
        setLocalStorageValue(null)
    }

    return { 
        isAuthenticated,
        userData,
        login,
        logout
    }
}