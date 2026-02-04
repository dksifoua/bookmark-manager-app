import { type JSX, useRef, useState } from "react"
import { useCloseModal } from "@/hooks/modal.hook"
import MenuHamburgerIcon from "@/assets/images/icon-menu-hamburger.svg"
import SearchIcon from "@/assets/images/icon-search.svg"
import AddIcon from "@/assets/images/icon-add.svg"
import ThemeIcon from "@/assets/images/icon-theme.svg"
import LightThemeIcon from "@/assets/images/icon-light-theme.svg"
import DarkThemeIcon from "@/assets/images/icon-dark-theme.svg"
import LogoutIcon from "@/assets/images/icon-logout.svg"
import AvatarImage from "@/assets/images/image-avatar.webp"
import { useAuthContext } from "@/hooks/auth.hook"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"

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
    const { setIsMobileSidebarOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsMobileSidebarOpen: store.setIsMobileSidebarOpen,
        }))
    )

    return (
        <div className="flex flex-row gap-x-2.5 md:gap-x-4 justify-start">
            <button onClick={() => setIsMobileSidebarOpen(true)}
                    className="xl:hidden w-10 md-w-11 h-10 md:h-11 p-2.5 border border-neutral-400 rounded-8 cursor-pointer">
                <img src={MenuHamburgerIcon} alt="Menu Hamburger Icon" className="w-5 h-5"/>
            </button>
            <div className="relative w-48.25 md:w-[320px]">
                <input type="search" name="search" placeholder="Search by title..."
                       className="w-full h-10 md:h-11 pl-10 border border-neutral-400 rounded-8"/>
                <img src={SearchIcon} alt="Search Icon" className="w-5 h-5 absolute top-1/2 -translate-y-1/2 left-2.5"/>
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
                <img src={AddIcon} alt="Add Icon" className="w-5 h-5 brightness-0 invert"/>
                <p className="max-md:hidden text-preset-3 text-neutral-0">Add Bookmark</p>
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
                        <img src={ThemeIcon} alt="Theme Icon" className="w-4 h-4"/>
                        <p className="text-preset-4 text-neutral-800">Theme</p>
                    </div>
                    <div className="flex flex-row p-0.5 rounded-4 bg-neutral-300 border border-neutral-300 items-center">
                        <div className="flex px-2 py-1.5 rounded-4 bg-neutral-0 items-center">
                            <img src={LightThemeIcon} alt="Light Theme Icon" className="w-4 h-4"/>
                        </div>
                        <div className="flex px-2 py-1.5 rounded-4 bg-neutral-300 items-center">
                            <img src={DarkThemeIcon} alt="Dark Theme Icon" className="w-4 h-4"/>
                        </div>
                    </div>
                </div>
            </div>
            <div className="flex px-2 py-1 border border-neutral-100 items-center">
                <button
                    onClick={logout}
                    className="w-full flex flex-row gap-x-2.5 p-2 items-center cursor-pointer"
                >
                    <img src={LogoutIcon} alt="Logout Icon" className="w-4 h-4"/>
                    <p className="text-preset-4 text-neutral-800">Logout</p>
                </button>
            </div>
        </div>
    )
}