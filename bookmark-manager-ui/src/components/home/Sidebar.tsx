import { type ChangeEvent, type JSX, useEffect, useState } from "react"
import { Logo } from "@/components/Logo"
import HomeIcon from "@/assets/images/icon-home.svg"
import ArchiveIcon from "@/assets/images/icon-archive.svg"
import CloseIcon from "@/assets/images/icon-close.svg"
import { useQuery } from "@tanstack/react-query"
import { fetchTagCount } from "@/api/tags"
import type { TagCount } from "@/api/tags/schema"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"
import { useAuthContext } from "@/hooks/auth.hook"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"

export function Sidebar({ openSidebar }: { openSidebar?: () => void }): JSX.Element {
    const { data: tags, isLoading, isError, error } = useQuery({
        queryKey: ["tags"],
        queryFn: fetchTagCount,
        select: (tags: TagCount[]): TagCount[] =>
            [...tags].sort((a: TagCount, b: TagCount): number => a.name.localeCompare(b.name))
    })

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
                        <img src={CloseIcon} alt="Close Icon" className="w-8 h-8"/>
                    </button>
                }
            </div>
            <Navigation/>
            <div className="flex flex-col px-4">
                <p className="px-3 pb-4 text-preset-5">TAGS</p>
                {
                    isLoading
                        ? <p className="px-3 text-preset-3 text-neutral-800">Loading tags...</p>
                        : tags?.map((tag: TagCount): JSX.Element =>
                            <Tag key={tag.id} tag={tag}/>)
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
                    <img src={HomeIcon} alt="Home Icon" className="w-5 h-5"/>
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
                    <img src={ArchiveIcon} alt="Archive Icon" className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-800">Archived</p>
                </button>
            </div>
        </div>
    )
}

function Tag({ tag }: { tag: TagCount }): JSX.Element {
    const { tagFilters, addFilter, removeFilter, filterArchivedBookmarks } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            tagFilters: store.tagFilters,
            addFilter: store.addTagFilter,
            removeFilter: store.removeTagFilter,
            filterArchivedBookmarks: store.filterArchivedBookmarks
        }))
    )

    const { name, count, archivedCount } = tag

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
                       className={`size-4 border border-neutral-500 cursor-pointer`}
                />
                <span className="text-preset-3 text-neutral-800">{name}</span>
            </label>
            <div
                className="w-6 h-6 flex px-2 py-0.5 items-center justify-center rounded-full bg-neutral-100 border border-neutral-300 size-4">
                <span className="text-preset-5 text-neutral-800">{
                    filterArchivedBookmarks ? archivedCount : count
                }</span>
            </div>
        </div>
    )
}