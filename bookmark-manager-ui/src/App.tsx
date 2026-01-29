import { SignInFormContainer } from "@/components/auth/SignInFormContainer"
import type { JSX } from "react"
import { Route, Routes } from "react-router"
import { AuthLayout } from "@/components/auth/AuthLayout"
import { SignUpFormContainer } from "@/components/auth/SignUpFormContainer"
import { HomeLayout } from "@/components/home/HomeLayout"
import { ProtectedRoute } from "@/components/ProtectedRoute"
import { BookmarkList } from "@/components/bookmark/BookmarkList"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"

const queryClient = new QueryClient()

export function App(): JSX.Element {

    return (
        <>
            <QueryClientProvider client={queryClient}>
                <Routes>
                    <Route element={<ProtectedRoute/>}>
                        <Route element={<HomeLayout/>}>
                            <Route index element={<BookmarkList/>}/>
                        </Route>
                    </Route>

                    <Route element={<AuthLayout/>}>
                        <Route path="login" element={<SignInFormContainer/>}/>
                        <Route path="register" element={<SignUpFormContainer/>}/>
                    </Route>
                </Routes>
            </QueryClientProvider>
        </>
    )
}