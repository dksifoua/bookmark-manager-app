import { z } from "zod"

export const TagApiResponseSchema = z.object({
    tagId: z.number(),
    name: z.string(),
})
export type Tag = z.infer<typeof TagApiResponseSchema>