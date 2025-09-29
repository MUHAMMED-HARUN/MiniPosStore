@echo off
echo 📊 Docker Compose Status Check...

:: تحديد المسار الحالي للملف
set SCRIPT_DIR=%~dp0

echo.
echo 🔍 Checking Local Instance...
docker compose -f "%SCRIPT_DIR%..\docker-compose.local.yml" ps

echo.
echo 🔍 Checking Route Instance...
docker compose -f "%SCRIPT_DIR%..\docker-compose.route.yml" ps

echo.
echo 📋 Container Status:
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

echo.
echo 💡 To start instances, run: Run.bat
echo 💡 To stop instances, run: StopAll.bat
pause
