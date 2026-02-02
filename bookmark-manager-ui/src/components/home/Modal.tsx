import type { JSX } from "react"
import { BookmarkDeleteContainer } from "@/components/bookmarks/BookmarkDeleteContainer"
import { Sidebar } from "@/components/home/Sidebar"
import { useShallow } from "zustand/react/shallow"
import { type GlobalStore, useGlobalStore } from "@/store"

export function Modal(): JSX.Element {
    const { bookmark, isMobileSidebarOpen, setIsMobileSidebarOpen, isDeleteDialogOpen, setIsDeleteDialogOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            isMobileSidebarOpen: store.isMobileSidebarOpen,
            setIsMobileSidebarOpen: store.setIsMobileSidebarOpen,
            
            bookmark: store.dialogBookmarkData,
            isDeleteDialogOpen: store.isDeleteDialogOpen,
            setIsDeleteDialogOpen: store.setIsDeleteDialogOpen
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
                isDeleteDialogOpen && bookmark
                && <div className="fixed inset-0 z-50 flex items-center justify-center bg-modal-70">
                    <BookmarkDeleteContainer bookmark={bookmark} closeModal={() => setIsDeleteDialogOpen(false, null)}/>
                </div>
            }
        </div>
    )
}