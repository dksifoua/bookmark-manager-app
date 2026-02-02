const apiUrl = import.meta.env.VITE_BOOKMARK_MANAGER_API_URL

export async function visitBookmark({ bookmarkId, visitTime }: {
    bookmarkId: number,
    visitTime: Date
}): Promise<void> {
    if (!apiUrl) throw new Error("BOOKMARK_MANAGER_API_URL environment variable is not set")

    const response = await fetch(`${apiUrl}/visits`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ bookmarkId, visitTime }),
        credentials: "include"
    })
    if (response.status !== 201) {
        throw new Error(`Unexpected status code: ${response.status}`)
    }
}