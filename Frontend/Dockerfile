################## BUILD ##################
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet restore "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -a $TARGETARCH
RUN dotnet build "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -c Release -o /app/build -a $TARGETARCH

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -c Release -o /app/publish --no-restore -a $TARGETARCH

################## FINAL ##################
FROM nginxinc/nginx-unprivileged:1.25 AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY Docker/nginx.conf /etc/nginx/nginx.conf

CMD ["nginx", "-g", "daemon off;"]