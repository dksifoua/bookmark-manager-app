import type { ErrorApiResponse } from "@/api/errors/schema"

export abstract class ApiError extends Error {
    readonly response: ErrorApiResponse

    protected constructor(response: ErrorApiResponse) {
        super(response.title)
        this.name = response.title
        this.response = response
    }
}