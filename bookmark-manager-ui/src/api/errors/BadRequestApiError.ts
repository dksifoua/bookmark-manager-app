import type { BadRequestApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"

export class BadRequestApiError extends ApiError {

    constructor(response: BadRequestApiResponse) {
        super(response)
    }
}