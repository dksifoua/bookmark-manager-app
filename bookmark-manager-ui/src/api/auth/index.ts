import type { Result } from "@/types"
import { LoginApiResponseSchema, type LoginResponse, LoginResponseSchema } from "@/api/auth/schema"

export async function login({ email, password }: { email: string, password: string }): Promise<Result<LoginResponse>> {
    const apiUrl = Bun.env.BOOKMARK_MANAGER_API_URL
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/api/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
    })
    if (![200, 400, 401].includes(response.status)) {
        return { success: false, error: new Error(`Unexpected status code: ${response.status}`) }
    }
    
    const parsedResponse = LoginApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        return { success: false, error: parsedResponse.error }
    }
    
    const parsedLoginResponse = LoginResponseSchema.safeParse(parsedResponse.data)
    if (!parsedLoginResponse.success) {
        return { success: false, error: parsedLoginResponse.error }
    }
    
    return { success: true, data: parsedLoginResponse.data }
}

