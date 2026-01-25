import type { JSX } from "react"
import { Outlet } from "react-router"

export function HomeLayout(): JSX.Element {
    return (
        <div>
            <Outlet/>
        </div>
    )
}