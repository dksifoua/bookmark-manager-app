import type { NotFoundApiResponse } from "@/api/errors/schema"
import { ApiError } from "@/api/errors/ApiError"

export class NotFoundApiError extends ApiError {

    constructor(response: NotFoundApiResponse) {
        super(response)
    }
}