#!/bin/bash

add_to_json() {
    local var_name="$1"
    local var_value="${!var_name}"
    if [ -n "$var_value" ]; then
        echo "  \"$var_name\": \"$var_value\"," >> /usr/share/nginx/html/appsettings.json
    fi
}

echo "{" > /usr/share/nginx/html/appsettings.json

add_to_json "XIPONA_API_URL"
add_to_json "XIPONA_LOGS_ENABLED"
add_to_json "XIPONA_LOGS_HOST_URL"
add_to_json "XIPONA_AUTH_ENABLED"
add_to_json "XIPONA_AUTH_AUTHORITY"
add_to_json "XIPONA_AUTH_CLIENT_ID"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__0"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__1"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__2"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__3"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__4"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__5"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__6"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__7"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__8"
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__9"
add_to_json "XIPONA_AUTH_RESPONSE_TYPE"
add_to_json "XIPONA_AUTH_ROLE_NAME_USER"
add_to_json "XIPONA_AUTH_CLAIM_NAME"
add_to_json "XIPONA_AUTH_CLAIM_ROLE"
add_to_json "XIPONA_AUTH_CLAIM_SCOPE"

echo "}" >> /usr/share/nginx/html/appsettings.json