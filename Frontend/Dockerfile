################## BUILD ##################
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet restore "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -a $TARGETARCH
RUN dotnet build "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -c Release -o /app/build -a $TARGETARCH

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -c Release -o /app/publish --no-restore -a $TARGETARCH

################## FINAL ##################
FROM nginxinc/nginx-unprivileged:1.31-alpine AS final

WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY Docker/nginx.conf /etc/nginx/nginx.conf
COPY Docker/xipona.conf /etc/nginx/conf.d/xipona.conf

USER root
RUN apk add jq
COPY Docker/generate-runtime-config.sh /docker-entrypoint.d/40-generate-runtime-config.sh
RUN chmod +x /docker-entrypoint.d/40-generate-runtime-config.sh
RUN touch ./wwwroot/runtime-config.json \
    && chown 101:101 ./wwwroot/runtime-config.json \
    && chmod 664 ./wwwroot/runtime-config.json
USER 101

CMD ["nginx", "-g", "daemon off;"]
