#!/bin/sh

DIR=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
"$DIR/create-variables.sh"

exec nginx -g 'daemon off;'