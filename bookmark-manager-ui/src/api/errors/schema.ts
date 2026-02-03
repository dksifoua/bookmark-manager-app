import { z } from "zod"

export const BadRequestApiResponseSchema = z.union([
    z.object({
        title: z.string(),
        status: z.number(),
        errors: z.record(z.string(), z.array(z.string())),
    }),
    z.object({
        title: z.string(),
        status: z.number(),
        detail: z.string(),
    })
])
export type BadRequestApiResponse = z.infer<typeof BadRequestApiResponseSchema>

export const UnauthorizedApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})
export type UnauthorizedApiResponse = z.infer<typeof UnauthorizedApiResponseSchema>

export const ForbiddenApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})
export type ForbiddenApiResponse = z.infer<typeof ForbiddenApiResponseSchema>

export const NotFoundApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})
export type NotFoundApiResponse = z.infer<typeof NotFoundApiResponseSchema>

export const ConflictApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})
export type ConflictApiResponse = z.infer<typeof ConflictApiResponseSchema>

export const ErrorApiResponseSchema = z.union([
    BadRequestApiResponseSchema,
    UnauthorizedApiResponseSchema,
    ForbiddenApiResponseSchema,
    NotFoundApiResponseSchema,
    ConflictApiResponseSchema,
])
export type ErrorApiResponse = z.infer<typeof ErrorApiResponseSchema>