import type { JSX } from "react"
import BookmarkIcon from "@/assets/images/icon-bookmark.svg"

export function Logo(): JSX.Element {
    return (
        <div className="flex flex-row gap-x-2 items-center">
            <img src={BookmarkIcon} alt="Bookmark Icon" className="w-8 h-8"/>
            <p className="text-logo">Bookmark Manager</p>
        </div>
    )
}