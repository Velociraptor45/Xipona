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

############ PREPARE EXECUTION ############
FROM build AS prep-execution
WORKDIR /app/script
COPY Docker/generate-runtime-config.sh /docker-entrypoint.d/40-generate-runtime-config.sh
RUN chmod +x /docker-entrypoint.d/40-generate-runtime-config.sh

################## FINAL ##################
FROM nginxinc/nginx-unprivileged:1.31-alpine AS final
USER root
RUN apk add jq
USER 101
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY Docker/nginx.conf /etc/nginx/nginx.conf
COPY Docker/xipona.conf /etc/nginx/conf.d/xipona.conf
COPY --from=prep-execution /app/script .

CMD ["exec nginx -g 'daemon off;'"]