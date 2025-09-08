#!/bin/bash

# ===============================
# الانتقال إلى مجلد المشروع (الرجوع خطوة للخلف من Commands)
# ===============================
cd .. || { echo "Project directory not found!"; exit 1; }

# ===============================
# حفظ التغييرات الحالية في stash
# ===============================
echo "Stashing local changes..."
git stash save "backup-before-pull-$(date +%Y%m%d%H%M%S)"

# ===============================
# جلب آخر التحديثات من GitHub
# ===============================
echo "Fetching remote changes..."
git fetch origin

# ===============================
# دمج التغييرات
# ===============================
echo "Merging remote main branch..."
git merge origin/main

# ===============================
# استرجاع التغييرات المحلية
# ===============================
echo "Applying stashed local changes..."
git stash pop

# ===============================
# عرض حالة Git بعد السحب
# ===============================
echo "Git status after pull:"
git status

echo "Done."
