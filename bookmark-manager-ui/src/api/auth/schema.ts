import { z } from "zod"

export const LoginApiResponseSchema = z.union([
    z.object({
        token: z.jwt(),
        expiresAt: z.string(),
    }),
    z.object({
        title: z.string(),
        status: z.number(),
        detail: z.string(),
    }),
    z.object({
        title: z.string(),
        status: z.number(),
        errors: z.record(z.string(), z.array(z.string())),
    })
])

export const LoginResponseSchema = LoginApiResponseSchema.transform((response) => {
    if ("token" in response) {
        return {
            status: 200,
            token: response.token,
            expiresAt: new Date(response.expiresAt)
        }
    }
    
    return response
})
export type LoginResponse = z.infer<typeof LoginResponseSchema>