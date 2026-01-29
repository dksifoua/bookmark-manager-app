import { z } from "zod"
import { type BookmarkTagCount, BookmarkTagCountApiResponseSchema } from "@/api/tags/schema"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchBookmarkTagCount(): Promise<BookmarkTagCount[]> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmark-tags`, {
        method: "GET",
        credentials: "include"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
    
    const parsedResponse = z.array(BookmarkTagCountApiResponseSchema).safeParse(await response.json())
    if (!parsedResponse.success) {
        throw new Error(`Failed to parse response: ${parsedResponse.error}`)
    }
    
    return parsedResponse.data
}