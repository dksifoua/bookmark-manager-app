import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import "@/index.css"
import { App } from "@/App.tsx"
import { BrowserRouter } from "react-router"
import { AuthProvider } from "@/providers/AuthProvider"

const basename = import.meta.env.BASE_URL

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <BrowserRouter basename={
            basename.endsWith('/') 
                ? basename.slice(0, -1) 
                : basename
        }>
            <AuthProvider>
                <App/>
            </AuthProvider>
        </BrowserRouter>
    </StrictMode>,
)
