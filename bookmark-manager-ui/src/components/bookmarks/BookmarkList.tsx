import { type JSX, useEffect, useRef, useState } from "react"
import { useCloseModal } from "@/hooks/modal.hook"
import { SortIcon, CheckIcon, LoadingIcon } from "@/components/icons"
import { BookmarkCard } from "@/components/bookmarks/BookmarkCard"
import { useQuery } from "@tanstack/react-query"
import { fetchBookmarks } from "@/api/bookmarks"
import type { Bookmark } from "@/api/bookmarks/schema"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import { useAuthContext } from "@/hooks/auth.hook"

export function BookmarkList(): JSX.Element {
    const ref = useRef<HTMLDivElement>(null)
    const [isDropdownOpen, setIsDropdownOpen] = useState<boolean>(false)

    const { headerTitle, searchQuery, sortBookmarksBy, tagFilters, filterArchivedBookmarks } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            headerTitle: store.headerTitle,
            searchQuery: store.searchQuery,
            sortBookmarksBy: store.sortBookmarksBy,
            tagFilters: store.tagFilters,
            filterArchivedBookmarks: store.filterArchivedBookmarks,
        }))
    )

    const { data: bookmarks, isFetching, isError, error } = useQuery({
        queryKey: ["bookmarks", searchQuery],
        queryFn: async (): Promise<Bookmark[]> => fetchBookmarks(searchQuery),
        select: (data: Bookmark[]): Bookmark[] =>
            [
                ...data
                    .filter((bookmark: Bookmark): boolean => filterArchivedBookmarks ? bookmark.isArchived : !bookmark.isArchived)
                    .filter((bookmark: Bookmark): boolean => tagFilters.every((filter: string): boolean => bookmark.tags.includes(filter)))
            ].sort((a: Bookmark, b: Bookmark): number => {
                const primarySort = Number(b.isPinned) - Number(a.isPinned)
                if (primarySort !== 0) return primarySort

                if (sortBookmarksBy === "recently-added") {
                    return b.creationTime.getTime() - a.creationTime.getTime()
                } else if (sortBookmarksBy === "most-visited") {
                    return b.visitsCount - a.visitsCount
                } else if (sortBookmarksBy === "last-visited") {
                    return (b.lastVisitTime?.getTime() ?? 0) - (a.lastVisitTime?.getTime() ?? 0)
                }
                return 0
            })
    })

    useCloseModal(ref, (): void => setIsDropdownOpen(false))
    
    const { logout } = useAuthContext()
    useEffect(() => {
        if (isError && error instanceof UnauthorizedApiError) {
            logout()
        }
    }, [isError, error, logout])
    

    return (
        <div className="flex flex-col gap-y-5 px-8 pt-8 pb-16">
            <div className="flex flex-row gap-x-4 items-center justify-between relative" ref={ref}>
                <p className="text-preset-1 md:text-preset-2 text-neutral-900">{headerTitle}</p>
                <button
                    onClick={(): void => setIsDropdownOpen(!isDropdownOpen)}
                    className="w-29 h-10.5 flex flex-row gap-x-1 px-3 py-2.5 items-center justify-between bg-neutral-0 rounded-8 cursor-pointer">
                    <SortIcon className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-900 w-20">Sort by</p>
                </button>
                <div className="absolute right-0 top-[125%] z-10">
                    {isDropdownOpen && <SortByDropdown closeDropdown={(): void => setIsDropdownOpen(false)}/>}
                </div>
            </div>
            <div className="flex flex-wrap gap-8 pt-8 items-center justify-center">
                {
                    isFetching
                        ? <LoadingIcon className="w-12 h-12 mx-auto"/>
                        : !bookmarks || bookmarks.length === 0
                            ? <p>No bookmarks to display.</p>
                            : bookmarks?.map((bookmark: Bookmark) => (
                                <BookmarkCard key={bookmark.bookmarkId} bookmark={bookmark}/>
                            ))
                }
            </div>
        </div>
    )
}

function SortByDropdown({ closeDropdown }: { closeDropdown: () => void }): JSX.Element {
    const { sortBookmarksBy, setSortBookmarksBy } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            sortBookmarksBy: store.sortBookmarksBy,
            setSortBookmarksBy: store.setSortBookmarksBy
        }))
    )

    return (
        <div
            className="w-50 flex flex-col gap-y-1 p-2 rounded-8 bg-neutral-0 norder border-neutral-100 cursor-pointer">
            <button
                onClick={(): void => {
                    if (sortBookmarksBy !== "recently-added") {
                        setSortBookmarksBy("recently-added")
                        closeDropdown()
                    }
                }}
                className={`h-9 flex flex-row gap-x-2.5 p-2 items-center justify-between ${
                    sortBookmarksBy === "recently-added"
                        ? ""
                        : "cursor-pointer rounded-8 hover:bg-neutral-300"
                }`}
            >
                <p className="text-preset-4 text-neutral-800">Recently added</p>
                {sortBookmarksBy === "recently-added" && <CheckIcon className="w-4 h-4"/>}
            </button>
            <button
                onClick={(): void => {
                    if (sortBookmarksBy !== "last-visited") {
                        setSortBookmarksBy("last-visited")
                        closeDropdown()
                    }
                }}
                className={`h-9 flex flex-row gap-x-2.5 p-2 items-center justify-between ${
                    sortBookmarksBy === "last-visited"
                        ? ""
                        : "cursor-pointer rounded-8 hover:bg-neutral-300"
                }`}
            >
                <p className="text-preset-4 text-neutral-800">Last visited</p>
                {sortBookmarksBy === "last-visited" && <CheckIcon className="w-4 h-4"/>}
            </button>
            <button
                onClick={(): void => {
                    if (sortBookmarksBy !== "most-visited") {
                        setSortBookmarksBy("most-visited")
                        closeDropdown()
                    }
                }}
                className={`h-9 flex flex-row gap-x-2.5 p-2 items-center justify-between ${
                    sortBookmarksBy === "most-visited"
                        ? ""
                        : "cursor-pointer rounded-8 hover:bg-neutral-300"
                }`}
            >
                <p className="text-preset-4 text-neutral-800">Most visited</p>
                {sortBookmarksBy === "most-visited" && <CheckIcon className="w-4 h-4"/>}
            </button>
        </div>
    )
}