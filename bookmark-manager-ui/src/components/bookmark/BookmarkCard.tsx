import type { JSX } from "react"
import MenuBookmarkIcon from "@/assets/images/icon-menu-bookmark.svg"
import PinIcon from "@/assets/images/icon-pin.svg"
import VisitCountIcon from "@/assets/images/icon-visit-count.svg"
import LastVisitedIcon from "@/assets/images/icon-last-visited.svg"
import CreatedIcon from "@/assets/images/icon-created.svg"
import type { BookmarkResponse } from "@/api/bookmarks/schema"
import type { Nullable } from "@/types"

const formatter = new Intl.DateTimeFormat("en-US", { day: "2-digit", month: "short" })

export function BookmarkCard({ bookmark }: { bookmark: BookmarkResponse }): JSX.Element {

    return (
        <div className="w-85.75 md:w-84 xl:w-84.5 h-68 flex flex-col rounded-12 bg-neutral-0">
            <div className="w-full h-57.75 flex flex-col gap-y-4 p-4">
                <BookmarkCardHeader title={bookmark.title} url={bookmark.url}/>
                <div className="h-px bg-neutral-300"></div>
                <p className="text-preset-4-md text-neutral-800">
                    {bookmark.description}
                </p>
                <BookmarkCardTags tags={bookmark.tags}/>
            </div>
            <BookmarkCardFooter visitCount={bookmark.visitCount} creationTime={bookmark.creationTime}
                                lastVisitTime={bookmark.lastVisitTime}/>
        </div>
    )
}

function BookmarkCardHeader({ title, url }: { title: string, url: string }): JSX.Element {
    const urlObject = new URL(url)
    const formattedUrl = urlObject.pathname === "/"
        ? `${urlObject.host}${urlObject.search}`
        : `${urlObject.host}${urlObject.pathname}${urlObject.search}`

    return (
        <div className="w-full flex flex-row gap-x-3 justify-between">
            <div className="flex flex-row gap-x-3">
                <img src={`https://www.faviconextractor.com/favicon/${urlObject.host}`} alt={`${title} Icon`}
                     className="w-11 h-11 rounded-8 border border-neutral-100"/>
                <div className="flex flex-col gap-y-1">
                    <p className="text-preset-2 text-neutral-900">{title}</p>
                    <p className="text-preset-5 text-neutral-800">{formattedUrl}</p>
                </div>
            </div>
            <div className="w-8 h-8 flex border border-neutral-500 rounded-8 items-center justify-center">
                <img src={MenuBookmarkIcon} alt="Menu Bookmark Icon"/>
            </div>
        </div>
    )
}

function BookmarkCardTags({ tags }: { tags: string[] }): JSX.Element {

    return (
        <div className="flex flex-row gap-x-2">
            {
                tags.map((tag: string, index: number): JSX.Element => (
                    <div key={index} className="flex px-2 py-0.5 rounded-4 bg-neutral-100">
                        <p className="text-preset-5">{tag}</p>
                    </div>
                ))
            }
        </div>
    )
}

function BookmarkCardFooter({ visitCount, creationTime, lastVisitTime }: {
    visitCount: number,
    creationTime: Date,
    lastVisitTime: Nullable<Date>
}): JSX.Element {
    const creationTimeParts = formatter.formatToParts(creationTime)
    const creationTimeDay = creationTimeParts.find(part => part.type === "day")?.value || ""
    const creationTimeMonth = creationTimeParts.find(part => part.type === "month")?.value || ""

    let lastVisitTimeDay = ""
    let lastVisitTimeMonth = ""
    if (lastVisitTime) {
        const lastVisitTimeParts = formatter.formatToParts(lastVisitTime)
        lastVisitTimeDay = lastVisitTimeParts.find(part => part.type === "day")?.value || ""
        lastVisitTimeMonth = lastVisitTimeParts.find(part => part.type === "month")?.value || ""
    }

    return (
        <div className="h-10.25 flex flex-row gap-x-8 px-4 py-3 border border-neutral-300 items-center justify-between">
            <div className="flex flex-row gap-x-4 items-center">
                <div className="flex flex-row gap-x-1.5 items-center justify-between">
                    <img src={VisitCountIcon} alt="Visit Count Icon" className="w-3 h-3"/>
                    <p className="text-preset-5">{visitCount}</p>
                </div>
                <div className="flex flex-row gap-x-1.5 items-center justify-between">
                    <img src={LastVisitedIcon} alt="Last Visited Icon" className="w-3 h-3"/>
                    <p className="text-preset-5">
                        {
                            lastVisitTime === null
                                ? "Never"
                                : `${lastVisitTimeDay} ${lastVisitTimeMonth}`
                        }
                    </p>
                </div>
                <div className="flex flex-row gap-x-1.5 items-center justify-between">
                    <img src={CreatedIcon} alt="Created Icon" className="w-3 h-3"/>
                    <p className="text-preset-5">{`${creationTimeDay} ${creationTimeMonth}`}</p>
                </div>
            </div>
            <div className="flex w-4 h-4 items-center justify-center">
                <img src={PinIcon} alt="Pin Icon"/>
            </div>
        </div>
    )
}