import { z } from "zod"

export const AuthApiResponseSchema = z.object({
    fullname: z.string(),
    email: z.string(),
})
export type AuthApiResponse = z.infer<typeof AuthApiResponseSchema>