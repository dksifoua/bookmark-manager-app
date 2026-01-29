import { z } from "zod"

export const FetchCurrentUserApiResponseSchema = z.object({
    fullname: z.string(),
    email: z.string(),
})

export type CurrentUser = z.infer<typeof FetchCurrentUserApiResponseSchema>