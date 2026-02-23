import { type ChangeEvent, type JSX, useEffect, useState } from "react"
import { Logo } from "@/components/Logo"
import { ArchiveIcon, CloseIcon, HomeIcon, LoadingIcon } from "@/components/icons"
import { useQuery } from "@tanstack/react-query"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"
import { useAuthContext } from "@/hooks/auth.hook"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import type { Bookmark } from "@/api/bookmarks/schema"
import { fetchBookmarks } from "@/api/bookmarks"

function buildTag2count({ bookmarks }: { bookmarks: Bookmark[] | undefined }): Map<string, number> {
    const tag2count: Map<string, number> = new Map()
    if (!bookmarks) return tag2count

    for (const bookmark of bookmarks) {
        for (const tag of bookmark.tags) {
            tag2count.set(tag, (tag2count.get(tag) ?? 0) + 1)
        }
    }

    return tag2count
}

export function Sidebar({ openSidebar }: { openSidebar?: () => void }): JSX.Element {
    const { searchQuery, tagFilters, filterArchivedBookmarks } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            searchQuery: store.searchQuery,
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
            ]
    })
    const tag2count = buildTag2count({ bookmarks })

    const { logout } = useAuthContext()
    useEffect(() => {
        if (isError && error instanceof UnauthorizedApiError) {
            logout()
        }
    }, [isError, error, logout])

    return (
        <div className="w-74 h-full flex flex-col bg-neutral-0 border border-neutral-300">
            <div className="xl:h-19.5 flex items-center justify-center pt-5 pb-2.5 relative">
                <Logo/>
                {
                    openSidebar &&
                    <button onClick={openSidebar} className="xl:hidden absolute top-2.5 right-2.5 cursor-pointer">
                        <CloseIcon className="w-8 h-8"/>
                    </button>
                }
            </div>
            <Navigation/>
            <div className="flex flex-col px-4">
                <p className="px-3 pb-4 text-preset-5">TAGS</p>
                {
                    isFetching
                        ? <LoadingIcon className="w-12 h-12 mx-auto"/>
                        : Array.from(tag2count.entries())
                            .sort((a: [string, number], b: [string, number]): number => a[0].localeCompare(b[0]))
                            .map(([tag, count]: [string, number]): JSX.Element =>
                                <Tag key={tag} name={tag} count={count}/>)
                }
            </div>
        </div>
    )
}

function Navigation(): JSX.Element {
    const { filterArchivedBookmarks, setFilterArchivedBookmarks } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            filterArchivedBookmarks: store.filterArchivedBookmarks,
            setFilterArchivedBookmarks: store.setFilterArchivedBookmarks,
        }))
    )
    const [selected, setSelected] = useState<"home" | "archived">(() => {
        return filterArchivedBookmarks ? "archived" : "home"
    })

    return (
        <div className="flex flex-col gap-y-4 px-4 py-5">
            <div className="flex flex-col gap-y-2">
                <button className={`flex flex-row gap-x-3 items-center justify-start px-3 py-2 border ${
                    selected === "home"
                        ? "rounded-6 bg-neutral-100 border-neutral-100"
                        : "hover:rounded-6 border-neutral-0 hover:bg-neutral-100 hover:border-neutral-100 cursor-pointer"
                }`} onClick={() => {
                    if (selected === "archived") {
                        setSelected("home")
                        setFilterArchivedBookmarks(false)
                    }
                }}>
                    <HomeIcon className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-900">Home</p>
                </button>
                <button className={`flex flex-row gap-x-3 items-center justify-start px-3 py-2 border ${
                    selected === "archived"
                        ? "rounded-6 bg-neutral-100 border-neutral-100"
                        : "hover:rounded-6 border-neutral-0 hover:bg-neutral-100 hover:border-neutral-100 cursor-pointer"
                }`} onClick={() => {
                    if (selected === "home") {
                        setSelected("archived")
                        setFilterArchivedBookmarks(true)
                    }
                }}>
                    <ArchiveIcon className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-800">Archived</p>
                </button>
            </div>
        </div>
    )
}

function Tag({ name, count }: { name: string, count: number }): JSX.Element {
    const { tagFilters, addFilter, removeFilter } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            tagFilters: store.tagFilters,
            addFilter: store.addTagFilter,
            removeFilter: store.removeTagFilter,
        }))
    )

    function handleCheck(event: ChangeEvent<HTMLInputElement>): void {
        if (event.target.checked) {
            addFilter(name)
        } else {
            removeFilter(name)
        }
    }

    return (
        <div className="h-10.5 flex flex-row gap-x-3 px-3 py-2 items-center justify-between">
            <label className="w-full flex flex-row gap-x-2 items-center justify-start">
                <input type="checkbox" checked={tagFilters.includes(name)} onChange={handleCheck}
                       className={`appearance-none size-4 grid place-content-center bg-inherit checked:bg-neutral-500 border border-neutral-500 rounded-4 cursor-pointer`}
                />
                <span className="text-preset-3 text-neutral-800">{name}</span>
            </label>
            <div
                className="w-6 h-6 flex px-2 py-0.5 items-center justify-center rounded-full bg-neutral-100 dark:bg-neutral-d-600 border border-neutral-300 size-4">
                <span className="text-preset-5 text-neutral-800">{count}</span>
            </div>
        </div>
    )
}