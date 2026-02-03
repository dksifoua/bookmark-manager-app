import { type AuthApiResponse, AuthApiResponseSchema } from "@/api/auth/schema"
import { parseKnownErrors } from "@/api/errors"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function authLogin({ email, password }: { email: string, password: string }): Promise<AuthApiResponse> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")
    console.log(apiUrl)

    const response = await fetch(`${apiUrl}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 200, response })

    const parsedResponse = AuthApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }

    return parsedResponse.data
}

export async function authLogout(): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/logout`, {
        method: "POST"
    })
    await parseKnownErrors({ expectedStatusCode: 204, response })
}

export async function authRegister({ fullname, email, password }: { fullname: string, email: string, password: string }): Promise<AuthApiResponse> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ fullname, email, password }),
        credentials: "include"
    })
    await parseKnownErrors({ expectedStatusCode: 201, response })

    const parsedResponse = AuthApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }

    return parsedResponse.data
}