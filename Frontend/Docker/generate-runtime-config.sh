#!/bin/sh
set -eu

jq -n \
  --argjson authEnabled "${XIPONA_AUTH_ENABLED:-false}" \
  --arg authAuthority "${XIPONA_AUTH_AUTHORITY:-}" \
  --arg authClientId "${XIPONA_AUTH_CLIENT_ID:-}" \
  --argjson authDefaultScopes "${XIPONA_AUTH_DEFAULT_SCOPES:-[]}" \
  --arg authResponseType "${XIPONA_AUTH_RESPONSE_TYPE:-code}" \
  --arg authRoleNameUser "${XIPONA_AUTH_ROLE_NAME_USER:-User}" \
  --arg authClaimName "${XIPONA_AUTH_CLAIM_NAME:-given_name}" \
  --arg authClaimRole "${XIPONA_AUTH_CLAIM_ROLE:-role}" \
  --arg authClaimScope "${XIPONA_AUTH_CLAIM_SCOPE:-scope}" \
  --argjson logsEnabled "${XIPONA_LOGS_ENABLED:-false}" \
  --arg logsHostUrl "${XIPONA_LOGS_HOST_URL:-}" \
  --arg apiUrl "${XIPONA_API_URL:-}" \
  '{
    authConfig: {
      XIPONA_AUTH_ENABLED: $authEnabled,
      XIPONA_AUTH_AUTHORITY: $authAuthority,
      XIPONA_AUTH_CLIENT_ID: $authClientId,
      XIPONA_AUTH_DEFAULT_SCOPES: $authDefaultScopes,
      XIPONA_AUTH_RESPONSE_TYPE: $authResponseType,
      XIPONA_AUTH_ROLE_NAME_USER: $authRoleNameUser,
      XIPONA_AUTH_CLAIM_NAME: $authClaimName,
      XIPONA_AUTH_CLAIM_ROLE: $authClaimRole,
      XIPONA_AUTH_CLAIM_SCOPE: $authClaimScope
    },
    logConfig: {
      XIPONA_LOGS_ENABLED: $logsEnabled,
      XIPONA_LOGS_HOST_URL: $logsHostUrl
    },
    connectionConfig: {
      XIPONA_API_URL: $apiUrl
    }
  }' > /usr/share/nginx/html/wwwroot/runtime-config.json
