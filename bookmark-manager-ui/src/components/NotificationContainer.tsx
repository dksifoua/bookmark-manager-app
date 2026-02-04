import { type JSX, useEffect } from "react"
import CheckIcon from "@/assets/images/icon-check.svg"
import CloseIcon from "@/assets/images/icon-close.svg"
import CopyIcon from "@/assets/images/icon-copy.svg"
import PinIcon from "@/assets/images/icon-pin.svg"
import UnpinIcon from "@/assets/images/icon-unpin.svg"
import ArchiveIcon from "@/assets/images/icon-archive.svg"
import UnarchiveIcon from "@/assets/images/icon-unarchive.svg"
import DeleteIcon from "@/assets/images/icon-delete.svg"
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

    const { logo, text } = getLogoAndTextNotificationFrom(notificationType)

    return (
        <div
            className="w-85 h-10.25 flex flex-row gap-x-2 px-3 py-2.5 items-center justify-between rounded-8 bg-neutral-0 border border-neutral-300">
            <img src={logo} alt="Notification Icon" className="w-5 h-5"/>
            <p className="text-preset-4-md text-neutral-900">{text}</p>
            <button onClick={closeNotification} className="cursor-pointer">
                <img src={CloseIcon} alt="Close Icon" className="w-4 h-4"/>
            </button>
        </div>
    )
}

function getLogoAndTextNotificationFrom(type: Nullable<NotificationType>): { logo: string, text: string } {
    switch (type) {
        case "bookmark-added":
            return { logo: CheckIcon, text: "Bookmark added successfully." }
        case "bookmark-updated":
            return { logo: CheckIcon, text: "Bookmark updated successfully" }
        case "bookmark-link-copied":
            return { logo: CopyIcon, text: "Bookmark link copied to clipboard." }
        case "bookmark-pinned":
            return { logo: PinIcon, text: "Bookmark pinned to the top." }
        case "bookmark-unpinned":
            return { logo: UnpinIcon, text: "Bookmark unpinned from the top." }
        case "bookmark-archived":
            return { logo: ArchiveIcon, text: "Bookmark archived." }
        case "bookmark-restored":
            return { logo: UnarchiveIcon, text: "Bookmark restored." }
        case "bookmark-deleted":
            return { logo: DeleteIcon, text: "Bookmark permanently deleted." }
        default:
            return { logo: CheckIcon, text: "Action executed successfully." }
    }
}