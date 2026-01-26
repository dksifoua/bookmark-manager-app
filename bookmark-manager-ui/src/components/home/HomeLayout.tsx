import type { JSX } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { Sidebar } from "@/components/home/Sidebar"

export function HomeLayout(): JSX.Element {
    return (
        <div className="flex flex-row p-0">
            <div className="max-xl:hidden">
                <Sidebar/>
            </div>
            <div className="w-full relative flex flex-col gap-y-2.5 px-0 pt-16.25 md:pt-19.5">
                <Header/>
                <Outlet/>
            </div>
        </div>
    )
}