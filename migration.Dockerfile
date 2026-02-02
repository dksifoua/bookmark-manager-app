FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
COPY ["bookmark-manager-app.csproj", "./"]
RUN dotnet restore "bookmark-manager-app.csproj"
COPY . .
RUN dotnet build "bookmark-manager-app.csproj" -c $BUILD_CONFIGURATION
RUN dotnet ef migrations bundle -o /app/efbundle --project bookmark-manager-app.csproj --self-contained

FROM base AS final
WORKDIR /app
COPY --from=build /app/efbundle .
COPY appsettings.json . 
ENTRYPOINT ["./efbundle"]
