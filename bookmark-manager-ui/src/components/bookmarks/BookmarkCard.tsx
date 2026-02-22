import { type JSX, useRef, useState } from "react"
import {
    ArchiveIcon,
    CopyIcon,
    CreatedIcon,
    DeleteIcon,
    EditIcon,
    LastVisitedIcon,
    MenuBookmarkIcon,
    PinIcon,
    UnarchiveIcon,
    UnpinIcon,
    VisitCountIcon,
    VisitIcon
} from "@/components/icons"
import type { Nullable } from "@/types"
import type { Bookmark } from "@/api/bookmarks/schema"
import { useCloseModal } from "@/hooks/modal.hook"
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { visitBookmark } from "@/api/visits"
import { togglePin } from "@/api/bookmarks"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"

const formatter = new Intl.DateTimeFormat("en-US", { day: "2-digit", month: "short" })

export function BookmarkCard({ bookmark }: { bookmark: Bookmark }): JSX.Element {

    return (
        <div className="w-85.75 md:w-84 xl:w-84.5 h-68 flex flex-col rounded-12 bg-neutral-0">
            <div className="w-full h-57.75 flex flex-col gap-y-4 p-4">
                <BookmarkCardHeader bookmark={bookmark}/>
                <div className="h-px bg-neutral-300"></div>
                <p className="text-preset-4-md text-neutral-800">
                    {bookmark.description}
                </p>
                <BookmarkCardTags tags={bookmark.tags}/>
            </div>
            <BookmarkCardFooter isPinned={bookmark.isPinned} isArchived={bookmark.isArchived}
                                visitCount={bookmark.visitsCount} creationTime={bookmark.creationTime}
                                lastVisitTime={bookmark.lastVisitTime}/>
        </div>
    )
}

function BookmarkCardHeader({ bookmark }: { bookmark: Bookmark }): JSX.Element {
    const ref = useRef<HTMLDivElement>(null)
    const [isDropdownOpen, setIsDropdownOpen] = useState<boolean>(false)

    useCloseModal(ref, (): void => setIsDropdownOpen(false))

    const urlObject = new URL(bookmark.url)
    const formattedUrl = urlObject.pathname === "/"
        ? `${urlObject.host}${urlObject.search}`
        : `${urlObject.host}${urlObject.pathname}${urlObject.search}`

    return (
        <div className="w-full flex flex-row gap-x-3 justify-between relative" ref={ref}>
            <div className="flex flex-row gap-x-3">
                <img src={`https://www.faviconextractor.com/favicon/${urlObject.host}`} alt={`${bookmark.title} Icon`}
                     className="w-11 h-11 rounded-8 border border-neutral-100"/>
                <div className="flex flex-col gap-y-1">
                    <p className="text-preset-2 text-neutral-900">{bookmark.title}</p>
                    <p className="text-preset-5 text-neutral-800">{formattedUrl}</p>
                </div>
            </div>
            <button
                onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                className="w-8 h-8 flex border border-neutral-400 rounded-8 items-center justify-center cursor-pointer"
            >
                <MenuBookmarkIcon className={`w-5 h-5`}/>
            </button>
            <div className="absolute right-0 top-full">
                {
                    isDropdownOpen
                    && <BookmarkActionDropdown bookmark={bookmark} closeDropdown={() => setIsDropdownOpen(false)}/>
                }
            </div>
        </div>
    )
}

function BookmarkActionDropdown({ bookmark, closeDropdown }: {
    bookmark: Bookmark,
    closeDropdown: () => void
}): JSX.Element {
    const { url, isPinned, isArchived } = bookmark

    const queryClient = useQueryClient()
    const { setIsNotificationOpen, setIsArchiveDialogOpen, setUpdateDialogOpen, setIsDeleteDialogOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
            setIsArchiveDialogOpen: store.setIsArchiveDialogOpen,
            setUpdateDialogOpen: store.setIsUpdateDialogOpen,
            setIsDeleteDialogOpen: store.setIsDeleteDialogOpen
        }))
    )
    const { mutate: visitBookmarkFn } = useMutation({
        mutationFn: visitBookmark,
        onSuccess: async () => await queryClient.invalidateQueries({ queryKey: ["bookmarks"] })
    })
    const { mutate: pinToggleFn } = useMutation({
        mutationFn: togglePin,
        onSuccess: async () => await queryClient.invalidateQueries({ queryKey: ["bookmarks"] })
    })

    function handleVisit(): void {
        visitBookmarkFn({ bookmarkId: bookmark.bookmarkId, visitTime: new Date() })
        window.open(url, "_blank", "noopener,noreferrer")
        closeDropdown()
    }

    function handlePin(): void {
        pinToggleFn({ bookmarkId: bookmark.bookmarkId })
        closeDropdown()
        setIsNotificationOpen(true, isPinned ? "bookmark-unpinned" : "bookmark-pinned")
    }

    function handleEdit(): void {
        closeDropdown()
        setUpdateDialogOpen(true, bookmark)
    }

    function handleArchive(): void {
        closeDropdown()
        setIsArchiveDialogOpen(true, bookmark)
    }

    function handleDelete(): void {
        closeDropdown()
        setIsDeleteDialogOpen(true, bookmark)
    }

    return (
        <div className="w-50 flex flex-col gap-y-1 p-2 rounded-8 bg-neutral-0 border border-neutral-100">
            <button onClick={handleVisit}
                    className="w-full h-9 flex flex-row gap-x-2.5 p-2 rounded-8 items-center cursor-pointer hover:bg-neutral-300"
            >
                <VisitIcon className="w-4 h-4"/>
                <p className="text-preset-4 text-neutral-800">Visit</p>
            </button>
            <button onClick={(): void => {
                navigator.clipboard.writeText(url).then(closeDropdown)
                setIsNotificationOpen(true, "bookmark-link-copied")
            }}
                    className="w-full h-9 flex flex-row gap-x-2.5 p-2 rounded-8 items-center cursor-pointer hover:bg-neutral-300">
                <CopyIcon className="w-4 h-4"/>
                <p className="text-preset-4 text-neutral-800">Copy URL</p>
            </button>
            {
                !isArchived
                && <button onClick={handlePin}
                           className="w-full h-9 flex flex-row gap-x-2.5 p-2 rounded-8 items-center cursor-pointer hover:bg-neutral-300">
                    {isPinned ? <UnpinIcon className="w-4 h-4"/> : <PinIcon className="w-4 h-4"/>}
                    <p className="text-preset-4 text-neutral-800">{isPinned ? "Unpin" : "Pin"}</p>
                </button>
            }
            {
                !isArchived
                && <button onClick={handleEdit}
                    className="w-full h-9 flex flex-row gap-x-2.5 p-2 rounded-8 items-center cursor-pointer hover:bg-neutral-300">
                    <EditIcon className="w-4 h-4"/>
                    <p className="text-preset-4 text-neutral-800">Edit</p>
                </button>
            }
            <button onClick={handleArchive}
                    className="w-full h-9 flex flex-row gap-x-2.5 p-2 rounded-8 items-center cursor-pointer hover:bg-neutral-300">
                {isArchived ? <UnarchiveIcon className="w-4 h-4"/> : <ArchiveIcon className="w-4 h-4"/>}
                <p className="text-preset-4 text-neutral-800">{isArchived ? "Unarchive" : "Archive"}</p>
            </button>
            {
                isArchived
                && <button onClick={handleDelete}
                           className="w-full h-9 flex flex-row gap-x-2.5 p-2 rounded-8 items-center cursor-pointer hover:bg-neutral-300">
                    <DeleteIcon className="w-4 h-4"/>
                    <p className="text-preset-4 text-neutral-800">Delete Permanently</p>
                </button>
            }
        </div>
    )
}

function BookmarkCardTags({ tags }: { tags: string[] }): JSX.Element {

    return (
        <div className="flex flex-row gap-x-2">
            {
                tags.map((tag: string, index: number): JSX.Element => (
                    <div key={index} className="flex px-2 py-0.5 rounded-4 bg-neutral-100 dark:bg-neutral-d-600">
                        <p className="text-preset-5 text-neutral-800">{tag}</p>
                    </div>
                ))
            }
        </div>
    )
}

function BookmarkCardFooter({ isPinned, isArchived, visitCount, creationTime, lastVisitTime }: {
    isPinned: boolean,
    isArchived: boolean,
    visitCount: number,
    creationTime: Date,
    lastVisitTime: Nullable<Date>
}): JSX.Element {
    const creationTimeParts = formatter.formatToParts(creationTime)
    const creationTimeDay = creationTimeParts.find(part => part.type === "day")?.value || ""
    const creationTimeMonth = creationTimeParts.find(part => part.type === "month")?.value || ""

    let lastVisitTimeDay = ""
    let lastVisitTimeMonth = ""
    if (lastVisitTime !== null) {
        const lastVisitTimeParts = formatter.formatToParts(lastVisitTime)
        lastVisitTimeDay = lastVisitTimeParts.find(part => part.type === "day")?.value || ""
        lastVisitTimeMonth = lastVisitTimeParts.find(part => part.type === "month")?.value || ""
    }

    return (
        <div
            className="h-10.25 flex flex-row gap-x-8 px-4 py-3 border-t border-neutral-300 items-center justify-between">
            <div className="flex flex-row gap-x-4 items-center">
                <div className="flex flex-row gap-x-1.5 items-center justify-between">
                    <VisitCountIcon className="w-3 h-3"/>
                    <p className="text-preset-5 text-neutral-800">{visitCount}</p>
                </div>
                <div className="flex flex-row gap-x-1.5 items-center justify-between">
                    <LastVisitedIcon className="w-3 h-3"/>
                    <p className="text-preset-5 text-neutral-800">
                        {
                            lastVisitTime === null
                                ? "Never"
                                : `${lastVisitTimeDay} ${lastVisitTimeMonth}`
                        }
                    </p>
                </div>
                <div className="flex flex-row gap-x-1.5 items-center justify-between">
                    <CreatedIcon className="w-3 h-3"/>
                    <p className="text-preset-5 text-neutral-800">{`${creationTimeDay} ${creationTimeMonth}`}</p>
                </div>
            </div>
            <div className="flex items-center justify-center">
                {
                    isArchived
                        ?
                        <p className="px-1.5 rounded-4 text-preset-5 text-neutral-800 bg-neutral-300 border border-neutral-300">Archived</p>
                        : isPinned && <PinIcon className="w-4 h-4"/>
                }
            </div>
        </div>
    )
}