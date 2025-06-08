#!/bin/bash

add_to_json() {
    local var_name="$1"
    local var_value="${!var_name}"
    local needsApostrophes=$2
    if [ -n "$var_value" ]; then
        if [ needsApostrophes ]; then
            echo "  \"$var_name\": \"$var_value\"," >> /usr/share/nginx/html/wwwroot/appsettings.json
        else
            echo "  \"$var_name\": $var_value," >> /usr/share/nginx/html/wwwroot/appsettings.json
        fi
    fi
}

echo "{" > /usr/share/nginx/html/wwwroot/appsettings.json

add_to_json "XIPONA_API_URL" true
add_to_json "XIPONA_LOGS_ENABLED"  false
add_to_json "XIPONA_LOGS_HOST_URL"  true
add_to_json "XIPONA_AUTH_ENABLED"  true
add_to_json "XIPONA_AUTH_AUTHORITY"  true
add_to_json "XIPONA_AUTH_CLIENT_ID"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__0"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__1"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__2"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__3"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__4"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__5"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__6"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__7"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__8"  true
add_to_json "XIPONA_AUTH_DEFAULT_SCOPES__9"  true
add_to_json "XIPONA_AUTH_RESPONSE_TYPE"  true
add_to_json "XIPONA_AUTH_ROLE_NAME_USER"  true
add_to_json "XIPONA_AUTH_CLAIM_NAME"  true
add_to_json "XIPONA_AUTH_CLAIM_ROLE"  true
add_to_json "XIPONA_AUTH_CLAIM_SCOPE"  true

echo "}" >> /usr/share/nginx/html/wwwroot/appsettings.json