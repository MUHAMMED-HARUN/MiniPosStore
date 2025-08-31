@echo off
SETLOCAL

REM ===============================
REM إعداد مسار المشروع
REM ===============================
SET PROJECT_DIR=D:\Project\ddd\MiniPosStore
cd /d %PROJECT_DIR%

REM ===============================
REM إزالة ملفات البناء المؤقتة من Git
REM ===============================
echo Removing temporary build files...
for /d %%d in (bin obj) do (
    if exist %%d (
        git rm -r --cached %%d
    )
)

REM ===============================
REM إضافة جميع الملفات المهمة
REM ===============================
echo Adding files...
git add .

REM ===============================
REM عمل Commit
REM ===============================
set /p commitMessage="Enter commit message: "
if "%commitMessage%"=="" set commitMessage=Update
git commit -m "%commitMessage%"

REM ===============================
REM جلب أي تغييرات من المستودع ودمجها
REM ===============================
echo Fetching remote changes...
git fetch origin

echo Merging remote changes...
git merge origin/main

REM ===============================
REM رفع التغييرات إلى GitHub
REM ===============================
echo Pushing to remote repository...
git push origin main

echo Done.
pause
