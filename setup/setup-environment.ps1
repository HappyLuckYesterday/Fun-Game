function WriteToFile {
    param (
        [string] $FileName,
        [string] $Content,
        [bool] $Force = $false
    )

    if ($Force -eq $true) {
        Set-Content -Path "$FileName" -Value "$Content" -Force
    }
    else {
        Add-Content -Path "$FileName" -Value "$Content"
    }
}

function BuildSolution {
    Write-Host "Building Rhisis solution..."
    dotnet build Rhisis.sln
}

function ConfigureMySQLServer {
    Write-Host "Configuring MySQL Server..."
    $MySQLUser = Read-Host -Prompt "MySQL Server username"
    $MySQLPassword = Read-Host -Prompt "MySQL Server user's password"
    $MySQLDatabase = Read-Host -Prompt "Database name"

    WriteToFile -FileName ".env" -Content "MYSQL_USER=$MySQLUser" -Force $true
    WriteToFile -FileName ".env" -Content "MYSQL_PASSWORD=$MySQLPassword"
    WriteToFile -FileName ".env" -Content "MYSQL_DATABASE=$MySQLDatabase"

    docker-compose up -d rhisis.database
    docker-compose up rhisis.database.waiter

    Start-Process -FilePath "./bin/rhisis-cli" -WorkingDirectory "./bin" -ArgumentList "database update -s 127.0.0.1 -u ${MySQLUser} -pwd ${MySQLPassword} -d ${MySQLDatabase} -p 3307" -NoNewWindow -Wait

    docker-compose down
}

function Main {
    Write-Host "======= RHISIS-PROJECT ======="
    BuildSolution
    ConfigureMySQLServer
}

Main
