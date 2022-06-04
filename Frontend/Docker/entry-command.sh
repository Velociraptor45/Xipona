#!/bin/sh

cat /usr/local/share/ca-certificates/*.crt >> /etc/ssl/certs/ca-certificates.crt
nginx -g "daemon off;"