################## BUILD ##################
FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim-arm32v7 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj"
RUN dotnet build "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/build

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/publish

################## FINAL ##################
FROM arm32v6/nginx:1.18-alpine AS final
EXPOSE 443
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY ssl/ /etc/nginx/ssl/
COPY shoppinglist.conf /etc/nginx/conf.d/
COPY nginx.conf /etc/nginx/nginx.conf

# SSL
COPY ssl/rootCA.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates