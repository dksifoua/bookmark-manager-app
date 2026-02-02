import { z } from "zod"

export const TagCountApiResponseSchema = z.object({
    id: z.number(),
    name: z.string(),
    count: z.number(),
    archivedCount: z.number(),
})
export type TagCount = z.infer<typeof TagCountApiResponseSchema>