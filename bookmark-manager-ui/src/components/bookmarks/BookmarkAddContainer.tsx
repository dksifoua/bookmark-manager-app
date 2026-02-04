import { type JSX, type SyntheticEvent, useState } from "react"
import CloseIcon from "@/assets/images/icon-close.svg"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { fetchTagCount } from "@/api/tags"
import type { TagCount } from "@/api/tags/schema"
import { addBookmark } from "@/api/bookmarks"
import LoadingIcon from "@/assets/images/icon-loading.svg"
import { ApiError } from "@/api/errors/ApiError"
import type { Nullable } from "@/types"
import type { ErrorApiResponse } from "@/api/errors/schema"
import { type GlobalStore, useGlobalStore } from "@/store"
import { useShallow } from "zustand/react/shallow"

export function BookmarkAddContainer({ closeModal }: { closeModal: () => void }): JSX.Element {
    const [tags, setTags] = useState<string[]>([])
    const [currentTag, setCurrentTag] = useState<string>("")
    const queryClient = useQueryClient()
    const { data: tagSuggestions } = useQuery({
        queryKey: ["tags"],
        queryFn: fetchTagCount,
        select: (tags: TagCount[]): TagCount[] =>
            [...tags].sort((a: TagCount, b: TagCount): number => a.name.localeCompare(b.name))
    })
    const { setIsNotificationOpen } = useGlobalStore(
        useShallow((store: GlobalStore) => ({
            setIsNotificationOpen: store.setIsNotificationOpen,
        }))
    )
    const { mutate, isPending, error: mutationError } = useMutation({
        mutationFn: addBookmark,
        onSuccess: (): Promise<void> => Promise.all([
            queryClient.invalidateQueries({ queryKey: ["bookmarks"] }),
            queryClient.invalidateQueries({ queryKey: ["tags"] })
        ]).then((): void => {
            closeModal()
            setIsNotificationOpen(true, "bookmark-added")
        })
    })
    
    const error: Nullable<ErrorApiResponse> = mutationError instanceof ApiError ? mutationError.response : null
    
    function handleSubmit(event: SyntheticEvent<HTMLFormElement>): void {
        event.preventDefault()
        
        const formData = new FormData(event.currentTarget)
        const title = formData.get("title") as string
        const description = formData.get("description") as string
        const url = formData.get("url") as string
        
        mutate({ title, description, url, tags })
    }

    return (
        <form onSubmit={handleSubmit} className="w-85.75 md:w-md xl:w-142.5 flex flex-col gap-y-8 p-8 rounded-16 bg-neutral-0 relative">
            <button type="reset" onClick={closeModal}
                    className="w-8 h-8 flex absolute top-2.5 right-2.5 rounded-8 border border-neutral-400 cursor-pointer items-center justify-center">
                <img src={CloseIcon} alt="Close Icon" className="w-5 h-5"/>
            </button>
            <div className="flex flex-col gap-y-2">
                <p className="text-preset-1 text-neutral-900">Add a bookmark</p>
                <p className="text-preset-4-md text-neutral-800">
                    Save a link with details to keep your collection organized. We extract the favicon automatically
                    from
                    the URL.
                </p>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="title" className="text-preset-4 color-neutral-900">Title *</label>
                    <input type="text" id="title" name="title" autoComplete="off" required={false}
                           className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                    {
                        error !== null && "errors" in error && "Title" in error.errors &&
                        <div className="flex flex-col gap-y-1.5">
                            {
                                error.errors["Title"].map((error: string, index: number) => (
                                    <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                ))
                            }
                        </div>
                    }
                </div>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="url" className="text-preset-4 color-neutral-900">Website URL *</label>
                    <input type="text" id="url" name="url" autoComplete="off" required={false}
                           className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                    {
                        error !== null && "errors" in error && "Url" in error.errors &&
                        <div className="flex flex-col gap-y-1.5">
                            {
                                error.errors["Url"].map((error: string, index: number) => (
                                    <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                ))
                            }
                        </div>
                    }
                </div>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="description" className="text-preset-4 color-neutral-900">Description *</label>
                    <textarea id="description" name="description" autoComplete="off" required={false}
                              className="h-22.75 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                    {
                        error !== null && "errors" in error && "Description" in error.errors &&
                        <div className="flex flex-col gap-y-1.5">
                            {
                                error.errors["Description"].map((error: string, index: number) => (
                                    <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                ))
                            }
                        </div>
                    }
                </div>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="tags" className="text-preset-4 color-neutral-900">Tags *</label>
                    <div className="flex flex-wrap gap-2 mb-2">
                        {
                            tags.map((tag: string, index: number): JSX.Element => (
                                <div key={index}
                                     className="bg-teal-700 text-neutral-0 px-2 py-1 rounded-8 flex items-center gap-x-1.5 text-preset-4"
                                >
                                    {tag}
                                    <button
                                        onClick={(): void => setTags(tags.filter((_: string, i: number): boolean => i !== index))}
                                        className="text-neutral-0 cursor-pointer"
                                    >
                                        ×
                                    </button>
                                </div>
                            ))
                        }
                    </div>
                    <input
                        type="text"
                        value={currentTag}
                        onChange={(e): void => setCurrentTag(e.target.value)}
                        onKeyDown={(e): void => {
                            if (e.key === "," || e.key === "Enter") {
                                e.preventDefault()
                                if (currentTag.trim()) {
                                    setTags([...tags, currentTag.trim()])
                                    setCurrentTag("")
                                }
                            }
                        }}
                        onBlur={(): void => {
                            if (currentTag.trim()) {
                                setTags([...tags, currentTag.trim()])
                                setCurrentTag("")
                            }
                        }}
                        list="tag-suggestions"
                        className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"
                    />
                    {
                        error !== null && "errors" in error && "Tags" in error.errors &&
                        <div className="flex flex-col gap-y-1.5">
                            {
                                error.errors["Tags"].map((error: string, index: number) => (
                                    <span key={index} className="text-preset-4 text-red-800">{error}</span>
                                ))
                            }
                        </div>
                    }
                    <datalist id="tag-suggestions">
                        {
                            tagSuggestions && tagSuggestions.map((tag: TagCount): JSX.Element => (
                                <option key={tag.name} value={tag.name}>{tag.name}</option>
                            ))
                        }
                    </datalist>
                </div>
            </div>
            {
                error !== null && "detail" in error &&
                <div className="flex flex-col gap-y-1.5">
                    <span className="text-preset-4 text-red-800">{error.detail}</span>
                </div>
            }
            <div className="flex flex-row gap-x-4 items-center justify-end">
                <button type="reset" onClick={closeModal}
                        className="h-11.5 flex px-4 py-3 bg-neutral-0 rounded-8 border border-neutral-400 items-center justify-center cursor-pointer">
                    <p className="text-preset-3 text-neutral-900">Cancel</p>
                </button>
                <button type="submit"
                    className="h-11.5 flex flex-row gap-x-2 px-4 py-3 bg-teal-700 rounded-8 items-center justify-center cursor-pointer">
                    {
                        isPending && <img src={LoadingIcon} alt="Loading Icon" className="w-4 h-4 spin-slow"/>
                    }
                    <p className="text-preset-3 text-neutral-0">Add Bookmark</p>
                </button>
            </div>
        </form>
    )
}