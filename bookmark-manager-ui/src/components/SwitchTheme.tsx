import { type JSX, useState } from "react"

export function SwitchTheme(): JSX.Element {
    const [theme, setTheme] = useState<"light" | "dark">("light")

    function toggleTheme(): void {
        const newTheme = theme === "light" ? "dark" : "light"
        setTheme(newTheme)
        document.documentElement.classList.toggle("dark", newTheme === "dark")
    }

    return (
        <>
            <button onClick={toggleTheme} className="rounded-full p-2 cursor-pointer">{theme} mode</button>
        </>
    )
}