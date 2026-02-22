import { type JSX } from "react"

export function UnarchiveIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`fill-neutral-0 stroke-neutral-900 ${className}`}>
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                  d="M1.667 8.333S3.337 6.057 4.695 4.7a7.5 7.5 0 1 1-1.902 7.385m-1.126-3.75v-5m0 5h5"/>
        </svg>
    )
}