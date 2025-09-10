@echo off
echo 🚀 Starting Docker Compose with build...

:: Go one level up (from Commands to project folder)
cd ..

:: Run Docker Compose with build
docker compose -f docker-compose.local.yml up --build -d

echo ✅ Docker Compose finished.
