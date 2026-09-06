# setup.ps1
# First-time setup for BikeShopApp

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host " BikeShopApp - Setup" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# --------------------------------------------------
# Paths
# --------------------------------------------------

$repoRoot = $PSScriptRoot
$apiRoot = Join-Path $repoRoot "BikeShopAppAPI"
$uiRoot = Join-Path $repoRoot "BikeShopAppUI"

$solution = Join-Path $apiRoot "BikeShopAppSolution.sln"
$webProject = Join-Path $apiRoot "BikeShopApp\BikeShopApp.WebAPI.csproj"
$infrastructureProject = Join-Path $apiRoot "BikeShopApp.Infrastructure\BikeShopApp.Infrastructure.csproj"

# --------------------------------------------------
# Check required files
# --------------------------------------------------

Write-Host "Checking project files..." -ForegroundColor Cyan

if (-not (Test-Path $solution)) {
    throw "Could not find solution: $solution"
}

if (-not (Test-Path $webProject)) {
    throw "Could not find API project: $webProject"
}

if (-not (Test-Path $infrastructureProject)) {
    throw "Could not find Infrastructure project: $infrastructureProject"
}

if (-not (Test-Path (Join-Path $uiRoot "package.json"))) {
    throw "Could not find Angular project: $uiRoot"
}

if (-not (Test-Path (Join-Path $uiRoot "package-lock.json"))) {
    throw "Could not find BikeShopAppUI\package-lock.json. The lockfile should be included in the repository."
}

Write-Host "Project files found." -ForegroundColor Green

# --------------------------------------------------
# Check .NET 10
# --------------------------------------------------

Write-Host ""
Write-Host "Checking .NET SDK..." -ForegroundColor Cyan

try {
    $dotnetVersion = (& dotnet --version).Trim()
}
catch {
    throw ".NET SDK was not found. Please install the .NET 10 SDK."
}

$dotnetMajor = [int]$dotnetVersion.Split('.')[0]

if ($dotnetMajor -ne 10) {
    throw "This project requires the .NET 10 SDK. Detected: $dotnetVersion"
}

Write-Host ".NET SDK: $dotnetVersion" -ForegroundColor Green

# --------------------------------------------------
# Check Node.js
# --------------------------------------------------

Write-Host ""
Write-Host "Checking Node.js..." -ForegroundColor Cyan

try {
    $nodeVersion = (& node --version).Trim()
}
catch {
    throw "Node.js was not found. Please install Node.js 24 or newer."
}

$nodeMajor = [int]$nodeVersion.TrimStart('v').Split('.')[0]

if ($nodeMajor -lt 24) {
    throw "This project requires Node.js 24 or newer. Detected: $nodeVersion"
}

Write-Host "Node.js: $nodeVersion" -ForegroundColor Green

# --------------------------------------------------
# Check npm
# --------------------------------------------------

Write-Host ""
Write-Host "Checking npm..." -ForegroundColor Cyan

try {
    $npmVersion = (& npm --version).Trim()
}
catch {
    throw "npm was not found."
}

Write-Host "npm: $npmVersion" -ForegroundColor Green

# --------------------------------------------------
# Check SQL Server Express
# --------------------------------------------------

Write-Host ""
Write-Host "Checking SQL Server Express..." -ForegroundColor Cyan

$sqlService = Get-Service -Name 'MSSQL$SQLEXPRESS' -ErrorAction SilentlyContinue

if ($null -eq $sqlService) {
    throw "SQL Server Express instance '.\SQLEXPRESS' was not found. Please install SQL Server Express."
}

if ($sqlService.Status -ne "Running") {

    Write-Host "SQL Server Express is installed but not running." -ForegroundColor Yellow
    Write-Host "Attempting to start SQL Server Express..." -ForegroundColor Yellow

    try {
        Start-Service -Name 'MSSQL$SQLEXPRESS'

        $sqlService.WaitForStatus(
            [System.ServiceProcess.ServiceControllerStatus]::Running,
            [TimeSpan]::FromSeconds(15)
        )
    }
    catch {
        throw "Could not start SQL Server Express. You may need to run PowerShell as Administrator."
    }
}

Write-Host "SQL Server Express is running." -ForegroundColor Green

# --------------------------------------------------
# Check Entity Framework CLI
# --------------------------------------------------

Write-Host ""
Write-Host "Checking Entity Framework Core CLI..." -ForegroundColor Cyan

try {
    & dotnet ef --version | Out-Null

    if ($LASTEXITCODE -ne 0) {
        throw "EF Core CLI check failed."
    }
}
catch {
    throw "Entity Framework Core CLI could not be found. Install the .NET EF Core command-line tools."
}

Write-Host "EF Core CLI found." -ForegroundColor Green

# --------------------------------------------------
# Restore .NET packages
# --------------------------------------------------

Write-Host ""
Write-Host "Restoring .NET packages..." -ForegroundColor Cyan

Push-Location $apiRoot

try {
    & dotnet restore $solution

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet restore failed."
    }
}
finally {
    Pop-Location
}

Write-Host "NuGet restore completed." -ForegroundColor Green

# --------------------------------------------------
# Apply EF Core migrations
# --------------------------------------------------

Write-Host ""
Write-Host "Applying Entity Framework Core migrations..." -ForegroundColor Cyan
Write-Host "Database: BikeShopDB" -ForegroundColor DarkGray

Push-Location $apiRoot

try {
    & dotnet ef database update `
        --project $infrastructureProject `
        --startup-project $webProject

    if ($LASTEXITCODE -ne 0) {
        throw "Entity Framework database update failed."
    }
}
finally {
    Pop-Location
}

Write-Host "Database migrations applied successfully." -ForegroundColor Green

# --------------------------------------------------
# Install Angular dependencies
# --------------------------------------------------

Write-Host ""
Write-Host "Installing Angular dependencies from package-lock.json..." -ForegroundColor Cyan

Push-Location $uiRoot

try {
    & npm ci

    if ($LASTEXITCODE -ne 0) {
        throw "npm ci failed."
    }
}
finally {
    Pop-Location
}

Write-Host "Angular dependencies installed." -ForegroundColor Green

# --------------------------------------------------
# Finished
# --------------------------------------------------

Write-Host ""
Write-Host "==========================================" -ForegroundColor Green
Write-Host " Setup completed successfully!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
Write-Host ""
Write-Host "BikeShopDB has been created/updated."
Write-Host "Seed data will be inserted automatically when the API starts."
Write-Host ""
Write-Host "Start the application with:"
Write-Host "  .\start.ps1" -ForegroundColor Cyan
Write-Host ""
Write-Host "Note: npm may report audit or install-script warnings."
Write-Host "These do not necessarily mean setup failed." -ForegroundColor DarkGray
Write-Host ""