#!/bin/bash
# This script should be run once during initial setup to add custom host entries for the
# local dev retain URLs. This allows the docker host computer to access the docker
# services by their correct hostnames, which is often required for SSL and named
# virtual hosts to work correctly.
#
# This script can be run more than once and will not duplicate the entries if they already exist.

{
    # Get the path to the directory where this script is located so other 
    # scripts can be sourced from this script relative to it's own path.
    declare SCRIPT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
    
    # Hosts file for the current OS, gets set by get_hosts_file()
    declare HOSTS_FILE=""
}

# Import common functions and variables for scripts
source "$SCRIPT_PATH/common-functions.sh"

# Function to add each of the host entries if it does not yet exist
# in the hosts file
hosts_add_entries() {
    local HOSTS_ARRAY=("$@")
    hosts_get_file
    
    # This value should be empty when running as root or when sudo is not available
    local sudo_cmd=$(get_sudo_cmd)

    for entry in "${HOSTS_ARRAY[@]}"; do
        if [ "$entry" == "127.0.0.1 host.docker.internal" ]; then
            log_output "Skipping localhost entry"
            continue
        fi
        
        # Check if the entry already exists
        if ! grep -qF "$entry" "$HOSTS_FILE"; then
            echo "Adding entry: $entry"
            if ! echo "$entry" | $sudo_cmd tee -a "$HOSTS_FILE" > /dev/null; then
                output_permissions_error
            fi
        else
            log_output "Entry already exists: $entry" "$COLOR_GREEN"
        fi
    done
}

# Detects the OS and sets the HOSTS_FILE variable accordingly.
# If the OS is not supported, it exits with an error message.
hosts_get_file() {
    # Detect the OS and return the appropriate hosts file
    # or exit with an error if it cannot be determined
    OS=$(uname -s)
    case "$OS" in
        Linux)
            log_output "Detected Linux"
            HOSTS_FILE="/etc/hosts"
            ;;
        Darwin)
            log_output "Detected MacOS"
            HOSTS_FILE="/private/etc/hosts"
            ;;
        CYGWIN*|MINGW32*|MSYS*|MINGW*)
            log_output "Detected Windows"
            WIN_HOSTS_FILE_1="/mnt/c/Windows/System32/drivers/etc/hosts"
            WIN_HOSTS_FILE_2="/c/Windows/System32/drivers/etc/hosts"
            if [ -f "$WIN_HOSTS_FILE_1" ]; then
                HOSTS_FILE="$WIN_HOSTS_FILE_1"
            else
                if [ -f "$WIN_HOSTS_FILE_2" ]; then
                    HOSTS_FILE="$WIN_HOSTS_FILE_2"
                else
                    log_output "Hosts file not found in the expected location(s): $WIN_HOSTS_FILE_1 OR $WIN_HOSTS_FILE_2" "$COLOR_RED"
                    exit 1
                fi
            fi
            ;;
        *)
            log_output "Unsupported OS: $OS" "$COLOR_RED"
            log_output "This script only supports Linux, MacOS, and Windows." "$COLOR_RED"
            exit 1
            ;;
    esac
}