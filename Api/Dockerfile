################## CHISEL ################
FROM golang:latest AS chisel
RUN mkdir /extract
RUN mkdir /extract/tmp
RUN go install github.com/canonical/chisel/cmd/chisel@latest
RUN chisel cut --release ubuntu-22.04 --root /extract/ ca-certificates_data libstdc++6_libs libssl3_libs

################## BASE ###################
FROM scratch AS base
ARG APP_VERSION
COPY --from=chisel /extract /

################## PUBLISH#################
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS publish
ARG TARGETARCH
WORKDIR /src
COPY . .
RUN dotnet publish "Xipona.Api.WebApp/Xipona.Api.WebApp.csproj" -c Release -o /app/publish --sc true -a $TARGETARCH
RUN chmod +x ./Xipona.Api.WebApp

################## FINAL ##################
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV APP_VERSION=${APP_VERSION}
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT ["./Xipona.Api.WebApp"]