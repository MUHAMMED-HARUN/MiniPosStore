@echo off
set DOTNET_CLI_UI_LANGUAGE=en

REM Navigate to solution folder (bat already here, so not needed)
cd /d "%~dp0"

REM Build the whole solution
dotnet build MimiPosStore.sln

REM If build succeeded, run the startup project
if %errorlevel%==0 (
    dotnet run --project MimiPosStore\MimiPosStore.csproj
) else (
    echo Build failed!
)
pause
