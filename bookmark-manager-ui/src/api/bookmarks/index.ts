import { z } from "zod"
import { type Bookmark, FetchBookmarkApiResponseSchema } from "@/api/bookmarks/schema"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchBookmarks(): Promise<Bookmark[]> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks`, {
        method: "GET",
        credentials: "include"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }

    const parsedResponse = z.array(FetchBookmarkApiResponseSchema).safeParse(await response.json())
    if (!parsedResponse.success) {
        throw new Error(`Failed to parse response: ${parsedResponse.error}`)
    }

    return parsedResponse.data
}

export async function togglePin({ bookmarkId }: {
    bookmarkId: number
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks/${bookmarkId}/pin`, {
        method: "PATCH",
        credentials: "include"
    })
    if (response.status !== 204) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}

export async function toggleArchive({ bookmarkId }: {
    bookmarkId: number
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks/${bookmarkId}/archive`, {
        method: "PATCH",
        credentials: "include"
    })
    if (response.status !== 204) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}

export async function deleteBookmark({ bookmarkId }: {
    bookmarkId: number
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks/${bookmarkId}`, {
        method: "DELETE",
        credentials: "include"
    })
    if (response.status !== 204) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}