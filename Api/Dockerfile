FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj"
COPY . .
RUN dotnet build "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xipona.Api.WebApp.dll"]