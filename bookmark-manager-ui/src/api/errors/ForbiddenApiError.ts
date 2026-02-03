import type { ForbiddenApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"

export class ForbiddenApiError extends ApiError {

    constructor(response: ForbiddenApiResponse) {
        super(response)
    }
}