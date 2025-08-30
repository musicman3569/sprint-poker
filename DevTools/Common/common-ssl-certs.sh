#!/bin/bash
# This script was used to generate the self-signed wildcard certificate for
# development environments. There are  some particular settings that took a 
# bit of digging in order for it to work properly in current browsers, so I 
# kept the script for reference here. See https://superuser.com/a/1466427
#
# This script can be run more than once and will not duplicate or overwrite
# the cert files if they already exist.

{
    # Get the path to the directory where this script is located so other 
    # scripts can be sourced from this script relative to it's own path.
    declare SCRIPT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
}

# Import common functions and variables for scripts
source "$SCRIPT_PATH/common-functions.sh"

ssl_generate_self_signed() {
    # print usage
    local DOMAIN="$1"
    local SSL_TARGET_PATH="$2"
    
    if [ -f "${SSL_TARGET_PATH}/${DOMAIN}.crt" ] && \
       [ -f "${SSL_TARGET_PATH}/${DOMAIN}.key" ]; then
        log_output "Certificate & key already exist for $DOMAIN, skipping." "$COLOR_GREEN"
        return 0
    fi
    
    # Add wildcard
    WILDCARD="*.$DOMAIN"
    
    # Set our variables
    cat <<EOF > req.cnf
[req]
distinguished_name = req_distinguished_name
x509_extensions = v3_req
prompt = no
[req_distinguished_name]
C = US
ST = UT
O = Star Wars Info
localityName = Salt Lake City
commonName = $WILDCARD
organizationalUnitName = Engineering
emailAddress = admin@$DOMAIN
[v3_req]
keyUsage = nonRepudiation, digitalSignature, keyEncipherment
extendedKeyUsage = serverAuth
subjectAltName = @alt_names
[alt_names]
DNS.1   = https.$DOMAIN
DNS.2   = *.https.$DOMAIN
EOF
    
    log_output "Generating self-signed certificate for $WILDCARD" "$COLOR_BLUE"
    
    # Generate our Private Key, and Certificate directly
    openssl req -x509 -nodes -days 3650 -newkey rsa:2048 \
      -keyout "${SSL_TARGET_PATH}/${DOMAIN}.key" -config req.cnf \
      -out "${SSL_TARGET_PATH}/${DOMAIN}.crt" -sha256
    rm req.cnf
    
    log_output "Generated key: ${SSL_TARGET_PATH}/${DOMAIN}.key" "$COLOR_GREEN"
    log_output "Generated cert: ${SSL_TARGET_PATH}/${DOMAIN}.crt" "$COLOR_GREEN"
}