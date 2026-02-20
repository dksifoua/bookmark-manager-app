import { type JSX } from "react"

export function CloseIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`${className} fill-neutral-0 stroke-neutral-900`}>
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.6"
                  d="M15 5 5 15M5 5l10 10"/>
        </svg>
    )
}