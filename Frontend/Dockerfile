################## BUILD ##################
FROM mcr.microsoft.com/dotnet/sdk:6.0.302-bullseye-slim-arm32v7 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj"
RUN dotnet build "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/build

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "ShoppingList.Frontend.WebApp/ShoppingList.Frontend.WebApp.csproj" -c Release -o /app/publish

################## FINAL ##################
FROM arm32v6/nginx:1.22-alpine AS final
EXPOSE 443
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY Docker/nginx.conf /etc/nginx/nginx.conf
COPY Docker/entry-command.sh /entry-command.sh
RUN chmod +x /entry-command.sh

CMD ["/entry-command.sh"]