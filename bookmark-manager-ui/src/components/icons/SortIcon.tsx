import { type JSX } from "react"

export function SortIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"
             className={`fill-neutral-0 stroke-neutral-900 ${className}`}>
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                  d="M14.167 3.333v13.334m0 0-3.334-3.334m3.334 3.334 3.333-3.334M5.833 16.667V3.333m0 0L2.5 6.667m3.333-3.334 3.334 3.334"/>
        </svg>
    )
}