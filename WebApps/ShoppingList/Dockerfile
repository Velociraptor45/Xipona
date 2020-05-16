FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

ENV BUILD_DIR=/app/ShoppingList

COPY . ${BUILD_DIR}/
WORKDIR ${BUILD_DIR}
RUN ["dotnet", "restore"]
 
RUN ["dotnet", "build"]
RUN ["dotnet", "publish", "-c", "Release", "-o", "/app/ShoppingList/release"]

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV ASPNETCORE_URLS=http://*:14050

WORKDIR ${BUILD_DIR}/release
COPY --from=build-env app/ShoppingList/release .
ENTRYPOINT ["dotnet", "ShoppingList.dll"]