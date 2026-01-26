import { type RefObject, useEffect } from "react"
import type { Nullable } from "@/types"

export function useCloseModal<T extends HTMLElement>(ref: RefObject<Nullable<T>>, handler: (event: Event) => void): void {
    useEffect(() => {
        function listener(event: Event): void {
            if (!ref.current || ref.current.contains(event.target as Node)) {
                return
            }

            handler(event)
        }

        document.addEventListener("mousedown", listener)
        document.addEventListener("touchstart", listener)

        return () => {
            document.removeEventListener("mousedown", listener)
            document.removeEventListener("touchstart", listener)
        }
    }, [ref, handler]);
}