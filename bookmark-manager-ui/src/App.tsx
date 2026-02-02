import { Route, Routes } from "react-router";
import { AuthLayout } from "@/components/auth/AuthLayout"
import { SignInFormContainer } from "@/components/auth/SignInFormContainer"
import { HomeLayout } from "@/components/home/HomeLayout"
import { ProtectedRoute } from "@/components/ProtectedRoute"
import { SignUpFormContainer } from "@/components/auth/SignUpFormContainer"
import { BookmarkList } from "@/components/bookmarks/BookmarkList"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryClient = new QueryClient()

export function App() {

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
