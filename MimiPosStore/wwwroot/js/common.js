// Common JavaScript Utilities
// أدوات JavaScript المشتركة

// دالة تأخير للبحث
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// دالة إظهار مؤشر التحميل
function showLoadingIndicator(elementId = 'loadingIndicator') {
    const loadingIndicator = document.getElementById(elementId);
    if (loadingIndicator) {
        loadingIndicator.style.display = 'block';
    }
}

// دالة إخفاء مؤشر التحميل
function hideLoadingIndicator(elementId = 'loadingIndicator') {
    const loadingIndicator = document.getElementById(elementId);
    if (loadingIndicator) {
        loadingIndicator.style.display = 'none';
    }
}

// دالة إظهار رسالة نجاح
function showSuccessMessage(message) {
    // يمكن استخدام مكتبة مثل SweetAlert أو Toastr
    alert(message);
}

// دالة إظهار رسالة خطأ
function showErrorMessage(message) {
    // يمكن استخدام مكتبة مثل SweetAlert أو Toastr
    alert('خطأ: ' + message);
}

// دالة إظهار رسالة تأكيد
function showConfirmMessage(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// دالة تنسيق الأرقام
function formatNumber(number, decimals = 2) {
    return parseFloat(number).toFixed(decimals);
}

// دالة تنسيق العملة
function formatCurrency(amount, currency = 'TRY') {
    return formatNumber(amount) + ' ' + currency;
}

// دالة التحقق من صحة البريد الإلكتروني
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// دالة التحقق من صحة رقم الهاتف
function isValidPhone(phone) {
    const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/;
    return phoneRegex.test(phone);
}

// دالة إرسال طلب AJAX
function sendAjaxRequest(url, options = {}) {
    const defaultOptions = {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    };
    
    const mergedOptions = { ...defaultOptions, ...options };
    
    return fetch(url, mergedOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response;
        });
}

// دالة إرسال طلب AJAX مع JSON
function sendJsonRequest(url, data, method = 'POST') {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    return sendAjaxRequest(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': tokenValue
        },
        body: JSON.stringify(data)
    });
}

// دالة إرسال طلب AJAX مع FormData
function sendFormRequest(url, formData, method = 'POST') {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    return sendAjaxRequest(url, {
        method: method,
        headers: {
            'RequestVerificationToken': tokenValue
        },
        body: formData
    });
}

// دالة تحديث محتوى العنصر
function updateElementContent(elementId, content) {
    const element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = content;
    }
}

// دالة إضافة class للعنصر
function addClass(elementId, className) {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.add(className);
    }
}

// دالة إزالة class من العنصر
function removeClass(elementId, className) {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.remove(className);
    }
}

// دالة تبديل class للعنصر
function toggleClass(elementId, className) {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.toggle(className);
    }
}

// دالة إخفاء العنصر
function hideElement(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.display = 'none';
    }
}

// دالة إظهار العنصر
function showElement(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.display = 'block';
    }
}

// دالة إعادة تحميل الصفحة
function reloadPage() {
    window.location.reload();
}

// دالة الانتقال إلى صفحة
function redirectTo(url) {
    window.location.href = url;
}

// دالة فتح نافذة جديدة
function openWindow(url, name = '_blank', features = '') {
    window.open(url, name, features);
}

// دالة إغلاق النافذة الحالية
function closeWindow() {
    window.close();
}

// دالة الحصول على معاملات URL
function getUrlParameter(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}

// دالة تعيين معاملات URL
function setUrlParameter(name, value) {
    const url = new URL(window.location);
    url.searchParams.set(name, value);
    window.history.pushState({}, '', url);
}

// دالة إزالة معاملات URL
function removeUrlParameter(name) {
    const url = new URL(window.location);
    url.searchParams.delete(name);
    window.history.pushState({}, '', url);
}

// دالة نسخ النص إلى الحافظة
function copyToClipboard(text) {
    if (navigator.clipboard) {
        navigator.clipboard.writeText(text).then(() => {
            showSuccessMessage('تم نسخ النص إلى الحافظة');
        });
    } else {
        // Fallback for older browsers
        const textArea = document.createElement('textarea');
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand('copy');
        document.body.removeChild(textArea);
        showSuccessMessage('تم نسخ النص إلى الحافظة');
    }
}

// دالة تحويل التاريخ إلى تنسيق عربي
function formatDateToArabic(date) {
    const options = { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric',
        weekday: 'long'
    };
    return new Intl.DateTimeFormat('ar-SA', options).format(new Date(date));
}

// دالة تحويل التاريخ إلى تنسيق قصير
function formatDateShort(date) {
    const options = { 
        year: 'numeric', 
        month: '2-digit', 
        day: '2-digit'
    };
    return new Intl.DateTimeFormat('ar-SA', options).format(new Date(date));
}

// دالة تحويل الوقت إلى تنسيق عربي
function formatTimeToArabic(date) {
    const options = { 
        hour: '2-digit', 
        minute: '2-digit',
        second: '2-digit'
    };
    return new Intl.DateTimeFormat('ar-SA', options).format(new Date(date));
}

// دالة التحقق من وجود العنصر
function elementExists(elementId) {
    return document.getElementById(elementId) !== null;
}

// دالة الحصول على قيمة العنصر
function getElementValue(elementId) {
    const element = document.getElementById(elementId);
    return element ? element.value : null;
}

// دالة تعيين قيمة العنصر
function setElementValue(elementId, value) {
    const element = document.getElementById(elementId);
    if (element) {
        element.value = value;
    }
}

// دالة الحصول على نص العنصر
function getElementText(elementId) {
    const element = document.getElementById(elementId);
    return element ? element.textContent : null;
}

// دالة تعيين نص العنصر
function setElementText(elementId, text) {
    const element = document.getElementById(elementId);
    if (element) {
        element.textContent = text;
    }
}

// دالة إضافة مستمع حدث
function addEventListener(elementId, event, handler) {
    const element = document.getElementById(elementId);
    if (element) {
        element.addEventListener(event, handler);
    }
}

// دالة إزالة مستمع حدث
function removeEventListener(elementId, event, handler) {
    const element = document.getElementById(elementId);
    if (element) {
        element.removeEventListener(event, handler);
    }
}

// تصدير الدوال للاستخدام العام
window.Common = {
    debounce,
    showLoadingIndicator,
    hideLoadingIndicator,
    showSuccessMessage,
    showErrorMessage,
    showConfirmMessage,
    formatNumber,
    formatCurrency,
    isValidEmail,
    isValidPhone,
    sendAjaxRequest,
    sendJsonRequest,
    sendFormRequest,
    updateElementContent,
    addClass,
    removeClass,
    toggleClass,
    hideElement,
    showElement,
    reloadPage,
    redirectTo,
    openWindow,
    closeWindow,
    getUrlParameter,
    setUrlParameter,
    removeUrlParameter,
    copyToClipboard,
    formatDateToArabic,
    formatDateShort,
    formatTimeToArabic,
    elementExists,
    getElementValue,
    setElementValue,
    getElementText,
    setElementText,
    addEventListener,
    removeEventListener
};
