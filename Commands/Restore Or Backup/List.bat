@echo off
setlocal

:: ===== Configuration =====
set CONTAINER_NAME=mimiposstore-local
set WWWROOT_PATH=/app/wwwroot/product

:: ===== List all files and folders recursively in wwwroot =====
echo Listing all files and folders recursively inside wwwroot of container "%CONTAINER_NAME%"...
docker exec %CONTAINER_NAME% sh -c "ls -R %WWWROOT_PATH%"

echo.
echo Done.
pause
