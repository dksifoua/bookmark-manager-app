import type { BadRequestApiResponse } from "@/api/errors/schema"

export class BadRequestApiError extends Error {
    readonly response: BadRequestApiResponse

    constructor(response: BadRequestApiResponse) {
        super(response.title)
        this.name = response.title
        this.response = response
    }
}