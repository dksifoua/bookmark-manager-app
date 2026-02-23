import { type ChangeEvent, type JSX, type KeyboardEvent, useRef, useState } from "react"
import { useCloseModal } from "@/hooks/modal.hook"
import {
    AddIcon,
    DarkThemeIcon,
    LightThemeIcon,
    LogoutIcon,
    MenuHamburgerIcon,
    SearchIcon,
    ThemeIcon,
} from "@/components/icons"
import AvatarImage from "@/assets/images/image-avatar.webp"
import { useAuthContext } from "@/hooks/auth.hook"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"
import { useThemeContext } from "@/hooks/theme.hook"
import { useQuery } from "@tanstack/react-query"
import type { Bookmark } from "@/api/bookmarks/schema"
import { fetchBookmarks } from "@/api/bookmarks"

export function Header(): JSX.Element {

    return (
        <div
            className={`w-full h-16.25 md:h-19.5 absolute top-0 flex flex-row gap-y-2.5 px-4 py-3 bg-neutral-0 items-center justify-between`}>
            <SearchBar/>
            <ButtonGroup/>
        </div>
    )
}

function SearchBar(): JSX.Element {
    const [searchTerm, setSearchTerm] = useState<string>("")
    const [enableSearch, setEnableSearch] = useState<boolean>(false)
    const { searchQuery, setSearchQuery, setIsMobileSidebarOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            searchQuery: store.searchQuery,
            setSearchQuery: store.setSearchQuery,
            setIsMobileSidebarOpen: store.setIsMobileSidebarOpen,
        }))
    )
    useQuery({
        queryKey: ["bookmarks", searchQuery],
        queryFn: async (): Promise<Bookmark[]> => fetchBookmarks(searchQuery),
        enabled: enableSearch
    })

    function handleSearch(event: KeyboardEvent<HTMLInputElement>): void {
        if (event.key === "Enter") {
            event.preventDefault()

            setSearchQuery(searchTerm.toLowerCase())
            setEnableSearch(true)
        }
    }
    
    function handleChange(event: ChangeEvent<HTMLInputElement>): void {        
        const prevSearchTerm = searchTerm
        setSearchTerm(event.target.value)
        
        if (prevSearchTerm.length > 0 && event.target.value.length === 0) {
            setSearchQuery("")
            setEnableSearch(true)
        } else {
            setEnableSearch(false)
        }
    }

    return (
        <div className="flex flex-row gap-x-2.5 md:gap-x-4 justify-start">
            <button onClick={() => setIsMobileSidebarOpen(true)}
                    className="xl:hidden w-10 md-w-11 h-10 md:h-11 p-2.5 border border-neutral-400 rounded-8 cursor-pointer">
                <MenuHamburgerIcon className="w-5 h-5"/>
            </button>
            <div className="relative w-48.25 md:w-[320px]">
                <input type="search" name="search" placeholder="Search by title..."
                       value={searchTerm} onChange={handleChange} onKeyDown={handleSearch}
                       className="w-full h-10 md:h-11 pl-10 pr-2 border border-neutral-300 rounded-8 placeholder:text-neutral-800"/>
                <SearchIcon className="w-5 h-5 absolute top-1/2 -translate-y-1/2 left-2.5"/>
            </div>
        </div>
    )
}

function ButtonGroup(): JSX.Element {
    const ref = useRef<HTMLDivElement>(null)
    const [isDropdownOpen, setIsDropdownOpen] = useState<boolean>(false)
    const { setIsAddDialogOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsAddDialogOpen: store.setIsAddDialogOpen,
        }))
    )

    useCloseModal(ref, (): void => setIsDropdownOpen(false))

    return (
        <div className="flex flex-row gap-x-2.5 md:gap-x-4 items-center justify-between relative" ref={ref}>
            <button onClick={(): void => setIsAddDialogOpen(true, null)}
                    className="w-10 md:w-auto h-10 md:h-11 flex md:flex-row md:gap-x-1 items-center justify-center md:justify-between md:px-4 rounded-8 bg-teal-700 text-neutral-0 cursor-pointer"
            >
                <AddIcon className="w-5 h-5"/>
                <p className="max-md:hidden text-preset-3 text-neutral-0 dark:text-neutral-d-0">Add Bookmark</p>
            </button>
            <button
                onClick={(): void => setIsDropdownOpen(!isDropdownOpen)}
                className="w-10 md:w-11 h-10 md:h-11 flex items-center justify-center rounded-8 cursor-pointer"
            >
                <img src={AvatarImage} alt="Avatar Image" className="w-10 md:w-11"/>
            </button>
            {
                isDropdownOpen &&
                <div className="absolute right-0 top-14 z-10">
                    <AvatarDropdown/>
                </div>
            }
        </div>
    )
}

function AvatarDropdown(): JSX.Element {
    const { theme, setTheme } = useThemeContext()
    const { logout } = useAuthContext()

    return (
        <div className="w-62 flex flex-col gap-y-1 p-0 rounded-8 bg-neutral-0 border border-neutral-100">
            <div className="flex px-4 py-3 border border-neutral-100 items-center justify-center">
                <div className="flex flex-row gap-x-3">
                    <img src={AvatarImage} alt="Avatar Image" className="w-10 md:w-11"/>
                    <div className="flex flex-col gap-y-1">
                        <p className="text-preset-4 text-neutral-900">{`Dimitri Sifoua`}</p>
                        <p className="text-preset-4-md text-neutral-800">{`dimitri.sifoua@gmail.com`}</p>
                    </div>
                </div>
            </div>
            <div className="flex px-2 py-1 items-center">
                <div className="w-full h-11.5 flex flex-row gap-x-2.5 p-2 items-center justify-between">
                    <div className="flex flex-row gap-x-2.5 items-center">
                        <ThemeIcon className="w-4 h-4"/>
                        <p className="text-preset-4 text-neutral-800">Theme</p>
                    </div>
                    <button
                        onClick={(): void => setTheme(theme === "light" ? "dark" : "light")}
                        className="flex flex-row p-0.5 rounded-4 bg-neutral-300 border border-neutral-300 items-center cursor-pointer"
                    >
                        <div className={`flex px-2 py-1.5 rounded-4 ${
                            theme === "light" ? "bg-neutral-0" : "bg-neutral-300"
                        } items-center`}>
                            <LightThemeIcon className="w-4 h-4"/>
                        </div>
                        <div className={`flex px-2 py-1.5 rounded-4 ${
                            theme === "dark" ? "bg-neutral-0" : "bg-neutral-300"
                        } items-center`}>
                            <DarkThemeIcon className="w-4 h-4"/>
                        </div>
                    </button>
                </div>
            </div>
            <div className="flex px-2 py-1 border border-neutral-100 items-center">
                <button
                    onClick={logout}
                    className="w-full flex flex-row gap-x-2.5 p-2 items-center rounded-8 hover:bg-neutral-300 cursor-pointer"
                >
                    <LogoutIcon className="w-4 h-4"/>
                    <p className="text-preset-4 text-neutral-800">Logout</p>
                </button>
            </div>
        </div>
    )
}