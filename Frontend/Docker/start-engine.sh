#!/bin/bash

DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
"$DIR/create-appsettings.sh"

nginx -g 'daemon off;'