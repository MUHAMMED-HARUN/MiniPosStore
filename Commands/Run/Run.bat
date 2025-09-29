@echo off
echo 🚀 Starting Local and Route Docker Compose...

:: الانتقال إلى مجلد المشروع (افتراضياً السكربت في مجلد فرعي مثل Scripts)
cd ..\..

:: تشغيل Local Docker Compose
echo 📱 Starting Local Instance (Development)...
docker compose -f "docker-compose.local.yml" up -d

:: تشغيل Route Docker Compose
echo 🌐 Starting Route Instance (Wi-Fi Deployment)...
docker compose -f "docker-compose.route.yml" up -d

:: تعيين restart policy لكل حاوية
echo 🔄 Setting restart policy for containers...
docker update --restart unless-stopped MiniPosStoreDB
docker update --restart unless-stopped mimiposstore-local
docker update --restart unless-stopped mimiposstore-route
docker update --restart unless-stopped ef-tools

echo ✅ Both Local and Route instances started successfully!
echo.
echo 📱 LOCAL (Development):
echo    - Database: MiniPosStoreDB (Port 1433)
echo    - Application: http://localhost:8080
echo    - EF Tools: Available
echo.
echo 🌐 ROUTE (Wi-Fi Deployment):
echo    - Database: MiniPosStoreDB (Port 1433) - Separate
echo    - Application: http://localhost:8082
echo    - Wi-Fi Access: Run get-ip.bat for IP
echo    - EF Tools: Available
echo.
echo 💡 To get Wi-Fi access IP, run: get-ip.bat
pause
