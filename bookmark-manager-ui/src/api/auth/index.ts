import {
    LoginApiResponseSchema,
    type LoginResponse,
    RegistrationApiResponseSchema,
    type RegistrationResponse
} from "@/api/auth/schema"

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

    return parsedResponse.data
}

export async function authLogout(): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/logout`, {
        method: "POST",
        credentials: "include"
    })
    if (![200, 401].includes(response.status)) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}

export async function authRegister({ fullname, email, password }: {
    fullname: string,
    email: string,
    password: string
}): Promise<RegistrationResponse> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ fullname, email, password }),
        credentials: "include"
    })
    if (![201, 409].includes(response.status)) { // Created, Conflict
        console.log(await response.json())
        throw new Error(`Unexpected status code: ${response.status}`)
    }

    const parsedResponse = RegistrationApiResponseSchema.safeParse(await response.json())
    if (!parsedResponse.success) {
        throw parsedResponse.error
    }

    return parsedResponse.data
}
