import { z } from "zod"
import { type Bookmark, FetchBookmarkApiResponseSchema } from "@/api/bookmarks/schema"
import { parseKnownErrors } from "@/api/errors"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchBookmarks(): Promise<Bookmark[]> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks`, {
        method: "GET",
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 200, response })

    const parsedResponse = z.array(FetchBookmarkApiResponseSchema).safeParse(await response.json())
    if (!parsedResponse.success) {
        throw new Error(`Failed to parse response: ${parsedResponse.error}`)
    }

    return parsedResponse.data
}

export async function addBookmark({ title, url, description, tags }: {
    title: string,
    url: string,
    description: string,
    tags: string[]
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, url, description, tags }),
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 201, response })
}

export async function togglePin({ bookmarkId }: {
    bookmarkId: number
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks/${bookmarkId}/pin`, {
        method: "PATCH",
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 204, response })
}

export async function toggleArchive({ bookmarkId }: {
    bookmarkId: number
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks/${bookmarkId}/archive`, {
        method: "PATCH",
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 204, response })
}

export async function deleteBookmark({ bookmarkId }: {
    bookmarkId: number
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/bookmarks/${bookmarkId}`, {
        method: "DELETE",
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 204, response })
}