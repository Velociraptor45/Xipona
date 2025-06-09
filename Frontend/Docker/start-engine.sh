#!/bin/bash

DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
"$DIR/create-variables.sh"

nginx -g 'daemon off;'