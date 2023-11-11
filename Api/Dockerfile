FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShoppingList.Api.WebApp/ShoppingList.Api.WebApp.csproj"
COPY . .
RUN dotnet build "ShoppingList.Api.WebApp/ShoppingList.Api.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingList.Api.WebApp/ShoppingList.Api.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingList.Api.WebApp.dll"]