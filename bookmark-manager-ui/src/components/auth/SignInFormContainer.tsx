import { type JSX, type SyntheticEvent, useState } from "react"
import { Logo } from "@/components/Logo"
import { Link } from "react-router"
import { useAuthContext } from "@/hooks/auth.hook"

export function SignInFormContainer(): JSX.Element {

    return (
        <div
            className="w-85.75 md:w-md mx-auto flex flex-col gap-y-8 px-5 md:px-8 py-8 md:py-10 rounded-12 bg-neutral-0">
            <Logo/>
            <FormHeader/>
            <FormFields/>
            <FormFooter/>
        </div>
    )
}

function FormHeader(): JSX.Element {
    return (
        <div className="flex flex-col gap-y-1.5">
            <p className="text-preset-1">Log in to your account</p>
            <p className="text-preset-4-md color-neutral-800">Welcome back! Please enter your details.</p>
        </div>
    )
}

function FormFields(): JSX.Element {
    const [email, setEmail] = useState<string>("dimitri.sifoua@gmail.com")
    const [password, setPassword] = useState<string>("Password123")

    const { login, error } = useAuthContext()

    function handleSubmit(event: SyntheticEvent<HTMLFormElement>): void {
        event.preventDefault()
        console.log("Form submitted with email:", email, "and password:", password)
        
        const formData = new FormData(event.currentTarget)
        login(formData.get("email") as string, formData.get("password") as string)
    }
    
    return (
        <form onSubmit={handleSubmit} className="flex flex-col gap-y-4">
            <div className="flex flex-col gap-y-1.5">
                <label htmlFor="email" className="text-preset-4 color-neutral-900">Email</label>
                <input type="email" id="email" name="email" autoComplete="off" required={true}
                       value={email} onChange={(event): void => setEmail(event.target.value)}
                       className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
            </div>
            <div className="flex flex-col gap-y-1.5">
                <label htmlFor="password" className="text-preset-4 color-neutral-900">Password</label>
                <input type="password" id="password" name="password" autoComplete="off" required={true}
                       value={password} onChange={(event): void => setPassword(event.target.value)}
                       className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
            </div>
            {
                error !== null &&
                <div className="flex flex-col gap-y-1.5">
                    <span className="text-red-800">{error.detail}</span>
                </div>
            }
            <button type="submit"
                    className="h-11.5 flex px-4 py-3 bg-teal-700 rounded-8 items-center justify-center cursor-pointer">
                <p className="text-preset-3 text-neutral-0">Log in</p>
            </button>
        </form>
    )
}

function FormFooter(): JSX.Element {

    return (
        <div className="flex flex-col gap-y-3 items-center justify-center">
            <p className="text-preset-4-md text-neutral-800">
                Forgot password? <a href="#" className="text-preset-4 text-neutral-900">Reset it</a>
            </p>
            <p className="text-preset-4-md text-neutral-800">
                Don't have an account? <Link to="../register" className="text-preset-4 text-neutral-900">Sign up</Link>
            </p>
        </div>
    )
}