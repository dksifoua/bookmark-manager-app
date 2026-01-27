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
export type LoginResponse = z.infer<typeof LoginApiResponseSchema>

export const SuccessfulRegistrationApiResponseSchema = z.object({
    fullname: z.string(),
    email: z.string(),
})
export const ConflictRegistrationApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})

export const RegistrationApiResponseSchema = z.union([
    SuccessfulRegistrationApiResponseSchema,
    ConflictRegistrationApiResponseSchema,
])
export type RegistrationResponse = z.infer<typeof RegistrationApiResponseSchema>