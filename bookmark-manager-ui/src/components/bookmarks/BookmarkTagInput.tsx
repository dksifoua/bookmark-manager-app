import { type JSX, useState } from "react"
import type { Nullable } from "@/types"
import type { ErrorApiResponse } from "@/api/errors/schema"
import type { TagCount } from "@/api/tags/schema"

export function BookmarkTagInput({ tags, tagSuggestions, error, setTags }: {
    tags: string[],
    tagSuggestions: TagCount[] | undefined,
    error: Nullable<ErrorApiResponse>,
    setTags: (tags: string[]) => void
}): JSX.Element {
    const [currentTag, setCurrentTag] = useState<string>("")

    return (
        <div className="flex flex-col gap-y-5">
            <div className="flex flex-col gap-y-1.5">
                <label htmlFor="tags" className="text-preset-4 color-neutral-900">Tags (comma separated) *</label>
                <div className="flex flex-wrap gap-2 mb-2">
                    {
                        tags.map((tag: string, index: number): JSX.Element => (
                            <div key={index}
                                 className="bg-teal-700 text-neutral-0 dark:text-neutral-d-0 px-2 py-1 rounded-8 flex items-center gap-x-1.5 text-preset-4"
                            >
                                {tag}
                                <button
                                    type="button"
                                    onClick={(): void => setTags([...tags.filter((_: string, i: number): boolean => i !== index)])}
                                    className="text-neutral-0 dark:text-neutral-d-0 cursor-pointer"
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
    )
}