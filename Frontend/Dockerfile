FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-arm32v7 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj"
COPY . .
RUN dotnet build "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/publish

FROM arm32v6/nginx:1.18-alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY ssl/ /etc/nginx/ssl/
COPY shoppinglist.conf /etc/nginx/conf.d/
COPY nginx.conf /etc/nginx/nginx.conf