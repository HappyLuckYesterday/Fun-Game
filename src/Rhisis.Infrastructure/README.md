# Sqlite

## Create migration

### Create migration for Account database

Add-Migration Initial -Context AccountDbContext -Project Rhisis.Infrastructure.Persistance.Sqlite -Startup Rhisis.Infrastructure -Args 'sqlite ../../bin/config/login-server.yml account-database' -Output "Migrations/Account"

### Create migration for Game database

Add-Migration Initial -Context GameDbContext -Project Rhisis.Infrastructure.Persistance.Sqlite -Startup Rhisis.Infrastructure -Args 'sqlite ../../bin/config/cluster-server.yml game-database' -Output "Migrations/Game"

## Remove migration
