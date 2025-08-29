using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BAL.CustomAttributes
{
    public class ImageValidationAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private readonly int _maxSizeInMB = 5;

        public ImageValidationAttribute() : base("الصورة غير صالحة")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // الصورة اختيارية
            }

            if (value is IFormFile file)
            {
                // التحقق من امتداد الملف
                var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"امتداد الملف غير مسموح به. الامتدادات المسموحة: {string.Join(", ", _allowedExtensions)}");
                }

                // التحقق من حجم الملف
                if (file.Length > _maxSizeInMB * 1024 * 1024)
                {
                    return new ValidationResult($"حجم الملف يجب أن يكون أقل من {_maxSizeInMB} ميجابايت");
                }

                // التحقق من نوع MIME
                if (!file.ContentType.StartsWith("image/"))
                {
                    return new ValidationResult("الملف يجب أن يكون صورة");
                }
            }

            return ValidationResult.Success;
        }
    }
}
