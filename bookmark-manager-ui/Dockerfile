FROM oven/bun:1 AS builder

ARG VITE_BOOKMARK_MANAGER_API_URL
ENV VITE_BOOKMARK_MANAGER_API_URL=$VITE_BOOKMARK_MANAGER_API_URL

WORKDIR /app

COPY package.json bun.lock ./
RUN bun install --frozen-lockfile
COPY . .
RUN bun run build

FROM nginx:alpine AS runner

COPY --from=builder /app/dist /usr/share/nginx/html/bookmark-manager-app

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
