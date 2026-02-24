FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookmarkManagerApp/BookmarkManagerApp.csproj", "BookmarkManagerApp/"]
RUN dotnet restore "BookmarkManagerApp/BookmarkManagerApp.csproj"
COPY . .
WORKDIR "/src/BookmarkManagerApp"
RUN dotnet build "./BookmarkManagerApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookmarkManagerApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookmarkManagerApp.dll"]
