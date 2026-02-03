import type { JSX, SyntheticEvent } from "react"
import { Logo } from "@/components/Logo"
import { Link } from "react-router"
import { useAuthContext } from "@/hooks/auth.hook"
import LoadingIcon from "@/assets/images/icon-loading.svg"

export function SignUpFormContainer(): JSX.Element {

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
            <p className="text-preset-1">Create your account</p>
            <p className="text-preset-4-md color-neutral-800">Join us and start saving your favorite links — organized,
                searchable, and always within reach.</p>
        </div>
    )
}

function FormFields(): JSX.Element {
    const { register, isLoading, error: { register: error } } = useAuthContext()

    function handleSubmit(event: SyntheticEvent<HTMLFormElement>): void {
        event.preventDefault()

        const formData = new FormData(event.currentTarget)
        register(formData.get("fullname") as string, formData.get("email") as string, formData.get("password") as string)
    }
    
    return (
        <form onSubmit={handleSubmit} className="flex flex-col gap-y-4">
            <div className="flex flex-col gap-y-1.5">
                <label htmlFor="fullname" className="text-preset-4 color-neutral-900">Full Name</label>
                <input type="text" id="fullname" name="fullname" autoComplete="off" required={false}
                       className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                {
                    error !== null && "errors" in error && "Fullname" in error.errors &&
                    <div className="flex flex-col gap-y-1.5">
                        {
                            error.errors["Fullname"].map((error: string, index: number) => (
                                <span key={index} className="text-preset-4 text-red-800">{error}</span>
                            ))
                        }
                    </div>
                }
            </div>
            <div className="flex flex-col gap-y-1.5">
                <label htmlFor="email" className="text-preset-4 color-neutral-900">Email</label>
                <input type="email" id="email" name="email" autoComplete="off" required={false}
                       className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                {
                    error !== null && "errors" in error && "Email" in error.errors &&
                    <div className="flex flex-col gap-y-1.5">
                        {
                            error.errors["Email"].map((error: string, index: number) => (
                                <span key={index} className="text-preset-4 text-red-800">{error}</span>
                            ))
                        }
                    </div>
                }
            </div>
            <div className="flex flex-col gap-y-1.5">
                <label htmlFor="password" className="text-preset-4 color-neutral-900">Password</label>
                <input type="password" id="password" name="password" autoComplete="off" required={false}
                       className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                {
                    error !== null && "errors" in error && "Password" in error.errors &&
                    <div className="flex flex-col gap-y-1.5">
                        {
                            error.errors["Password"].map((error: string, index: number) => (
                                <span key={index} className="text-preset-4 text-red-800">{error}</span>
                            ))
                        }
                    </div>
                }
            </div>
            {
                error !== null && "detail" in error &&
                <div className="flex flex-col gap-y-1.5">
                    <span className="text-preset-4 text-red-800">{error.detail}</span>
                </div>
            }
            <button type="submit"
                    className="h-11.5 flex px-4 py-3 flex flex-row gap-x-2 bg-teal-700 rounded-8 items-center justify-center cursor-pointer">
                {
                    isLoading && <img src={LoadingIcon} alt="Loading Icon" className="w-4 h-4 spin-slow"/>
                }
                <p className="text-preset-3 text-neutral-0">Create account</p>
            </button>
        </form>
    )
}

function FormFooter(): JSX.Element {

    return (
        <div className="flex flex-col gap-y-3 items-center justify-center">
            <p className="text-preset-4-md text-neutral-800">
                Already have an account? <Link to="../login" className="text-preset-4 text-neutral-900">Log in</Link>
            </p>
        </div>
    )
}