# start.ps1
# Starts the BikeShopApp API and Angular UI.

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host " BikeShopApp - Start" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$repoRoot = $PSScriptRoot
$apiRoot = Join-Path $repoRoot "BikeShopAppAPI"
$uiRoot = Join-Path $repoRoot "BikeShopAppUI"

$webProject = Join-Path $apiRoot "BikeShopApp\BikeShopApp.WebAPI.csproj"

# --------------------------------------------------
# Check required files
# --------------------------------------------------

if (-not (Test-Path $webProject)) {
    throw "Could not find API project: $webProject"
}

if (-not (Test-Path (Join-Path $uiRoot "package.json"))) {
    throw "Could not find Angular project: $uiRoot"
}

# --------------------------------------------------
# Start API
# --------------------------------------------------

Write-Host "Starting API..." -ForegroundColor Cyan

Start-Process powershell.exe `
    -WorkingDirectory $apiRoot `
    -ArgumentList @(
        "-NoExit",
        "-Command",
        "dotnet run --project `"$webProject`""
    )

Start-Sleep -Seconds 2

# --------------------------------------------------
# Start Angular UI
# --------------------------------------------------

Write-Host "Starting Angular UI..." -ForegroundColor Cyan

Start-Process powershell.exe `
    -WorkingDirectory $uiRoot `
    -ArgumentList @(
        "-NoExit",
        "-Command",
        "npm start"
    )

Write-Host ""
Write-Host "==========================================" -ForegroundColor Green
Write-Host " Application started!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Angular: http://localhost:4200" -ForegroundColor Cyan
Write-Host "The API URL is shown in the API PowerShell window." -ForegroundColor White
Write-Host ""