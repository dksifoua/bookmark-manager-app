import type { JSX } from "react"
import { Outlet } from "react-router"

export function AuthLayout(): JSX.Element {

    return (
        <div className="min-h-screen flex px-4 md:p-0 items-center justify-center">
            <Outlet/>
        </div>
    )
}