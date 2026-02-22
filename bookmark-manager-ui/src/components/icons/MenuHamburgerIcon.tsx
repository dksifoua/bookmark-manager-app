import { type JSX } from "react"

export function MenuHamburgerIcon({ className }: { className?: string }): JSX.Element {

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`fill-neutral-0 stroke-neutral-900 ${className}`}>
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                  d="M2.5 10h15m-15-5h15m-15 10h15"/>
        </svg>
    )
}