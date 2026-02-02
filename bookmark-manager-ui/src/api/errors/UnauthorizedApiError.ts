import type { UnauthorizedApiResponse } from "@/api/errors/schema"

export class UnauthorizedApiError extends Error {
    readonly response: UnauthorizedApiResponse

    constructor(response: UnauthorizedApiResponse) {
        super(response.detail)
        this.name = response.title
        this.response = response
    }
}