import { BookmarkApiResponseSchema, type BookmarkResponse } from "@/api/bookmarks/schema"
import { z } from "zod"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchBookmarks(): Promise<BookmarkResponse[]> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks`, {
        method: "GET",
        credentials: "include"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
    
    const parsedResponse = z.array(BookmarkApiResponseSchema).safeParse(await response.json())
    if (!parsedResponse.success) {
        throw new Error(`Failed to parse response: ${parsedResponse.error}`)
    }

    return parsedResponse.data
}