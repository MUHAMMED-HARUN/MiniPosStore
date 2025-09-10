@echo off
echo 🚀 Starting Docker Compose...

:: تشغيل Docker Compose بدون إعادة البناء
docker compose -f docker-compose.local.yml up -d

:: تعيين restart policy لكل حاوية
echo 🔄 Setting restart policy for containers...
docker update --restart unless-stopped MiniPosStoreDB
docker update --restart unless-stopped mimiposstore-local
docker update --restart unless-stopped ef-tools

echo ✅ Docker Compose started and restart policies applied.
pause
