import type { JSX } from "react"
import { useAuth } from "@/hooks/auth.hook"
import { Navigate, Outlet } from "react-router"

export function ProtectedRoute(): JSX.Element {
    const { isAuthenticated } = useAuth()
    
    if (!isAuthenticated) {
        return <Navigate to="login" replace />
    }
    
    return <Outlet />
}