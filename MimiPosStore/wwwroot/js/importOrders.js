// Import Orders Page JavaScript
// إدارة صفحة أوامر الاستيراد

document.addEventListener('DOMContentLoaded', function() {
    console.log('Import Orders page loaded');
    
    // تهيئة الصفحة
    initializePage();
    
    // ربط الأحداث
    bindEvents();
});

// تهيئة الصفحة
function initializePage() {
    console.log('Initializing Import Orders page...');
    
    // تهيئة جداول البيانات
    initializeDataTables();
    
    // تهيئة البحث
    initializeSearch();
}

// ربط الأحداث
function bindEvents() {
    // ربط أحداث النماذج
    bindFormEvents();
    
    // ربط أحداث الأزرار
    bindButtonEvents();
    
    // ربط أحداث البحث
    bindSearchEvents();
}

// تهيئة جداول البيانات
function initializeDataTables() {
    // تهيئة DataTable إذا كان موجوداً
    const dataTable = document.querySelector('#importOrdersTable');
    if (dataTable && typeof $.fn.DataTable !== 'undefined') {
        $(dataTable).DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Arabic.json"
            },
            "pageLength": 25,
            "responsive": true,
            "order": [[0, "desc"]]
        });
    }
}

// تهيئة البحث
function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', handleSearch);
    }
}

// ربط أحداث النماذج
function bindFormEvents() {
    // ربط نموذج إضافة/تعديل أمر الاستيراد
    const importOrderForm = document.getElementById('importOrderForm');
    if (importOrderForm) {
        importOrderForm.addEventListener('submit', handleImportOrderSubmit);
    }
    
    // ربط نموذج البحث
    const searchForm = document.getElementById('searchForm');
    if (searchForm) {
        searchForm.addEventListener('submit', handleSearchSubmit);
    }
}

// ربط أحداث الأزرار
function bindButtonEvents() {
    // ربط أزرار الحذف
    bindDeleteButtons();
    
    // ربط أزرار التعديل
    bindEditButtons();
    
    // ربط أزرار العرض
    bindViewButtons();
    
    // ربط أزرار التأكيد
    bindConfirmButtons();
}

// ربط أزرار الحذف
function bindDeleteButtons() {
    document.querySelectorAll('.delete-importorder-btn').forEach(button => {
        button.addEventListener('click', handleDeleteClick);
    });
}

// ربط أزرار التعديل
function bindEditButtons() {
    document.querySelectorAll('.edit-importorder-btn').forEach(button => {
        button.addEventListener('click', handleEditClick);
    });
}

// ربط أزرار العرض
function bindViewButtons() {
    document.querySelectorAll('.view-importorder-btn').forEach(button => {
        button.addEventListener('click', handleViewClick);
    });
}

// ربط أزرار التأكيد
function bindConfirmButtons() {
    document.querySelectorAll('.confirm-importorder-btn').forEach(button => {
        button.addEventListener('click', handleConfirmClick);
    });
}

// ربط أحداث البحث
function bindSearchEvents() {
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', debounce(handleSearch, 300));
    }
}

// معالجة إرسال نموذج أمر الاستيراد
function handleImportOrderSubmit(event) {
    event.preventDefault();
    event.stopPropagation();
    
    console.log('Submitting import order form...');
    
    const form = event.target;
    const formData = new FormData(form);
    
    // التحقق من صحة البيانات
    if (!validateImportOrderForm(formData)) {
        return;
    }
    
    // إرسال البيانات
    submitImportOrderForm(formData);
}

// التحقق من صحة نموذج أمر الاستيراد
function validateImportOrderForm(formData) {
    const supplierId = formData.get('SupplierID');
    const importDate = formData.get('ImportDate');
    
    if (!supplierId || supplierId === '') {
        alert('يرجى اختيار المورد');
        return false;
    }
    
    if (!importDate || importDate.trim() === '') {
        alert('يرجى إدخال تاريخ الاستيراد');
        return false;
    }
    
    return true;
}

// إرسال نموذج أمر الاستيراد
function submitImportOrderForm(formData) {
    const url = formData.get('ImportOrderID') ? '/ImportOrders/Edit' : '/ImportOrders/Create';
    
    fetch(url, {
        method: 'POST',
        body: formData
    })
    .then(response => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Network response was not ok');
    })
    .then(data => {
        if (data.success) {
            alert('تم حفظ أمر الاستيراد بنجاح');
            if (data.redirectUrl) {
                window.location.href = data.redirectUrl;
            }
        } else {
            alert('فشل في حفظ أمر الاستيراد: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error submitting import order form:', error);
        alert('حدث خطأ أثناء حفظ أمر الاستيراد');
    });
}

// معالجة البحث
function handleSearch(event) {
    const searchTerm = event.target.value.trim();
    console.log('Searching for:', searchTerm);
    
    if (searchTerm.length >= 2) {
        performSearch(searchTerm);
    } else if (searchTerm.length === 0) {
        clearSearch();
    }
}

// معالجة إرسال نموذج البحث
function handleSearchSubmit(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const searchTerm = document.getElementById('searchInput').value.trim();
    if (searchTerm) {
        performSearch(searchTerm);
    }
}

// تنفيذ البحث
function performSearch(searchTerm) {
    console.log('Performing search for:', searchTerm);
    
    // إظهار مؤشر التحميل
    showLoadingIndicator();
    
    fetch(`/ImportOrders/Search?searchTerm=${encodeURIComponent(searchTerm)}`)
        .then(response => response.text())
        .then(html => {
            // تحديث نتائج البحث
            updateSearchResults(html);
            
            // إخفاء مؤشر التحميل
            hideLoadingIndicator();
        })
        .catch(error => {
            console.error('Error performing search:', error);
            hideLoadingIndicator();
            alert('حدث خطأ أثناء البحث');
        });
}

// مسح البحث
function clearSearch() {
    console.log('Clearing search...');
    
    // إعادة تحميل الصفحة لعرض جميع أوامر الاستيراد
    window.location.reload();
}

// تحديث نتائج البحث
function updateSearchResults(html) {
    const resultsContainer = document.getElementById('searchResults');
    if (resultsContainer) {
        resultsContainer.innerHTML = html;
        
        // إعادة ربط الأحداث للعناصر الجديدة
        bindDeleteButtons();
        bindEditButtons();
        bindViewButtons();
        bindConfirmButtons();
    }
}

// معالجة النقر على حذف
function handleDeleteClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.delete-importorder-btn');
    if (!button) return;
    
    const importOrderId = button.getAttribute('data-importorder-id');
    const supplierName = button.getAttribute('data-supplier-name');
    
    if (confirm(`هل أنت متأكد من حذف أمر الاستيراد للمورد "${supplierName}"؟`)) {
        deleteImportOrder(importOrderId);
    }
}

// معالجة النقر على تعديل
function handleEditClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.edit-importorder-btn');
    if (!button) return;
    
    const importOrderId = button.getAttribute('data-importorder-id');
    
    // الانتقال إلى صفحة التعديل
    window.location.href = `/ImportOrders/Edit/${importOrderId}`;
}

// معالجة النقر على عرض
function handleViewClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.view-importorder-btn');
    if (!button) return;
    
    const importOrderId = button.getAttribute('data-importorder-id');
    
    // الانتقال إلى صفحة التفاصيل
    window.location.href = `/ImportOrders/Details/${importOrderId}`;
}

// معالجة النقر على تأكيد
function handleConfirmClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.confirm-importorder-btn');
    if (!button) return;
    
    const importOrderId = button.getAttribute('data-importorder-id');
    const supplierName = button.getAttribute('data-supplier-name');
    
    if (confirm(`هل أنت متأكد من تأكيد أمر الاستيراد للمورد "${supplierName}"؟`)) {
        confirmImportOrder(importOrderId);
    }
}

// حذف أمر الاستيراد
function deleteImportOrder(importOrderId) {
    console.log('Deleting import order:', importOrderId);
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    fetch(`/ImportOrders/Delete/${importOrderId}`, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': tokenValue
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('تم حذف أمر الاستيراد بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حذف أمر الاستيراد: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error deleting import order:', error);
        alert('حدث خطأ أثناء حذف أمر الاستيراد');
    });
}

// تأكيد أمر الاستيراد
function confirmImportOrder(importOrderId) {
    console.log('Confirming import order:', importOrderId);
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    fetch(`/ImportOrders/Confirm/${importOrderId}`, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': tokenValue
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('تم تأكيد أمر الاستيراد بنجاح');
            window.location.reload();
        } else {
            alert('فشل في تأكيد أمر الاستيراد: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error confirming import order:', error);
        alert('حدث خطأ أثناء تأكيد أمر الاستيراد');
    });
}

// إظهار مؤشر التحميل
function showLoadingIndicator() {
    const loadingIndicator = document.getElementById('loadingIndicator');
    if (loadingIndicator) {
        loadingIndicator.style.display = 'block';
    }
}

// إخفاء مؤشر التحميل
function hideLoadingIndicator() {
    const loadingIndicator = document.getElementById('loadingIndicator');
    if (loadingIndicator) {
        loadingIndicator.style.display = 'none';
    }
}

// تصدير الدوال للاستخدام العام
window.ImportOrders = {
    initializePage,
    bindEvents,
    handleImportOrderSubmit,
    validateImportOrderForm,
    submitImportOrderForm,
    handleSearch,
    performSearch,
    clearSearch,
    updateSearchResults,
    handleDeleteClick,
    handleEditClick,
    handleViewClick,
    handleConfirmClick,
    deleteImportOrder,
    confirmImportOrder,
    showLoadingIndicator,
    hideLoadingIndicator
};
