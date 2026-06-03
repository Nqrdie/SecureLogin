@echo off
setlocal

cd /d "%~dp0"

pushd API
dotnet build
if errorlevel 1 (
    popd
    pause
    exit /b 1
)
popd

pushd Website
call npm run build
if errorlevel 1 (
    popd
    pause
    exit /b 1
)
popd

start "SecureLogin API" /D "%~dp0API" cmd /k dotnet run
start "SecureLogin Frontend" /D "%~dp0Website" cmd /k node server.js
endlocal
