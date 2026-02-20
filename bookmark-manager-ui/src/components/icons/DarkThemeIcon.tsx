import { type JSX } from "react"

export function DarkThemeIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`${className} fill-neutral-0 stroke-neutral-900`}>
            <g clip-path="url(#a)">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.667"
                      d="M18.296 10.797a6.667 6.667 0 1 1-9.092-9.093 8.334 8.334 0 1 0 9.092 9.093"/>
            </g>
            <defs>
                <clipPath id="a">
                    <path fill="#fff" d="M0 0h20v20H0z"/>
                </clipPath>
            </defs>
        </svg>
    )
}