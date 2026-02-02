import { z } from "zod"

export const BadRequestApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    errors: z.record(z.string(), z.array(z.string())),
})
export type BadRequestApiResponse = z.infer<typeof BadRequestApiResponseSchema>

export const UnauthorizedApiResponseSchema = z.object({
    title: z.string(),
    status: z.number(),
    detail: z.string(),
})
export type UnauthorizedApiResponse = z.infer<typeof UnauthorizedApiResponseSchema>