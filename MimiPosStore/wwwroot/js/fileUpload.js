// ملف JavaScript للتعامل مع رفع الملفات
class FileUploadManager {
    constructor() {
        this.maxFileSize = 5 * 1024 * 1024; // 5 MB
        this.allowedExtensions = ['.jpg', '.jpeg', '.png', '.gif', '.bmp'];
    }

    // دالة لاختيار ملف الصورة
    selectImage(inputId, previewId, fileNameId) {
        const input = document.getElementById(inputId);
        const preview = document.getElementById(previewId);
        const fileName = document.getElementById(fileNameId);

        if (input.files && input.files[0]) {
            const file = input.files[0];

            // التحقق من نوع الملف
            if (!this.isValidImageFile(file)) {
                alert('يرجى اختيار ملف صورة صالح (JPG, PNG, GIF, BMP)');
                input.value = '';
                return false;
            }

            // التحقق من حجم الملف
            if (file.size > this.maxFileSize) {
                alert('حجم الملف يجب أن يكون أقل من 5 ميجابايت');
                input.value = '';
                return false;
            }

            // عرض اسم الملف
            if (fileName) {
                fileName.textContent = file.name;
            }

            // عرض معاينة الصورة
            if (preview) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                    preview.style.display = 'block';
                };
                reader.readAsDataURL(file);
            }

            return true;
        }
        return false;
    }

    // التحقق من صحة ملف الصورة
    isValidImageFile(file) {
        const extension = '.' + file.name.split('.').pop().toLowerCase();
        return this.allowedExtensions.includes(extension) && file.type.startsWith('image/');
    }

    // دالة لمسح الصورة المختارة
    clearImage(inputId, previewId, fileNameId) {
        const input = document.getElementById(inputId);
        const preview = document.getElementById(previewId);
        const fileName = document.getElementById(fileNameId);

        if (input) input.value = '';
        if (preview) {
            preview.src = '';
            preview.style.display = 'none';
        }
        if (fileName) fileName.textContent = '';
    }

    // دالة لفتح نافذة اختيار الملف
    openFileDialog(inputId) {
        const input = document.getElementById(inputId);
        if (input) {
            input.click();
        }
    }

    // دالة للتحقق من صحة النموذج قبل الإرسال
    validateForm(formId) {
        const form = document.getElementById(formId);
        if (!form) return false;

        const requiredFields = form.querySelectorAll('[required]');
        let isValid = true;

        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                field.classList.add('is-invalid');
                isValid = false;
            } else {
                field.classList.remove('is-invalid');
            }
        });

        return isValid;
    }

    // دالة لإظهار رسالة نجاح
    showSuccess(message) {
        this.showAlert(message, 'success');
    }

    // دالة لإظهار رسالة خطأ
    showError(message) {
        this.showAlert(message, 'danger');
    }

    // دالة لإظهار رسالة تحذير
    showWarning(message) {
        this.showAlert(message, 'warning');
    }

    // دالة عامة لإظهار الرسائل
    showAlert(message, type) {
        const alertDiv = document.createElement('div');
        alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
        alertDiv.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        // إضافة الرسالة في أعلى الصفحة
        const container = document.querySelector('.container') || document.body;
        container.insertBefore(alertDiv, container.firstChild);

        // إزالة الرسالة تلقائياً بعد 5 ثواني
        setTimeout(() => {
            if (alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 5000);
    }
}

// إنشاء نسخة عامة من مدير رفع الملفات
const fileUploadManager = new FileUploadManager();

// دالة لاختيار صورة المنتج
function selectProductImage() {
    return fileUploadManager.selectImage('productImage', 'imagePreview', 'fileName');
}

// دالة لمسح صورة المنتج
function clearProductImage() {
    fileUploadManager.clearImage('productImage', 'imagePreview', 'fileName');
    
    // في وضع التحديث، إذا تم مسح الصورة، احتفظ بالصورة الحالية
    const productId = document.getElementById('ID').value;
    if (productId && productId !== '0') {
        // لا تمسح ImagePath في وضع التحديث
        // سيتم الاحتفاظ بالصورة الحالية
    } else {
        // في وضع الإضافة، امسح ImagePath
        const imagePathInput = document.getElementById('ImagePath');
        if (imagePathInput) {
            imagePathInput.value = '';
        }
    }
}

// دالة لفتح نافذة اختيار الصورة
function openImageDialog() {
    fileUploadManager.openFileDialog('productImage');
}

// دالة للتحقق من صحة نموذج المنتج
function validateProductForm() {
    return fileUploadManager.validateForm('productForm');
}
