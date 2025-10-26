// Orders Page JavaScript
// إدارة صفحة الطلبات

document.addEventListener('DOMContentLoaded', function() {
    console.log('Orders page loaded');
    
    // تهيئة الصفحة
    initializePage();
    
    // ربط الأحداث
    bindEvents();
});

// تهيئة الصفحة
function initializePage() {
    console.log('Initializing Orders page...');
    
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
    const dataTable = document.querySelector('#ordersTable');
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
    // ربط نموذج إضافة/تعديل الطلب
    const orderForm = document.getElementById('orderForm');
    if (orderForm) {
        orderForm.addEventListener('submit', handleOrderSubmit);
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
    document.querySelectorAll('.delete-order-btn').forEach(button => {
        button.addEventListener('click', handleDeleteClick);
    });
}

// ربط أزرار التعديل
function bindEditButtons() {
    document.querySelectorAll('.edit-order-btn').forEach(button => {
        button.addEventListener('click', handleEditClick);
    });
}

// ربط أزرار العرض
function bindViewButtons() {
    document.querySelectorAll('.view-order-btn').forEach(button => {
        button.addEventListener('click', handleViewClick);
    });
}

// ربط أزرار التأكيد
function bindConfirmButtons() {
    document.querySelectorAll('.confirm-order-btn').forEach(button => {
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

// معالجة إرسال نموذج الطلب
function handleOrderSubmit(event) {
    event.preventDefault();
    event.stopPropagation();
    
    console.log('Submitting order form...');
    
    const form = event.target;
    const formData = new FormData(form);
    
    // التحقق من صحة البيانات
    if (!validateOrderForm(formData)) {
        return;
    }
    
    // إرسال البيانات
    submitOrderForm(formData);
}

// التحقق من صحة نموذج الطلب
function validateOrderForm(formData) {
    const customerId = formData.get('CustomerID');
    const orderDate = formData.get('OrderDate');
    
    if (!customerId || customerId === '') {
        alert('يرجى اختيار العميل');
        return false;
    }
    
    if (!orderDate || orderDate.trim() === '') {
        alert('يرجى إدخال تاريخ الطلب');
        return false;
    }
    
    return true;
}

// إرسال نموذج الطلب
function submitOrderForm(formData) {
    const url = formData.get('OrderID') ? '/Orders/Edit' : '/Orders/Create';
    
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
            alert('تم حفظ الطلب بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حفظ الطلب: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error submitting order form:', error);
        alert('حدث خطأ أثناء حفظ الطلب');
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
    
    fetch(`/Orders/Search?searchTerm=${encodeURIComponent(searchTerm)}`)
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
    
    // إعادة تحميل الصفحة لعرض جميع الطلبات
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
    
    const button = event.target.closest('.delete-order-btn');
    if (!button) return;
    
    const orderId = button.getAttribute('data-order-id');
    const customerName = button.getAttribute('data-customer-name');
    
    if (confirm(`هل أنت متأكد من حذف طلب العميل "${customerName}"؟`)) {
        deleteOrder(orderId);
    }
}

// معالجة النقر على تعديل
function handleEditClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.edit-order-btn');
    if (!button) return;
    
    const orderId = button.getAttribute('data-order-id');
    
    // الانتقال إلى صفحة التعديل
    window.location.href = `/Orders/Edit/${orderId}`;
}

// معالجة النقر على عرض
function handleViewClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.view-order-btn');
    if (!button) return;
    
    const orderId = button.getAttribute('data-order-id');
    
    // الانتقال إلى صفحة التفاصيل
    window.location.href = `/Orders/Details/${orderId}`;
}

// معالجة النقر على تأكيد
function handleConfirmClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.confirm-order-btn');
    if (!button) return;
    
    const orderId = button.getAttribute('data-order-id');
    const customerName = button.getAttribute('data-customer-name');
    
    if (confirm(`هل أنت متأكد من تأكيد طلب العميل "${customerName}"؟`)) {
        confirmOrder(orderId);
    }
}

// حذف الطلب
function deleteOrder(orderId) {
    console.log('Deleting order:', orderId);
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    fetch(`/Orders/Delete/${orderId}`, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': tokenValue
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('تم حذف الطلب بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حذف الطلب: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error deleting order:', error);
        alert('حدث خطأ أثناء حذف الطلب');
    });
}

// تأكيد الطلب
function confirmOrder(orderId) {
    console.log('Confirming order:', orderId);
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    fetch(`/Orders/Confirm/${orderId}`, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': tokenValue
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('تم تأكيد الطلب بنجاح');
            window.location.reload();
        } else {
            alert('فشل في تأكيد الطلب: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error confirming order:', error);
        alert('حدث خطأ أثناء تأكيد الطلب');
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
window.Orders = {
    initializePage,
    bindEvents,
    handleOrderSubmit,
    validateOrderForm,
    submitOrderForm,
    handleSearch,
    performSearch,
    clearSearch,
    updateSearchResults,
    handleDeleteClick,
    handleEditClick,
    handleViewClick,
    handleConfirmClick,
    deleteOrder,
    confirmOrder,
    showLoadingIndicator,
    hideLoadingIndicator
};
