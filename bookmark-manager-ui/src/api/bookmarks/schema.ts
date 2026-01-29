import { z } from "zod"

export const BookmarkApiResponseSchema = z.object({
    bookmarkId: z.number(),
    title: z.string(),
    url: z.url(),
    description: z.string(),
    isPinned: z.boolean(),
    isArchived: z.boolean(),
    tags: z.array(z.string()),
    creationTime: z.coerce.date(),
    visitCount: z.number(),
    lastVisitTime: z.coerce.date().nullable()
})
export type BookmarkResponse = z.infer<typeof BookmarkApiResponseSchema>