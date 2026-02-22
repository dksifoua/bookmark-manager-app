import { type JSX } from "react"

export function LastVisitedIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`fill-neutral-0 stroke-neutral-900 ${className}`}>
            <g clipPath="url(#a)">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                      d="M10 5v5l3.334 1.667m5-1.667a8.333 8.333 0 1 1-16.667 0 8.333 8.333 0 0 1 16.667 0"/>
            </g>
            <defs>
                <clipPath id="a">
                    <path fill="#fff" d="M0 0h20v20H0z"/>
                </clipPath>
            </defs>
        </svg>
    )
}