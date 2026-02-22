import { type JSX } from "react"

export function VisitIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`fill-neutral-0 stroke-neutral-900 ${className}`}>
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                  d="M17.5 7.5v-5m0 0h-5m5 0-6.667 6.667m-2.5-5H6.5c-1.4 0-2.1 0-2.635.272a2.5 2.5 0 0 0-1.093 1.093C2.5 6.066 2.5 6.767 2.5 8.167V13.5c0 1.4 0 2.1.272 2.635a2.5 2.5 0 0 0 1.093 1.092C4.4 17.5 5.1 17.5 6.5 17.5h5.333c1.4 0 2.1 0 2.635-.273a2.5 2.5 0 0 0 1.093-1.092c.272-.535.272-1.235.272-2.635v-1.833"/>
        </svg>
    )
}