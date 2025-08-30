#!/bin/bash
# This script was used to generate the self-signed wildcard certificate for
# development environments in the .docker/conf/ssl/ directory. There are
# some particular settings that took a bit of digging in order for it to
# work properly in current browsers, so I kept the script for reference here.
# See https://superuser.com/a/1466427

set -e

# Load environment variables from the .env file
source "$(dirname "$0")/../.env"

# Import functions for environment variable checks
source "$(dirname "$0")/Common/common-ssl-certs.sh"

{
    # Path where the SSL certificate and key will be stored
    declare SSL_TARGET_PATH="$(dirname "$0")/../SSL"
    
    # Hostnames for the API, client, and Keycloak
    declare SSL_DOMAINS=(
        "$HOSTNAME_API"
        "$HOSTNAME_CLIENT"
        "$KC_HOSTNAME"
    )
}

# Generate a self-signed wildcard SSL certificate
# for each domain in the SSL_DOMAINS array
for SSL_DOMAIN in "${SSL_DOMAINS[@]}"; do
    ssl_generate_self_signed "$SSL_DOMAIN" "$SSL_TARGET_PATH"
done

log_output "Setting read permissions for cert/key files to allow Docker container access." "$COLOR_NONE" "ssl_generate_self_signed"
chmod +r "$SSL_TARGET_PATH"/*.key "$SSL_TARGET_PATH"/*.crt

log_output "COMPLETE" "$COLOR_GREEN" "ssl_generate_self_signed"