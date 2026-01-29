import { z } from "zod"

export const BookmarkTagCountApiResponseSchema = z.object({
    id: z.number(),
    name: z.string(),
    count: z.number(),
    archivedCount: z.number(),
})
export type BookmarkTagCount = z.infer<typeof BookmarkTagCountApiResponseSchema>