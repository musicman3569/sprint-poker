# Sprint Poker

A real-time sprint planning poker application that facilitates story point estimation in agile teams.

## Technologies & Frameworks

- .NET 8.0
- ASP.NET Core
- Entity Framework Core
- React.js + Typescript
- C# 12.0

## Features

- Create and join poker rooms
- Real-time player management
- Card set management for voting
- Player authentication and authorization
- Unique player identification via email

## System Requirements:
- Docker/Docker Compose (Docker Desktop for Windows)
- BASH (for Windows, Git BASH works and is usually installed alongside Git)
- Works as-is on Windows, Linux, and MacOS!

## Quick Start
- To get started, open a BASH terminal (not Powershell) in the project root directory and run the script `DevTools/initialize-environment.sh`.
  This only needs to be run once when you first clone the project, and automatically
  handles all the environment setup for you.
- Run `docker compose up --build --detach` to start the services, or open the solution in your favorite IDE/editor
  and run the Docker Compose build option. That's it!

## Development

The application is currently in development. More features and documentation will be added as the project progresses.

## Customizing the Application Further
The streamlined initialization is done by the `DevTools/initialize-environment.sh` script,
and is driven entirely by the environment variables defined in the `.env` file.
- The minimum amount of variables are used and the rest are generated.
- Passwords are auto-generated based on the .env.template annotations and are
  cryptographically secure, avoiding the need for risky default/shared passwords.
- All the env vars and generated values are then used for configuration of all
  the services, data seeding, etc., ensuring application consistency without complex configuration.
- A custom .env template file can be used prior to running `initialize-environment.sh` to
  customize the environment variables for things like different hostnames/ports.

Aside from the .env file values, there are a few other files that are seeded into the build:

- Custom PEM-formatted SSL certificates can be placed in the `SSL` directory, and should be named
  to match the hostnames used in the .env file, followed by the extensions `.crt` and `.key`.
- The initial database schema SQL scripts are defined in the `Postgres/docker-entrypoint-initdb.d/` file.
  This file is used to initialize the necessary schemas.  NOTE: It __does not__ include data seeding,
  which is done at runtime by the application itself.

## Entity Framework Migrations
- The database is managed using a code-first approach in Entity Framework.
- Migrations are automatically applied when the starwarsinfo docker service starts.
- Because the database connection is defined via docker compose from .env variables
  instead of the local machine, a wrapper script is provided to run `dotnet ef` commands This allows the solution to be IDE/platform agnostic.
  on the docker image. This allows the solution to be more IDE/platform agnostic.
    - Just use `DevTools/ef-cli-wrapper.sh` as a drop-in replacement for `dotnet ef` and
      follow it with the usual arguments, or run it with no arguments to see a convenient list of ef commands..

## Developer Links

- [EF Auditable Implementation - Full](https://medium.com/@bananicabananica/audit-automation-with-ef-core-2f629fb77523)
- [EF Auditable Implementation - Basic](https://dev.to/rickystam/ef-core-how-to-implement-basic-auditing-on-your-entities-1mbm)
