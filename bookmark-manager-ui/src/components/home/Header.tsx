import type { JSX } from "react"
import MenuHamburgerIcon from "@/assets/images/icon-menu-hamburger.svg"
import SearchIcon from "@/assets/images/icon-search.svg"
import AddIcon from "@/assets/images/icon-add.svg"
import AvatarImage from "@/assets/images/image-avatar.webp"

export function Header(): JSX.Element {
    
    return (
        <div className="w-full h-16.25 md:h-19.5 absolute top-0 flex flex-row gap-y-2.5 px-4 py-3 bg-neutral-0 items-center justify-between">
            <SearchBar/>
            <ButtonGroup/>
        </div>
    )
}

function SearchBar(): JSX.Element {
    
    return (
        <div className="flex flex-row gap-x-2.5 md:gap-x-4 justify-start">
            <button className="xl:hidden w-10 md-w-11 h-10 md:h-11 p-2.5 border border-neutral-400 rounded-8">
                <img src={MenuHamburgerIcon} alt="Menu Hamburger Icon" className="w-5 h-5"/>
            </button>
            <div className="relative w-48.25 md:w-[320px]">
                <input type="search" name="search" placeholder="Search by title..." className="w-full h-10 md:h-11 pl-10 border border-neutral-400 rounded-8"/>
                <img src={SearchIcon} alt="Search Icon" className="w-5 h-5 absolute top-1/2 -translate-y-1/2 left-2.5"/>
            </div>
        </div>
    )
}

function ButtonGroup(): JSX.Element {
    
    return (
        <div className="flex flex-row gap-x-2.5 md:gap-x-4 items-center justify-between">
            <button className="w-10 md:w-auto h-10 md:h-11 flex md:flex-row md:gap-x-1 items-center justify-center md:justify-between md:px-4 rounded-8 bg-teal-700 text-neutral-0">
                <img src={AddIcon} alt="Add Icon" className="w-5 h-5 brightness-0 invert"/>
                <p className="max-md:hidden text-preset-3 text-neutral-0">Add Bookmark</p>
            </button>
            <button className="w-10 md:w-11 h-10 md:h-11 flex items-center justify-center rounded-8">
                <img src={AvatarImage} alt="Avatar Image" className="w-10 md:w-11"/>
            </button>
        </div>
    )
}