import { type JSX, useState } from "react"
import { Logo } from "@/components/Logo"
import HomeIcon from "@/assets/images/icon-home.svg"
import ArchiveIcon from "@/assets/images/icon-archive.svg"

export function Sidebar(): JSX.Element {

    return (
        <div className="xl:w-74 min-h-screen flex flex-col bg-neutral-0 border border-neutral-300">
            <div className="xl:h-19.5 flex items-center justify-center pt-5 pb-2.5">
                <Logo/>
            </div>
            <Navigation/>
            <div className="flex flex-col px-4">
                <p className="px-3 pb-4 text-preset-5">TAGS</p>
                <Tag name="Docker" value={10}/>
                <Tag name="Jenkins" value={10}/>
                <Tag name="Kubernetes" value={10}/>
                <Tag name="AWS" value={10}/>
                <Tag name="Git" value={10}/>
                <Tag name="Terraform" value={10}/>
                <Tag name="Ansible" value={10}/>
                <Tag name="ArgoCD" value={10}/>
            </div>
        </div>
    )
}

function Navigation(): JSX.Element {
    const [selected, setSelected] = useState<"home" | "archived">("home")

    return (
        <div className="flex flex-col gap-y-4 px-4 py-5">
            <div className="flex flex-col gap-y-2">
                <button className={`flex flex-row gap-x-3 items-center justify-start px-3 py-2 border ${
                    selected === "home"
                        ? "rounded-6 bg-neutral-100 border-neutral-100"
                        : "hover:rounded-6 border-neutral-0 hover:bg-neutral-100 hover:border-neutral-100 cursor-pointer"
                }`} onClick={() => selected === "archived" && setSelected("home")}>
                    <img src={HomeIcon} alt="Home Icon" className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-900">Home</p>
                </button>
                <button className={`flex flex-row gap-x-3 items-center justify-start px-3 py-2 border ${
                    selected === "archived"
                        ? "rounded-6 bg-neutral-100 border-neutral-100"
                        : "hover:rounded-6 border-neutral-0 hover:bg-neutral-100 hover:border-neutral-100 cursor-pointer"
                }`} onClick={() => selected === "home" && setSelected("archived")}>
                    <img src={ArchiveIcon} alt="Archive Icon" className="w-5 h-5"/>
                    <p className="text-preset-3 text-neutral-800">Archived</p>
                </button>
            </div>
        </div>
    )
}

function Tag({ name, value }: { name: string, value: number }): JSX.Element {

    return (
        <div className="h-10.5 flex flex-row gap-x-3 px-3 py-2 items-center justify-between">
            <div className="flex flex-row gap-x-2 items-center justify-start">
                <input type="checkbox"
                       className={`size-4 border border-neutral-500 cursor-pointer`}
                />
                <span className="text-preset-3 text-neutral-800">{name}</span>
            </div>
            <div
                className="w-6 h-6 flex px-2 py-0.5 items-center justify-center rounded-full bg-neutral-100 border border-neutral-300 size-4">
                <span className="text-preset-5 text-neutral-800">{value}</span>
            </div>
        </div>
    )
}