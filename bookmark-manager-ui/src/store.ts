import { create } from "zustand/react"
import { devtools } from "zustand/middleware"

export type GlobalStore = {
    headerTitle: string
    filterArchivedBookmarks: boolean
    setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean) => void
    tagFilters: string[]
    addTagFilter: (tag: string) => void
    removeTagFilter: (tag: string) => void
    tag2checked: Record<string, boolean>
    setTag2checked: (name: string, checked: boolean) => void
}

export const useGlobalStore = create<GlobalStore>()(
    devtools<GlobalStore>((set) => ({
        headerTitle: "All bookmarks",
        filterArchivedBookmarks: false,
        setFilterArchivedBookmarks: (filterArchivedBookmarks: boolean): void => set((store: GlobalStore) => {
            const prefix = "Archived - "
            const headerTitle = filterArchivedBookmarks
                ? prefix + store.headerTitle
                : store.headerTitle.startsWith(prefix)
                    ? store.headerTitle.slice(prefix.length)
                    : store.headerTitle
            return { filterArchivedBookmarks, headerTitle }
        }),
        tagFilters: [],
        addTagFilter: (tag: string): void => set((store: GlobalStore) => {
            const prevSuffix = ` tagged with [${store.tagFilters.join(", ")}]`
            let headerTitle = store.headerTitle.endsWith(prevSuffix)
                ? store.headerTitle.slice(0, -prevSuffix.length)
                : store.headerTitle

            const tagFilters = [...store.tagFilters, tag]
            const suffix = ` tagged with [${tagFilters.join(", ")}]`
            headerTitle = headerTitle + suffix

            return { tagFilters, headerTitle }
        }),
        removeTagFilter: (tag: string): void => set((store: GlobalStore) => {
            const prevSuffix = ` tagged with [${store.tagFilters.join(", ")}]`
            let headerTitle = store.headerTitle.endsWith(prevSuffix)
                ? store.headerTitle.slice(0, -prevSuffix.length)
                : store.headerTitle

            const tagFilters = store.tagFilters.filter((t: string): boolean => t !== tag)
            const suffix = ` tagged with [${tagFilters.join(", ")}]`
            headerTitle = tagFilters.length === 0
                ? headerTitle
                : headerTitle + suffix

            return { tagFilters, headerTitle }
        }),
        tag2checked: {},
        setTag2checked: (name: string, checked: boolean) => set((store) => {
            const tag2checked = { ...store.tag2checked, [name]: checked }

            return { tag2checked }
        })
    }))
)