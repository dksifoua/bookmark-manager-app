import type { JSX } from "react"
import { BookmarkDeleteContainer } from "@/components/bookmarks/BookmarkDeleteContainer"
import { Sidebar } from "@/components/home/Sidebar"
import { useShallow } from "zustand/react/shallow"
import { type GlobalStore, useGlobalStore } from "@/store"
import { BookmarkAddContainer } from "@/components/bookmarks/BookmarkAddContainer"
import { NotificationContainer } from "@/components/NotificationContainer"
import { BookmarkArchiveContainer } from "@/components/bookmarks/BookmarkArchiveContainer"

export function Modal(): JSX.Element {
    const {
        bookmark,
        isMobileSidebarOpen,
        setIsMobileSidebarOpen,
        isAddDialogOpen,
        setIsAddDialogOpen,
        isArchiveDialogOpen,
        setIsArchiveDialogOpen,
        isDeleteDialogOpen,
        setIsDeleteDialogOpen,
        notificationType,
        isNotificationOpen,
        setIsNotificationOpen,
    } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            isMobileSidebarOpen: store.isMobileSidebarOpen,
            setIsMobileSidebarOpen: store.setIsMobileSidebarOpen,
            bookmark: store.dialogBookmarkData,
            isAddDialogOpen: store.isAddDialogOpen,
            setIsAddDialogOpen: store.setIsAddDialogOpen,
            isArchiveDialogOpen: store.isArchiveDialogOpen,
            setIsArchiveDialogOpen: store.setIsArchiveDialogOpen,
            isDeleteDialogOpen: store.isDeleteDialogOpen,
            setIsDeleteDialogOpen: store.setIsDeleteDialogOpen,
            notificationType: store.notificationType,
            isNotificationOpen: store.isNotificationOpen,
            setIsNotificationOpen: store.setIsNotificationOpen
        }))
    )

    return (
        <div>
            {
                isMobileSidebarOpen
                && <div className="fixed inset-0 z-50 flex items-center justify-start bg-modal-70">
                    <Sidebar openSidebar={() => setIsMobileSidebarOpen(false)}/>
                </div>
            }
            {
                isAddDialogOpen
                && <div className="fixed inset-0 z-50 flex items-center justify-center bg-modal-70">
                    <BookmarkAddContainer closeModal={() => setIsAddDialogOpen(false, null)}/>
                </div>
            }
            {
                isArchiveDialogOpen && bookmark
                && <div className="fixed inset-0 z-50 flex items-center justify-center bg-modal-70">
                    <BookmarkArchiveContainer bookmark={bookmark} closeModal={() => setIsArchiveDialogOpen(false, null)}/>
                </div>
            }
            {
                isDeleteDialogOpen && bookmark
                && <div className="fixed inset-0 z-50 flex items-center justify-center bg-modal-70">
                    <BookmarkDeleteContainer bookmark={bookmark} closeModal={() => setIsDeleteDialogOpen(false, null)}/>
                </div>
            }
            {
                isNotificationOpen
                && <div className="fixed top-25 right-10 z-50 animate-slide-in-from-top">
                    <NotificationContainer 
                        notificationType={notificationType}
                        closeNotification={() => setIsNotificationOpen(false, null)}/>
                </div>
            }
        </div>
    )
}