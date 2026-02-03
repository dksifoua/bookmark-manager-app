import {
    BadRequestApiResponseSchema,
    ConflictApiResponseSchema, ForbiddenApiResponseSchema, NotFoundApiResponseSchema,
    UnauthorizedApiResponseSchema
} from "@/api/errors/schema"
import { BadRequestApiError } from "@/api/errors/BadRequestApiError"
import { UnauthorizedApiError } from "@/api/errors/UnauthorizedApiError"
import { ConflictApiError } from "@/api/errors/ConflictApiError"
import { ForbiddenApiError } from "@/api/errors/ForbiddenApiError"
import { NotFoundApiError } from "@/api/errors/NotFoundApiError"

export async function parseKnownErrors({ expectedStatusCode, response }: {
    expectedStatusCode: number,
    response: Response
}): Promise<void> {
    if (response.status === 400) {
        const parsedResponse = BadRequestApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new BadRequestApiError(parsedResponse.data)
    }
    if (response.status === 401) {
        const parsedResponse = UnauthorizedApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new UnauthorizedApiError(parsedResponse.data)
    }
    if (response.status === 403) {
        const parsedResponse = ForbiddenApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new ForbiddenApiError(parsedResponse.data)
    }
    if (response.status === 404) {
        const parsedResponse = NotFoundApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new NotFoundApiError(parsedResponse.data)
    }
    if (response.status === 409) {
        const parsedResponse = ConflictApiResponseSchema.safeParse(await response.json())
        if (!parsedResponse.success) {
            throw parsedResponse.error
        }
        throw new ConflictApiError(parsedResponse.data)
    }

    if (response.status !== expectedStatusCode) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}