import { type JSX } from "react"

export function CheckIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"
             className={`fill-neutral-0 stroke-neutral-900 ${className}`}>
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.6"
                  d="M16.666 5 7.5 14.167 3.333 10"/>
        </svg>
    )
}