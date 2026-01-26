import { type JSX, useRef, useState } from "react"
import { Outlet } from "react-router"
import { Header } from "@/components/home/Header"
import { Sidebar } from "@/components/home/Sidebar"
import { useCloseModal } from "@/hooks/modal.hook"

export function HomeLayout(): JSX.Element {
    const ref = useRef<HTMLDivElement>(null)
    const [isMobileSidebarOpen, setMobileSidebarOpen] = useState<boolean>(false)

    useCloseModal(ref, (): void => setMobileSidebarOpen(false))

    return (
        <div className="flex flex-row p-0">
            <div className="max-xl:hidden">
                <Sidebar/>
            </div>
            <div className={`w-full relative flex flex-col gap-y-2.5 px-0 pt-16.25 md:pt-19.5`} ref={ref}>
                {
                    isMobileSidebarOpen
                    && <div className="absolute top-0 left-0 z-10" ref={ref}>
                        <Sidebar openSidebar={() => setMobileSidebarOpen(false)}/>
                    </div>
                }
                <div className={isMobileSidebarOpen ? "opacity-50" : ""}>
                    <Header openSidebar={() => setMobileSidebarOpen(true)}/>
                    <Outlet/>
                </div>
            </div>
        </div>
    )
}