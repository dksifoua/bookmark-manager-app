import { type JSX, useRef, useState } from "react"
import { useQuery } from "@tanstack/react-query"
import { fetchBookmarks } from "@/api/bookmarks"
import type { BookmarkResponse } from "@/api/bookmarks/schema"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"
import { BookmarkCard } from "@/components/bookmark/BookmarkCard"
import SortIcon from "@/assets/images/icon-sort.svg"
import CheckIcon from "@/assets/images/icon-check.svg"
import { useCloseModal } from "@/hooks/modal.hook"

export function BookmarkList(): JSX.Element {
    const { headerTitle, areBookmarksArchived, tagFilters, bookmarkSortBy } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            headerTitle: store.headerTitle,
            areBookmarksArchived: store.filterArchivedBookmarks,
            tagFilters: store.tagFilters,
            bookmarkSortBy: store.bookmarkSortBy,
        }))
    )
    const { data: bookmarks } = useQuery({
        queryKey: ["bookmarks"],
        queryFn: fetchBookmarks,
        select: (data: BookmarkResponse[]): BookmarkResponse[] =>
            [
                ...data
                    .filter((bookmark: BookmarkResponse): boolean => tagFilters.every((filter: string): boolean => bookmark.tags.includes(filter)))
                    .filter((bookmark: BookmarkResponse): boolean => areBookmarksArchived ? bookmark.isArchived : true)
            ].sort((a: BookmarkResponse, b: BookmarkResponse): number => {
                if (bookmarkSortBy === "recently-added") {
                    return b.creationTime.getTime() - a.creationTime.getTime()
                } else if (bookmarkSortBy === "recently-visited") {
                    return (b.lastVisitTime?.getTime() ?? 0) - (a.lastVisitTime?.getTime() ?? 0)
                } else {
                    return b.visitCount - a.visitCount
                }
            })
    })
    const ref = useRef<HTMLDivElement>(null)
    const [isDropdownOpen, setIsDropdownOpen] = useState<boolean>(false)

    useCloseModal(ref, (): void => setIsDropdownOpen(false))

    return (
        <div className="flex flex-col gap-y-5 px-8 pt-8 pb-16">
            <div className="flex flex-row gap-x-4 items-center justify-between relative" ref={ref}>
                <p className="text-preset-1 md:text-preset-2 text-neutral-900">{headerTitle}</p>
                <button
                    onClick={(): void => setIsDropdownOpen(!isDropdownOpen)}
                    className="w-29 h-10.5 flex flex-row gap-x-1 px-3 py-2.5 items-center justify-between bg-neutral-0 rounded-8 cursor-pointer">
                    <img src={SortIcon} alt="Sort Icon" className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-900 w-20">Sort by</p>
                </button>
                <div className="absolute right-0 top-[125%]">
                    {isDropdownOpen && <SortByDropdown/>}
                </div>
            </div>
            <div className="flex flex-wrap gap-8 pt-8 items-center justify-center">
                {
                    bookmarks?.map((bookmark: BookmarkResponse) => (
                        <BookmarkCard key={bookmark.bookmarkId} bookmark={bookmark}/>
                    ))
                }
            </div>
        </div>
    )
}

function SortByDropdown(): JSX.Element {
    const { bookmarkSortBy, setBookmarkSortBy } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            bookmarkSortBy: store.bookmarkSortBy,
            setBookmarkSortBy: store.setBookmarkSortBy,
        }))
    )

    return (
        <div
            className="w-50 flex flex-col gap-y-1 p-2 rounded-8 bg-neutral-0 norder border-neutral-100 cursor-pointer">
            <button
                onClick={(): void => {
                    bookmarkSortBy !== "recently-added" && setBookmarkSortBy("recently-added")
                }}
                className={`h-9 flex flex-row gap-x-2.5 p-2 items-center justify-between ${
                    bookmarkSortBy === "recently-added"
                        ? ""
                        : "cursor-pointer rounded-8 hover:bg-neutral-300"
                }`}
            >
                <p className="text-preset-4 text-neutral-800">Recently added</p>
                {bookmarkSortBy === "recently-added" && <img src={CheckIcon} alt="Check Icon" className="w-4 h-4"/>}
            </button>
            {/*<button className="h-9 flex flex-row gap-x-2.5 p-2 items-center justify-between">*/}
            {/*    <p className="text-preset-4 text-neutral-800">Recently visited</p>*/}
            {/*    <img src={CheckIcon} alt="Check Icon" className="w-4 h-4"/>*/}
            {/*</button>*/}
            <button
                onClick={(): void => {
                    bookmarkSortBy !== "most-visited" && setBookmarkSortBy("most-visited")
                }}
                className={`h-9 flex flex-row gap-x-2.5 p-2 items-center justify-between ${
                    bookmarkSortBy === "most-visited"
                        ? ""
                        : "cursor-pointer rounded-8 hover:bg-neutral-300"
                }`}
            >
                <p className="text-preset-4 text-neutral-800">Most visited</p>
                {bookmarkSortBy === "most-visited" && <img src={CheckIcon} alt="Check Icon" className="w-4 h-4"/>}
            </button>
        </div>
    )
}