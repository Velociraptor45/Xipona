#!/bin/sh

cat /usr/local/share/ca-certificates/rootCA.crt >> /etc/ssl/certs/ca-certificates.crt
nginx -g "daemon off;"