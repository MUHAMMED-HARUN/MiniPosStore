@echo off
echo 🛑 Stopping All Docker Compose Instances...

:: تحديد المسار الحالي للملف
set SCRIPT_DIR=%~dp0

:: إيقاف Local Docker Compose
echo 📱 Stopping Local Instance...
docker compose -f "%SCRIPT_DIR%..\docker-compose.local.yml" down

:: إيقاف Route Docker Compose
echo 🌐 Stopping Route Instance...
docker compose -f "%SCRIPT_DIR%..\docker-compose.route.yml" down

echo ✅ All instances stopped successfully!
echo.
echo 📱 LOCAL: Stopped
echo 🌐 ROUTE: Stopped
echo.
echo 💡 To start again, run: Run.bat or CreateAndRun.bat
pause
