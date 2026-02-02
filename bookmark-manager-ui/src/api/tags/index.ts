import { z } from "zod"
import { type TagCount, TagCountApiResponseSchema } from "@/api/tags/schema"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchTagCount(): Promise<TagCount[]> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/tags`, {
        method: "GET",
        credentials: "include"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }

    const parsedResponse = z.array(TagCountApiResponseSchema).safeParse(await response.json())
    if (!parsedResponse.success) {
        throw new Error(`Failed to parse response: ${parsedResponse.error}`)
    }

    return parsedResponse.data
}