#!/bin/bash
set -e
# This script prepares the Keycloak realm export file for import by updating
# hostname references in the configuration to match the environment settings.
# It replaces hardcoded hostnames with values from the .env file to ensure
# proper configuration across different environments.

# Import common functions and variables for scripts
source "$(dirname "$0")/Common/common-functions.sh"

# Load environment variables from the .env file
source "$(dirname "$0")/../.env"

# Set global script variables
{
    declare KC_IMPORT_TEMPLATE="$(dirname "$0")/../Keycloak/import_template/realm-export.json"
    declare KC_IMPORT_FILE="$(dirname "$0")/../Keycloak/import/realm-export.json"
}

replace_hostname_from_env() {
    log_output "Updating hostname references in $KC_IMPORT_FILE to use hosts defined in .env file..." "$COLOR_BLUE"
    
    log_output "Copying keycloak template import file to import directory"
    cp "$KC_IMPORT_TEMPLATE" "$KC_IMPORT_FILE"

    log_output "Updating CLIENT host references to ${HOSTNAME_CLIENT}:${CLIENT_HTTPS_PORT}"    
    sed_replace "s|starwarsinfo\.test:8080|${HOSTNAME_CLIENT}:${CLIENT_HTTPS_PORT}|g" "$KC_IMPORT_FILE"
    
    log_output "Keycloak KEYCLOAK host references to ${KC_HOSTNAME}:${KC_HTTPS_PORT}"
    sed_replace "s|starwarsinfo\.test:8082|${KC_HOSTNAME}:${KC_HTTPS_PORT}|g" "$KC_IMPORT_FILE"
    
    log_output "Complete" "$COLOR_GREEN"
}

replace_hostname_from_env