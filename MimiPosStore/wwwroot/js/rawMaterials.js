// Raw Materials Page JavaScript
// إدارة صفحة المواد الخام

document.addEventListener('DOMContentLoaded', function() {
    console.log('Raw Materials page loaded');
    
    // تهيئة الصفحة
    initializePage();
    
    // ربط الأحداث
    bindEvents();
});

// تهيئة الصفحة
function initializePage() {
    console.log('Initializing Raw Materials page...');
    
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
    const dataTable = document.querySelector('#rawMaterialsTable');
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
    // ربط نموذج إضافة/تعديل المادة الخام
    const rawMaterialForm = document.getElementById('rawMaterialForm');
    if (rawMaterialForm) {
        rawMaterialForm.addEventListener('submit', handleRawMaterialSubmit);
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
}

// ربط أزرار الحذف
function bindDeleteButtons() {
    document.querySelectorAll('.delete-rawmaterial-btn').forEach(button => {
        button.addEventListener('click', handleDeleteClick);
    });
}

// ربط أزرار التعديل
function bindEditButtons() {
    document.querySelectorAll('.edit-rawmaterial-btn').forEach(button => {
        button.addEventListener('click', handleEditClick);
    });
}

// ربط أزرار العرض
function bindViewButtons() {
    document.querySelectorAll('.view-rawmaterial-btn').forEach(button => {
        button.addEventListener('click', handleViewClick);
    });
}

// ربط أحداث البحث
function bindSearchEvents() {
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', debounce(handleSearch, 300));
    }
}

// معالجة إرسال نموذج المادة الخام
function handleRawMaterialSubmit(event) {
    event.preventDefault();
    event.stopPropagation();
    
    console.log('Submitting raw material form...');
    
    const form = event.target;
    const formData = new FormData(form);
    
    // التحقق من صحة البيانات
    if (!validateRawMaterialForm(formData)) {
        return;
    }
    
    // إرسال البيانات
    submitRawMaterialForm(formData);
}

// التحقق من صحة نموذج المادة الخام
function validateRawMaterialForm(formData) {
    const name = formData.get('Name');
    const purchasePrice = formData.get('PurchasePrice');
    
    if (!name || name.trim() === '') {
        alert('يرجى إدخال اسم المادة الخام');
        return false;
    }
    
    if (!purchasePrice || parseFloat(purchasePrice) <= 0) {
        alert('يرجى إدخال سعر الشراء الصحيح');
        return false;
    }
    
    return true;
}

// إرسال نموذج المادة الخام
function submitRawMaterialForm(formData) {
    const url = formData.get('ID') ? '/RawMaterials/Edit' : '/RawMaterials/Create';
    
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
            alert('تم حفظ المادة الخام بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حفظ المادة الخام: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error submitting raw material form:', error);
        alert('حدث خطأ أثناء حفظ المادة الخام');
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
    
    fetch(`/RawMaterials/Search?searchTerm=${encodeURIComponent(searchTerm)}`)
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
    
    // إعادة تحميل الصفحة لعرض جميع المواد الخام
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
    }
}

// معالجة النقر على حذف
function handleDeleteClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.delete-rawmaterial-btn');
    if (!button) return;
    
    const rawMaterialId = button.getAttribute('data-rawmaterial-id');
    const rawMaterialName = button.getAttribute('data-rawmaterial-name');
    
    if (confirm(`هل أنت متأكد من حذف المادة الخام "${rawMaterialName}"؟`)) {
        deleteRawMaterial(rawMaterialId);
    }
}

// معالجة النقر على تعديل
function handleEditClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.edit-rawmaterial-btn');
    if (!button) return;
    
    const rawMaterialId = button.getAttribute('data-rawmaterial-id');
    
    // الانتقال إلى صفحة التعديل
    window.location.href = `/RawMaterials/Edit/${rawMaterialId}`;
}

// معالجة النقر على عرض
function handleViewClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.view-rawmaterial-btn');
    if (!button) return;
    
    const rawMaterialId = button.getAttribute('data-rawmaterial-id');
    
    // الانتقال إلى صفحة التفاصيل
    window.location.href = `/RawMaterials/Details/${rawMaterialId}`;
}

// حذف المادة الخام
function deleteRawMaterial(rawMaterialId) {
    console.log('Deleting raw material:', rawMaterialId);
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    fetch(`/RawMaterials/Delete/${rawMaterialId}`, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': tokenValue
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('تم حذف المادة الخام بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حذف المادة الخام: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error deleting raw material:', error);
        alert('حدث خطأ أثناء حذف المادة الخام');
    });
}

// البحث عن المادة الخام بالاسم (للاستخدام في صفحات أخرى)
function searchRawMaterialByName(searchTerm) {
    return fetch(`/ImportOrders/SearchRawMaterials?searchTerm=${encodeURIComponent(searchTerm)}`)
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
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

// تصدير الدوال للاستخدام العام
window.RawMaterials = {
    initializePage,
    bindEvents,
    handleRawMaterialSubmit,
    validateRawMaterialForm,
    submitRawMaterialForm,
    handleSearch,
    performSearch,
    clearSearch,
    updateSearchResults,
    handleDeleteClick,
    handleEditClick,
    handleViewClick,
    deleteRawMaterial,
    searchRawMaterialByName,
    showLoadingIndicator,
    hideLoadingIndicator
};
