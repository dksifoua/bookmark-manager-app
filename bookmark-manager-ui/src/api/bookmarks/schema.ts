import { z } from "zod"

export const FetchBookmarkApiResponseSchema = z.object({
    bookmarkId: z.number(),
    title: z.string(),
    url: z.url(),
    description: z.string(),
    isPinned: z.boolean(),
    isArchived: z.boolean(),
    tags: z.array(z.string()),
    creationTime: z.coerce.date(),
    visitsCount: z.number(),
    lastVisitTime: z.coerce.date().nullable()
})
export type Bookmark = z.infer<typeof FetchBookmarkApiResponseSchema>