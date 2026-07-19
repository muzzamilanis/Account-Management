<#
.SYNOPSIS
    Builds, signs, and packages a new Jaini Auto Accounts Manager installer end-to-end.

.DESCRIPTION
    Does everything that used to be four manual steps:
      1. Builds the Release configuration
      2. Signs AMS.exe with the Jaini Motors code-signing certificate
      3. Packages the installer with Inno Setup
      4. Signs the installer exe
    Run it any time you want to hand the client a new build.

.PARAMETER Version
    Optional. Bumps the version number in AMS.iss before building, e.g. -Version 1.2.0.
    If omitted, keeps whatever version is currently set in AMS.iss.

.EXAMPLE
    .\BuildRelease.ps1
    .\BuildRelease.ps1 -Version 1.2.0
#>
param(
    [string]$Version
)

$ErrorActionPreference = "Stop"
$installerDir = $PSScriptRoot
$projectDir   = Split-Path -Parent $installerDir
$issPath      = Join-Path $installerDir "AMS.iss"
$certThumbprint = "8BACB5CBD27889842DA29DE0BFE293AEA8589585"
$isccCandidates = @(
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe"
)

function Fail($msg) {
    Write-Host $msg -ForegroundColor Red
    exit 1
}

Write-Host "== Jaini Auto Accounts Manager - Release Build ==" -ForegroundColor Cyan

# 0. Refuse to build while the app is running - it locks the exe and the build fails halfway through.
if (Get-Process -Name "AMS" -ErrorAction SilentlyContinue) {
    Fail "AMS.exe is currently running (close the app first, including any copy launched from Visual Studio) and try again."
}

# 1. Optional version bump
if ($Version) {
    if (-not (Test-Path $issPath)) { Fail "Could not find $issPath" }
    (Get-Content $issPath) -replace '#define MyAppVersion "[^"]*"', "#define MyAppVersion `"$Version`"" |
        Set-Content $issPath
    Write-Host "Version set to $Version in AMS.iss" -ForegroundColor Yellow
}

# 2. Build Release
Write-Host "`n[1/4] Building Release..." -ForegroundColor Cyan
Push-Location $projectDir
try {
    dotnet build AMS.csproj -c Release
    if ($LASTEXITCODE -ne 0) { Fail "Release build failed - see errors above." }
} finally {
    Pop-Location
}

# 3. Sign the exe
Write-Host "`n[2/4] Signing AMS.exe..." -ForegroundColor Cyan
$cert = Get-ChildItem "Cert:\CurrentUser\My\$certThumbprint" -ErrorAction SilentlyContinue
if (-not $cert) { Fail "Code-signing certificate not found (thumbprint $certThumbprint). Was it removed from the certificate store?" }
$exePath = Join-Path $projectDir "bin\Release\net48\AMS.exe"
if (-not (Test-Path $exePath)) { Fail "Expected build output not found at $exePath" }
Set-AuthenticodeSignature -FilePath $exePath -Certificate $cert -HashAlgorithm SHA256 | Out-Null

# 4. Package the installer
Write-Host "`n[3/4] Packaging installer with Inno Setup..." -ForegroundColor Cyan
$iscc = $isccCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $iscc) { Fail "ISCC.exe (Inno Setup 6) not found. Checked:`n$($isccCandidates -join "`n")`nInstall it from https://jrsoftware.org/isdl.php" }
& $iscc $issPath
if ($LASTEXITCODE -ne 0) { Fail "Installer packaging failed - see errors above." }

# 5. Sign the installer
Write-Host "`n[4/4] Signing installer..." -ForegroundColor Cyan
$setupPath = Join-Path $installerDir "JainiAutoAccountsManager_Setup.exe"
if (-not (Test-Path $setupPath)) { Fail "Expected installer not found at $setupPath" }
Set-AuthenticodeSignature -FilePath $setupPath -Certificate $cert -HashAlgorithm SHA256 | Out-Null

Write-Host "`nDone. Signed installer ready at:" -ForegroundColor Green
Write-Host $setupPath -ForegroundColor Green
