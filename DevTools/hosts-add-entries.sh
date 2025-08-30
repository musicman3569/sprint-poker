#!/bin/bash
# This script should be run once during initial setup to add custom host entries for the
# local dev retain URLs. This allows the docker host computer to access the retain
# services by their correct hostnames, which is requires since retain routes to different
# services based on the URL of the request.
#
# This script can be run more than once and will not duplicate the entries if they already exist.

set -e

# Import common functions and variables for scripts
source "$(dirname "$0")/Common/common-functions.sh"

# Import common functions for adding host entries
source "$(dirname "$0")/Common/common-hosts-entries.sh"

# Load environment variables from the .env file
source "$(dirname "$0")/../.env"

# Define the array of host entries need for the
# Docker services. NOTE: Only use EXACTLY 1 SPACE between the IP and hostname
declare HOSTS_ARRAY=(
    "127.0.0.1 $HOSTNAME_API"
    "127.0.0.1 $HOSTNAME_CLIENT"
    "127.0.0.1 $KC_HOSTNAME"
)

hosts_add_entries "${HOSTS_ARRAY[@]}"

log_output "[COMPLETE] Host entries have been added successfully." "${COLOR_GREEN}" "hosts_add_entries"
