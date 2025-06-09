################## BUILD ##################
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet restore "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -a $TARGETARCH
RUN dotnet build "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -c Release -o /app/build -a $TARGETARCH

################# PUBLISH #################
FROM build AS publish
RUN dotnet publish "Xipona.Frontend.WebApp/Xipona.Frontend.WebApp.csproj" -c Release -o /app/publish --no-restore -a $TARGETARCH
RUN chmod 666 /app/publish/wwwroot/variables.json

############ PREPARE EXECUTION ############
FROM build AS prep-execution
WORKDIR /app/script
COPY Docker/create-variables.sh ./create-variables.sh
COPY Docker/start-engine.sh ./start-engine.sh
RUN chmod +x start-engine.sh create-variables.sh

################## FINAL ##################
FROM nginxinc/nginx-unprivileged:1.25 AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish .
COPY Docker/nginx.conf /etc/nginx/nginx.conf
COPY Docker/xipona.conf /etc/nginx/conf.d/xipona.conf
COPY --from=prep-execution /app/script .

CMD ["/usr/share/nginx/html/start-engine.sh"]