if ($args.Count -lt 1) {
    Write-Host 'Usage: ./CreateMigration.ps1 [DatabaseConfigurationFile]'
    exit 1
}

$env:DB_CONFIG = $args[0]

dotnet ef migrations remove -p "Rhisis.Infrastructure.Persistance.csproj" -s "../Rhisis.CLI/Rhisis.CLI.csproj"