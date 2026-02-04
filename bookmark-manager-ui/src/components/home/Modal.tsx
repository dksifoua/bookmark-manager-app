import type { JSX } from "react"
import { BookmarkDeleteContainer } from "@/components/bookmarks/BookmarkDeleteContainer"
import { Sidebar } from "@/components/home/Sidebar"
import { useShallow } from "zustand/react/shallow"
import { type GlobalStore, useGlobalStore } from "@/store"
import { BookmarkAddContainer } from "@/components/bookmarks/BookmarkAddContainer"

export function Modal(): JSX.Element {
    const {
        bookmark,
        isMobileSidebarOpen,
        setIsMobileSidebarOpen,
        isAddDialogOpen,
        setIsAddDialogOpen,
        isDeleteDialogOpen,
        setIsDeleteDialogOpen
    } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            isMobileSidebarOpen: store.isMobileSidebarOpen,
            setIsMobileSidebarOpen: store.setIsMobileSidebarOpen,

            bookmark: store.dialogBookmarkData,
            isAddDialogOpen: store.isAddDialogOpen,
            setIsAddDialogOpen: store.setIsAddDialogOpen,
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
                isAddDialogOpen
                && <div className="fixed inset-0 z-50 flex items-center justify-center bg-modal-70">
                    <BookmarkAddContainer closeModal={() => setIsAddDialogOpen(false, null)}/>
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