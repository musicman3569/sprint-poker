#!/bin/bash
set -e

# Import common functions and variables for scripts
source "$(dirname "$0")/Common/common-functions.sh"

# Import functions for environment variable checks
source "$(dirname "$0")/Common/common-env-files.sh"

# Initialize global script variables.
{    
    declare ENV_FILE=".env"
    declare ENV_TEMPLATE_FILE="${1:-.env.template}"
    declare ENV_TMP_FILE=".env.tmp"
}

# Check if the environment file exists. If not, create it from the template
env_run_all_checks "$ENV_FILE" "$ENV_TEMPLATE_FILE" "$ENV_TMP_FILE"
