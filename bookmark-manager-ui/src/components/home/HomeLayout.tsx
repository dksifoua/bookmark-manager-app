import { type JSX } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { Sidebar } from "@/components/home/Sidebar"
import { Modal } from "@/components/home/Modal"

export function HomeLayout(): JSX.Element {
    
    return (
        <div className="min-h-screen flex flex-row p-0">
            <div className="max-xl:hidden">
                <Sidebar/>
            </div>
            <div className={`w-full flex flex-col gap-y-2.5 px-0 pt-16.25 md:pt-19.5 relative`}>
                <Header/>
                <Outlet/>
            </div>
            <Modal/>
        </div>
    )
}