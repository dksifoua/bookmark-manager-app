import { type JSX, useEffect } from "react"
import {
    ArchiveIcon,
    CheckIcon,
    CloseIcon,
    CopyIcon,
    DeleteIcon,
    PinIcon,
    UnarchiveIcon,
    UnpinIcon,
} from "@/components/icons"
import type { Nullable } from "@/types"
import type { NotificationType } from "@/store"

export function NotificationContainer({ notificationType, closeNotification }: {
    notificationType: Nullable<NotificationType>,
    closeNotification: () => void
}): JSX.Element {

    useEffect(() => {
        const timeout = setTimeout(closeNotification, 5000)
        return () => clearTimeout(timeout)
    }, [closeNotification])

    const { icon, text } = getLogoAndTextNotificationFrom(notificationType)

    return (
        <div
            className="w-85 h-10.25 flex flex-row gap-x-2 px-3 py-2.5 items-center justify-between rounded-8 bg-neutral-0 border border-neutral-300">
            {icon}
            <p className="text-preset-4-md text-neutral-900">{text}</p>
            <button onClick={closeNotification} className="cursor-pointer">
                <CloseIcon className="w-4 h-4"/>
            </button>
        </div>
    )
}

function getLogoAndTextNotificationFrom(type: Nullable<NotificationType>): { icon: JSX.Element, text: string } {
    switch (type) {
        case "bookmark-added":
            return { icon: <CheckIcon className="w-5 h-5"/>, text: "Bookmark added successfully." }
        case "bookmark-updated":
            return { icon: <CheckIcon className="w-5 h-5"/>, text: "Bookmark updated successfully" }
        case "bookmark-link-copied":
            return { icon: <CopyIcon className="w-5 h-5"/>, text: "Bookmark link copied to clipboard." }
        case "bookmark-pinned":
            return { icon: <PinIcon className="w-5 h-5"/>, text: "Bookmark pinned to the top." }
        case "bookmark-unpinned":
            return { icon: <UnpinIcon className="w-5 h-5"/>, text: "Bookmark unpinned from the top." }
        case "bookmark-archived":
            return { icon: <ArchiveIcon className="w-5 h-5"/>, text: "Bookmark archived." }
        case "bookmark-restored":
            return { icon: <UnarchiveIcon className="w-5 h-5"/>, text: "Bookmark restored." }
        case "bookmark-deleted":
            return { icon: <DeleteIcon className="w-5 h-5"/>, text: "Bookmark permanently deleted." }
        default:
            return { icon: <CheckIcon className="w-5 h-5"/>, text: "Action executed successfully." }
    }
}