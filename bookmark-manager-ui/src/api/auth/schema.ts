import { z } from "zod"

export const SuccessfulLoginApiResponseSchema = z.object({
    fullname: z.string(),
    email: z.string(),
})

export const UnauthorizedLoginApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})

export const BadRequestLoginApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    errors: z.record(z.string(), z.array(z.string())),
})

export const LoginApiResponseSchema = z.union([
    SuccessfulLoginApiResponseSchema,
    UnauthorizedLoginApiResponseSchema,
    BadRequestLoginApiResponseSchema,
])

export const LoginResponseSchema = LoginApiResponseSchema.transform((response) => {
    return response
})
export type LoginResponse = z.infer<typeof LoginResponseSchema>