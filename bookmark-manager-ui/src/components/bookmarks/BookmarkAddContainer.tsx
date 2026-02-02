import type { JSX } from "react";
import CloseIcon from "@/assets/images/icon-close.svg"

export function BookmarkAddContainer({ closeModal }: { closeModal: () => void }): JSX.Element {

    return (
        <div className="w-[570px] h-[671px] flex flex-col gap-y-8 p-8 rounded-16 bg-neutral-0 relative">
            <button onClick={closeModal} className="w-8 h-8 flex absolute top-2.5 right-2.5 rounded-8 border border-neutral-400 cursor-pointer items-center justify-center">
                <img src={CloseIcon} alt="Close Icon" className="w-5 h-5"/>
            </button>
            <div className="flex flex-col gap-y-2">
                <p className="text-preset-1 text-neutral-900">Add a bookmark</p>
                <p className="text-preset-4-md text-neutral-800">
                    Save a link with details to keep your collection organized. We extract the favicon automatically from
                    the URL.
                </p>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="title" className="text-preset-4 color-neutral-900">Title *</label>
                    <input type="text" id="title" name="title" autoComplete="off" required={true}
                           className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                </div>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="description" className="text-preset-4 color-neutral-900">Description *</label>
                    <textarea id="description" name="description" autoComplete="off" required={true}
                           className="h-[91px] p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                </div>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="url" className="text-preset-4 color-neutral-900">Website URL *</label>
                    <input type="text" id="url" name="url" autoComplete="off" required={true}
                           className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                </div>
            </div>
            <div className="flex flex-col gap-y-5">
                <div className="flex flex-col gap-y-1.5">
                    <label htmlFor="tags" className="text-preset-4 color-neutral-900">Tags *</label>
                    <input type="text" id="tags" name="tags" autoComplete="off" required={true}
                           className="h-11.25 p-3 bg-neutral-0 border border-neutral-500 rounded-8"/>
                </div>
            </div>
            <div className="flex flex-row gap-x-4 items-center justify-end">
                <button
                        className="h-11.5 flex px-4 py-3 bg-neutral-0 rounded-8 border border-neutral-400 items-center justify-center cursor-pointer">
                    <p className="text-preset-3 text-neutral-900">Cancel</p>
                </button>
                <button
                    className="h-11.5 flex px-4 py-3 bg-teal-700 rounded-8 items-center justify-center cursor-pointer">
                    <p className="text-preset-3 text-neutral-0">Add Bookmark</p>
                </button>
            </div>
        </div>
    )
}