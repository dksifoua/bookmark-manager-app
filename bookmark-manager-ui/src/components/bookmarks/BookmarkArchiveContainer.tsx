import { type JSX } from "react"
import CloseIcon from "@/assets/images/icon-close.svg"
import type { Bookmark } from "@/api/bookmarks/schema"
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { toggleArchive } from "@/api/bookmarks"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"

export function BookmarkArchiveContainer({ bookmark, closeModal }: {
    bookmark: Bookmark,
    closeModal: () => void
}): JSX.Element {
    const { bookmarkId, title, isArchived } = bookmark
    const queryClient = useQueryClient()
    const { setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
        }))
    )
    const { mutate } = useMutation({
        mutationFn: toggleArchive,
        onSuccess: () => Promise.all([
            queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
            queryClient.invalidateQueries({ queryKey: ["tags"] })
        ]).then(() => {
            closeModal()
            setIsNotificationOpen(true, isArchived ? "bookmark-restored" : "bookmark-archived")
        })
    })

    return (
        <div className="w-112.5 flex flex-col gap-y-8 p-8 rounded-16 bg-neutral-0 relative">
            <button onClick={closeModal}
                    className="w-8 h-8 flex absolute top-2.5 right-2.5 rounded-8 border border-neutral-400 cursor-pointer items-center justify-center">
                <img src={CloseIcon} alt="Close Icon" className="w-5 h-5"/>
            </button>
            <div className="flex flex-col gap-y-2">
                <p className="text-preset-1 text-neutral-900">
                    {
                        isArchived ? "Restore bookmark" : "Archive bookmark"
                    }
                </p>
                <p className="text-preset-4-md text-neutral-800">
                    {
                        isArchived
                            ? <span>Move the bookmark [<span className="text-neutral-900">{title}</span>] back to your active list?</span>
                            : <span>Are you sure you want to archive the bookmark [<span className="text-neutral-900">{title}</span>]?</span>
                    }
                </p>
            </div>
            <div className="flex flex-row gap-x-4 items-center justify-end">
                <button onClick={closeModal}
                        className="h-11.5 flex px-4 py-3 bg-neutral-0 rounded-8 border border-neutral-400 items-center justify-center cursor-pointer">
                    <p className="text-preset-3 text-neutral-900">Cancel</p>
                </button>
                <button onClick={() => mutate({ bookmarkId })}
                        className="h-11.5 flex px-4 py-3 bg-teal-700 rounded-8 items-center justify-center cursor-pointer">
                    <p className="text-preset-3 text-neutral-0">
                        {
                            isArchived ? "Restore" : "Archive"
                        }
                    </p>
                </button>
            </div>
        </div>
    )
}