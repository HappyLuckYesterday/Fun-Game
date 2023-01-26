# Sqlite

## Create migration

### Create migration for Account database

Add-Migration Initial -Context AccountDbContext -Project Rhisis.Infrastructure.Persistance.Sqlite -Startup Rhisis.Infrastructure -Args 'sqlite ../../bin/config/login-server.yml' -Output "Migrations/Account"

### Create migration for Game database

Add-Migration Initial -Context GameDbContext -Project Rhisis.Infrastructure.Persistance.Sqlite -Startup Rhisis.Infrastructure -Args 'sqlite ../../bin/config/login-server.yml' -Output "Migrations/Game"

## Remove migration
