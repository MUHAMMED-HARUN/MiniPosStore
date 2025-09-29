@echo off
setlocal

:: ===== Configuration =====
set CONTAINER_NAME_WWW=mimiposstore-local
set CONTAINER_NAME_DB=MiniPosStoreDB
set DEST_FOLDER=%USERPROFILE%\Documents\MiniPosStore
set WWWROOT_DEST=%DEST_FOLDER%\wwwroot
set DB_BACKUP_DEST=%DEST_FOLDER%\backup
set DB_BACKUP_FILE=MiniPosStore.bak
set SA_PASSWORD=Sa123456!
set DB_NAME=MiniPosStore
set MSSQL_TOOLS_IMAGE=mcr.microsoft.com/mssql-tools

:: ===== Create directories =====
echo Creating backup directories...
mkdir "%WWWROOT_DEST%" >nul 2>&1
mkdir "%DB_BACKUP_DEST%" >nul 2>&1

:: ===== Check if containers exist =====
echo Checking containers...
docker inspect %CONTAINER_NAME_WWW% >nul 2>&1
if errorlevel 1 (
    echo WWW container not found. Please create or start "%CONTAINER_NAME_WWW%" first.
    pause
    exit /b
)

docker inspect %CONTAINER_NAME_DB% >nul 2>&1
if errorlevel 1 (
    echo DB container not found. Please create or start "%CONTAINER_NAME_DB%" first.
    pause
    exit /b
)

:: ===== Copy wwwroot =====
echo Copying wwwroot from container...
docker cp %CONTAINER_NAME_WWW%:/app/wwwroot/. "%WWWROOT_DEST%" >nul 2>&1
if errorlevel 1 (
    echo Error occurred while copying wwwroot!
) else (
    echo wwwroot copied successfully to "%WWWROOT_DEST%".
)

:: ===== Ensure backup folder inside DB container =====
docker exec %CONTAINER_NAME_DB% bash -c "mkdir -p /var/opt/mssql/backup"

:: ===== Backup database using temporary sqlcmd container on Windows =====
echo Creating database backup using sqlcmd...
docker run -it --rm %MSSQL_TOOLS_IMAGE% /opt/mssql-tools/bin/sqlcmd -S host.docker.internal,1433 -U sa -P "%SA_PASSWORD%" -Q "BACKUP DATABASE [%DB_NAME%] TO DISK = N'/var/opt/mssql/backup/%DB_BACKUP_FILE%' WITH INIT, FORMAT"

:: ===== Copy backup to host =====
echo Copying database backup to host...
docker cp %CONTAINER_NAME_DB%:/var/opt/mssql/backup/%DB_BACKUP_FILE% "%DB_BACKUP_DEST%\%DB_BACKUP_FILE%"

if exist "%DB_BACKUP_DEST%\%DB_BACKUP_FILE%" (
    echo Database backup successfully copied to "%DB_BACKUP_DEST%\%DB_BACKUP_FILE%".
) else (
    echo Error: Database backup not found!
)

echo Backup process finished.
pause
