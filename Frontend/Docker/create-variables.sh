#!/bin/sh

OUTPUT=/usr/share/nginx/html/wwwroot/variables.json

add_to_json() {
    var_name=$1
    value_type=$2
    var_value=$(printenv "$var_name") || var_value=

    [ -n "$var_value" ] || return 0

    if [ "$value_type" = string ]; then
        # Escape backslashes and double quotes for JSON string values.
        escaped_value=$(printf '%s' "$var_value" | sed 's/\\/\\\\/g; s/"/\\"/g')
        json_value="\"$escaped_value\""
    else
        case "$var_value" in
            true|false|null|[-+]?[0-9]* ) json_value=$var_value ;;
            *) printf 'Invalid JSON value for %s: %s\n' "$var_name" "$var_value" >&2; return 1 ;;
        esac
    fi

    if [ "$first" -eq 0 ]; then
        printf ',\n' >> "$OUTPUT"
    fi
    printf '  "%s": %s' "$var_name" "$json_value" >> "$OUTPUT"
    first=0
}

: > "$OUTPUT"
printf '{\n' >> "$OUTPUT"
first=1

add_to_json XIPONA_API_URL string
add_to_json XIPONA_LOGS_ENABLED raw
add_to_json XIPONA_LOGS_HOST_URL string
add_to_json XIPONA_AUTH_ENABLED raw
add_to_json XIPONA_AUTH_AUTHORITY string
add_to_json XIPONA_AUTH_CLIENT_ID string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__0 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__1 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__2 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__3 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__4 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__5 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__6 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__7 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__8 string
add_to_json XIPONA_AUTH_DEFAULT_SCOPES__9 string
add_to_json XIPONA_AUTH_RESPONSE_TYPE string
add_to_json XIPONA_AUTH_ROLE_NAME_USER string
add_to_json XIPONA_AUTH_CLAIM_NAME string
add_to_json XIPONA_AUTH_CLAIM_ROLE string
add_to_json XIPONA_AUTH_CLAIM_SCOPE string

printf '\n}\n' >> "$OUTPUT"