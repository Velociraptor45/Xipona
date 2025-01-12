FROM scratch AS base
ARG APP_VERSION
WORKDIR /app

################## BUILD ##################
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet restore "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj" -a $TARGETARCH
COPY . .
RUN dotnet build "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj" -c Release -o /app/build -a $TARGETARCH

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj" -c Release -o /app/publish --no-restore --sc true -a $TARGETARCH

################## FINAL ##################
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV APP_VERSION=${APP_VERSION}
ENTRYPOINT ["dotnet", "Xipona.Api.WebApp.dll"]