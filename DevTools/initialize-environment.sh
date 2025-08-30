#!/bin/bash
# This is the main setup script for setting up your environment for the first time.
# Once your environment is working, do you not need to use this any more and can instead
# use individial utility scripts from the DevTools.

set -e

# Usage: initialize-environment <env_template tile>
# Accepts optional custom .env.template as argument, or
# use the default template if one is not supplied.

# Initialize global script variables.
{
    SCRIPT_DIR="$(dirname "$0")"
    
    # ANSI colors for coloring terminal output
    declare COLOR_RED='\033[0;31m'
    declare COLOR_NONE='\033[0m'
    declare COLOR_GREEN='\033[0;32m'
    declare COLOR_BLUE='\033[0;34m'
}

"$SCRIPT_DIR"/env_run_all_checks.sh "$1"
"$SCRIPT_DIR"/keycloak-prep-import.sh
"$SCRIPT_DIR"/hosts-add-entries.sh
"$SCRIPT_DIR"/ssl-generate-certs.sh

cd "$SCRIPT_DIR"/..

# Only restore the tools if dotnet is installed.  This is primarily for
# development environments, since non-development environments do not
# need to have dotnet installed or use the tools.
if command -v dotnet &> /dev/null; then
    dotnet tool restore
fi

# Load environment variables from the .env file
source "$(dirname "$0")/../.env"

echo -e "$COLOR_GREEN"
echo "================================================"
echo "Congratulations, your environment is configured!"
echo "================================================"
echo -e "\nYou can start the service by either:"
echo -e "1) Running: ${COLOR_NONE}docker compose up --build --detach${COLOR_GREEN} OR"
echo -e "2) Opening the solution in Visual Studio/Rider/etc. and using the Docker Compose build option.\n"
echo "It will take a few minutes to build. Check the docker logs and ensure all"
echo "services build and are listening on their ports."
echo "See the README.md for additional details."
echo ""
echo "Once the services are ready you can access the main site at:"
echo "https://${HOSTNAME_CLIENT}:${CLIENT_HTTPS_PORT}"
echo -e "$COLOR_NONE"
