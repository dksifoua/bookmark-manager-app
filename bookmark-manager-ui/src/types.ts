export type Nullable<T> = T | null
export type Result<T> =
    | { success: true, data: T }
    | { success: false, error: Error }