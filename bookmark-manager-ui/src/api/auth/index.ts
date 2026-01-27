import { LoginApiResponseSchema, type LoginResponse, LoginResponseSchema } from "@/api/auth/schema"

const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function authLogin({ email, password }: { email: string, password: string }): Promise<LoginResponse> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
        credentials: "include"
    })
    if (![200, 400, 401].includes(response.status)) { // OK, Bad Request, Unauthorized
        throw new Error(`Unexpected status code: ${response.status}`)
    }
    
    const parsedResponse = LoginApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }
    
    const parsedLoginResponse = LoginResponseSchema.safeParse(parsedResponse.data)
    if (!parsedLoginResponse.success) {
        throw parsedLoginResponse.error
    }
    
    return parsedLoginResponse.data
}

export async function authLogout(): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/logout`, {
        method: "POST",
        credentials: "include"
    })
    if (response.status !== 200) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}

