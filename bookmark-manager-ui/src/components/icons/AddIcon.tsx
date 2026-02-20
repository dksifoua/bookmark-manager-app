import { type JSX } from "react"

export function AddIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`fill-neutral-0 stroke-neutral-0 dark:stroke-neutral-d-0 ${className}`}>
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.6"
                  d="M10 4.167v11.666M4.167 10h11.667"/>
        </svg>
    )
}