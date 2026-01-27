import type { JSX } from "react"
import { useAuthContext } from "@/hooks/auth.hook"
import { Navigate, Outlet } from "react-router"

export function ProtectedRoute(): JSX.Element {
    const { authUser } = useAuthContext()

    if (authUser === null) {
        return <Navigate to="login" replace/>
    }

    return <Outlet/>
}