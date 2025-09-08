@echo off
echo 🚀 Starting Docker Compose...

:: Go one level up (from Commands to project folder)
cd ..

:: Run Docker Compose
docker compose -f docker-compose.local.yml up --build

echo ✅ Docker Compose finished.
pause
