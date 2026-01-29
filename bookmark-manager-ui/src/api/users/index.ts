import { type CurrentUser, FetchCurrentUserApiResponseSchema } from "@/api/users/schema"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function fetchCurrentUser(): Promise<CurrentUser> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/users/me`, {
        method: "GET",
        credentials: "include"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
    
    const parsedResponse = FetchCurrentUserApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }
    
    return parsedResponse.data
}