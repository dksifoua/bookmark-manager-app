import { z } from "zod"
import { type Tag, TagApiResponseSchema } from "@/api/tags/schema"
import { parseKnownErrors } from "@/api/errors"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchTags(): Promise<Tag[]> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/tags`, {
        method: "GET",
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 200, response })

    const parsedResponse = z.array(TagApiResponseSchema).safeParse(await response.json())
    if (!parsedResponse.success) {
        throw new Error(`Failed to parse response: ${parsedResponse.error}`)
    }

    return parsedResponse.data
}