import type { UnauthorizedApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"

export class UnauthorizedApiError extends ApiError {

    constructor(response: UnauthorizedApiResponse) {
        super(response)
    }
}