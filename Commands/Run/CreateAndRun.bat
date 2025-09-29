@echo off
echo 🚀 Starting Local and Route Docker Compose with build...

:: Go one level up (from Commands to project folder)
cd ..
cd ..

:: Build and run Local Docker Compose
echo 📱 Building and starting Local Instance (Development)...
docker compose -f docker-compose.local.yml up --build -d

:: Build and run Route Docker Compose
echo 🌐 Building and starting Route Instance (Wi-Fi Deployment)...
docker compose -f docker-compose.route.yml up --build -d

:: تعيين restart policy لكل حاوية
echo 🔄 Setting restart policy for containers...
docker update --restart unless-stopped MiniPosStoreDB
docker update --restart unless-stopped mimiposstore-local
docker update --restart unless-stopped mimiposstore-route
docker update --restart unless-stopped ef-tools

echo ✅ Both Local and Route instances built and started successfully!
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
