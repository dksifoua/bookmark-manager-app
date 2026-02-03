import type { ConflictApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"

export class ConflictApiError extends ApiError {

    constructor(response: ConflictApiResponse) {
        super(response)
    }
}