using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BAL
{
    public static class clsUtil
    {
        public static string SaveImage(IFormFile imageFile, string uploadPath)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            try
            {
                // إنشاء مجلد التحميل إذا لم يكن موجوداً
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // إنشاء اسم فريد للملف باستخدام GUID
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                // حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // إرجاع المسار النسبي للملف
                return Path.Combine("product", "imgs", fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حفظ الصورة: {ex.Message}");
            }
        }

        public static bool DeleteImage(string imagePath, string uploadPath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return true;

            try
            {
                var fullPath = Path.Combine(uploadPath, imagePath.Replace("product/imgs/", ""));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حذف الصورة: {ex.Message}");
            }
        }
    }
}
