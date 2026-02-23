import { create } from "zustand/react"
import { persist } from "zustand/middleware"
import type { Nullable } from "@/types"
import type { Bookmark } from "@/api/bookmarks/schema"

export type SortBookmarksBy = "recently-added" | "most-visited" | "last-visited"
export type NotificationType =
    | "bookmark-added"
    | "bookmark-updated"
    | "bookmark-link-copied"
    | "bookmark-pinned"
    | "bookmark-unpinned"
    | "bookmark-archived"
    | "bookmark-restored"
    | "bookmark-deleted"

export type GlobalStore = {
    headerTitle: string

    searchQuery: string
    setSearchQuery: (searchQuery: string) => void

    sortBookmarksBy: SortBookmarksBy
    setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => void

    tagFilters: string[]
    addTagFilter: (tag: string) => void
    removeTagFilter: (tag: string) => void

    filterArchivedBookmarks: boolean
    setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => void

    isMobileSidebarOpen: boolean
    setIsMobileSidebarOpen: (isMobileSidebarOpen: boolean) => void

    dialogBookmarkData: Nullable<Bookmark>

    isAddDialogOpen: boolean
    setIsAddDialogOpen: (isAddDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => void

    isUpdateDialogOpen: boolean
    setIsUpdateDialogOpen: (isUpdateDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => void

    isArchiveDialogOpen: boolean
    setIsArchiveDialogOpen: (isArchiveDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => void

    isDeleteDialogOpen: boolean
    setIsDeleteDialogOpen: (isDeleteDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => void

    notificationType: Nullable<NotificationType>
    isNotificationOpen: boolean
    setIsNotificationOpen: (isNotificationOpen: boolean, notificationType: Nullable<NotificationType>) => void
}

export const useGlobalStore = create<GlobalStore>()(
    persist<GlobalStore>(
        (set) => ({
            headerTitle: "Bookmarks",
            searchQuery: "",
            setSearchQuery: (searchQuery: string) => set((store) => {
                const {tagFilters, filterArchivedBookmarks} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})

                return { searchQuery, headerTitle }
            }),
            sortBookmarksBy: "recently-added",
            setSortBookmarksBy: (sortBookmarksBy: SortBookmarksBy) => set({ sortBookmarksBy }),
            tagFilters: [],
            addTagFilter: (tag: string) => set((store) => {
                const tagFilters = [...store.tagFilters, tag]
                const {filterArchivedBookmarks, searchQuery} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})

                return { headerTitle, tagFilters }
            }),
            removeTagFilter: (tag: string) => set((store) => {
                const tagFilters = store.tagFilters.filter((t) => t !== tag)
                const {filterArchivedBookmarks, searchQuery} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})

                return { headerTitle, tagFilters }
            }),
            filterArchivedBookmarks: false,
            setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => set((store) => {
                const {tagFilters, searchQuery} = store
                const headerTitle = buildHeaderTitle({tagFilters, filterArchivedBookmarks, searchQuery})

                return { headerTitle, filterArchivedBookmarks }
            }),
            isMobileSidebarOpen: false,
            setIsMobileSidebarOpen: (isMobileSidebarOpen: boolean) => set(() => {
                return { isMobileSidebarOpen }
            }),

            dialogBookmarkData: null,

            isAddDialogOpen: false,
            setIsAddDialogOpen: (isAddDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => set(() => {
                return { isAddDialogOpen, dialogBookmarkData }
            }),

            isUpdateDialogOpen: false,
            setIsUpdateDialogOpen: (isUpdateDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => set(() => {
                return { isUpdateDialogOpen, dialogBookmarkData }
            }),

            isArchiveDialogOpen: false,
            setIsArchiveDialogOpen: (isArchiveDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => set(() => {
                return { isArchiveDialogOpen, dialogBookmarkData }
            }),

            isDeleteDialogOpen: false,
            setIsDeleteDialogOpen: (isDeleteDialogOpen: boolean, dialogBookmarkData: Nullable<Bookmark>) => set(() => {
                return { isDeleteDialogOpen, dialogBookmarkData }
            }),

            notificationType: null,
            isNotificationOpen: false,
            setIsNotificationOpen: (isNotificationOpen: boolean, notificationType: Nullable<NotificationType>) => set(() => {
                return { isNotificationOpen, notificationType }
            })
        }),
        {
            name: "global-state",
        }
    ),
)

function buildHeaderTitle({ tagFilters, filterArchivedBookmarks, searchQuery }: {
    tagFilters: string[],
    filterArchivedBookmarks: boolean,
    searchQuery: string
}): string {
    const parts: string[] = []

    if (searchQuery.length > 1) {
        parts.push("Search")
    }

    if (filterArchivedBookmarks) {
        parts.push("Archives")
    }

    parts.push("Bookmarks")

    if (searchQuery.length > 1) {
        parts.push(`"${searchQuery}"`)
    }

    if (tagFilters.length > 0) {
        parts.push(`Tagged with: [${tagFilters.join(', ')}]`)
    }

    return parts.join(" > ")
}
