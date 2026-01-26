import { SignInFormContainer } from "@/components/auth/SignInFormContainer"
import type { JSX } from "react"
import { Route, Routes } from "react-router"
import { AuthLayout } from "@/components/auth/AuthLayout"
import { SignUpFormContainer } from "@/components/auth/SignUpFormContainer"
import { HomeLayout } from "@/components/home/HomeLayout"

export function App(): JSX.Element {

    return (
        <>
            <Routes>
                <Route path="/bookmark-manager-app">
                    <Route index element={<HomeLayout/>}/>
                    
                    <Route element={<AuthLayout/>}>
                        <Route path="login" element={<SignInFormContainer/>}/>
                        <Route path="register" element={<SignUpFormContainer/>}/>
                    </Route>
                </Route>
            </Routes>
        </>
    )
}