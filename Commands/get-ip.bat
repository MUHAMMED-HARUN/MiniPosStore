@echo off
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr "IPv4"') do (
    set "ip=%%a"
    goto :found
)
:found
set "ip=%ip: =%"
echo %ip%
echo http://%ip%:8082
pause
