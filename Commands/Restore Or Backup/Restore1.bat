@echo off
setlocal

:: ========================================
:: ====== Configuration ===================
:: ========================================
set CONTAINER_NAME_WWW=mimiposstore-local
set CONTAINER_NAME_DB=MiniPosStoreDB
set DEST_FOLDER=%USERPROFILE%\Documents\MiniPosStore
set WWWROOT_DEST=%DEST_FOLDER%\wwwroot
set DB_BACKUP_DEST=%DEST_FOLDER%\backup
set WWWROOT_SRC=%DEST_FOLDER%\wwwroot\product
set DB_BACKUP_SRC=%DEST_FOLDER%\backup\MiniPosStore.bak
set SA_PASSWORD=Sa123456!
set DB_NAME=MiniPosStore
set DB_VOLUME=miniposstore_sql_data

:: ========================================
:: ====== Create destination directories ===
:: ========================================
echo Creating destination directories...
mkdir "%WWWROOT_DEST%" >nul 2>&1
mkdir "%DB_BACKUP_DEST%" >nul 2>&1

:: ========================================
:: ====== Check if source files exist =====
:: ========================================
if not exist "%WWWROOT_SRC%" (
    echo ERROR: wwwroot source folder "%WWWROOT_SRC%" does not exist!
    pause & exit /b
)

if not exist "%DB_BACKUP_SRC%" (
    echo ERROR: Database backup "%DB_BACKUP_SRC%" not found!
    pause & exit /b
)

:: ========================================
:: ====== Check and start containers ======
:: ========================================
echo Checking containers...
docker ps -a --format "{{.Names}}" | findstr /I "%CONTAINER_NAME_WWW%" >nul
if errorlevel 1 (
    echo ERROR: Container %CONTAINER_NAME_WWW% not found. Please create it via docker-compose.
    pause & exit /b
) else (
    echo Starting %CONTAINER_NAME_WWW% if stopped...
    docker start %CONTAINER_NAME_WWW% >nul 2>&1
)

docker ps -a --format "{{.Names}}" | findstr /I "%CONTAINER_NAME_DB%" >nul
if errorlevel 1 (
    echo ERROR: Container %CONTAINER_NAME_DB% not found. Please create it via docker-compose.
    pause & exit /b
) else (
    echo Starting %CONTAINER_NAME_DB% if stopped...
    docker start %CONTAINER_NAME_DB% >nul 2>&1
)

:: ========================================
:: ====== Copy wwwroot to container =======
:: ========================================
echo Copying wwwroot/product to container...
docker cp "%WWWROOT_SRC%" %CONTAINER_NAME_WWW%:/app/wwwroot/product
if errorlevel 1 (
    echo ERROR: Failed to copy wwwroot/product!
    pause & exit /b
) else (
    echo wwwroot/product copied successfully.
)

:: ========================================
:: ====== Copy database backup to volume ==
:: ========================================
echo Copying database backup to container volume...
docker run --rm -v %DB_VOLUME%:/var/opt/mssql -v "%DB_BACKUP_SRC%":/backup/backup.bak alpine sh -c "cp /backup/backup.bak /var/opt/mssql/backup.bak"
if errorlevel 1 (
    echo ERROR: Failed to copy database backup to volume!
    pause & exit /b
) else (
    echo Database backup copied successfully to volume.
)

:: ========================================
:: ====== Restore database in container ===
:: ========================================
echo Restoring database inside container...
docker run -i --rm --network container:%CONTAINER_NAME_DB% -v %DB_VOLUME%:/var/opt/mssql mcr.microsoft.com/mssql-tools /opt/mssql-tools/bin/sqlcmd ^
    -S localhost -U sa -P "%SA_PASSWORD%" ^
    -Q "ALTER DATABASE [%DB_NAME%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [%DB_NAME%] FROM DISK = N'/var/opt/mssql/backup.bak' WITH REPLACE, MOVE 'MiniPosStore' TO '/var/opt/mssql/data/MiniPosStore.mdf', MOVE 'MiniPosStore_log' TO '/var/opt/mssql/data/MiniPosStore_log.ldf'; ALTER DATABASE [%DB_NAME%] SET MULTI_USER;"
if errorlevel 1 (
    echo ERROR: Failed to restore database!
    pause & exit /b
) else (
    echo Database restored successfully.
)

:: ========================================
:: ====== Finish ==========================
:: ========================================
echo All operations completed successfully.
pause
