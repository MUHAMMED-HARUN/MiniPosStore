// Products Page JavaScript
// إدارة صفحة المنتجات

document.addEventListener('DOMContentLoaded', function() {
    console.log('Products page loaded');
    
    // تهيئة الصفحة
    initializePage();
    
    // ربط الأحداث
    bindEvents();
});

// تهيئة الصفحة
function initializePage() {
    console.log('Initializing Products page...');
    
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
    const dataTable = document.querySelector('#productsTable');
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
    // ربط نموذج إضافة/تعديل المنتج
    const productForm = document.getElementById('productForm');
    if (productForm) {
        productForm.addEventListener('submit', handleProductSubmit);
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
    document.querySelectorAll('.delete-product-btn').forEach(button => {
        button.addEventListener('click', handleDeleteClick);
    });
}

// ربط أزرار التعديل
function bindEditButtons() {
    document.querySelectorAll('.edit-product-btn').forEach(button => {
        button.addEventListener('click', handleEditClick);
    });
}

// ربط أزرار العرض
function bindViewButtons() {
    document.querySelectorAll('.view-product-btn').forEach(button => {
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

// معالجة إرسال نموذج المنتج
function handleProductSubmit(event) {
    event.preventDefault();
    event.stopPropagation();
    
    console.log('Submitting product form...');
    
    const form = event.target;
    const formData = new FormData(form);
    
    // التحقق من صحة البيانات
    if (!validateProductForm(formData)) {
        return;
    }
    
    // إرسال البيانات
    submitProductForm(formData);
}

// التحقق من صحة نموذج المنتج
function validateProductForm(formData) {
    const name = formData.get('Name');
    const retailPrice = formData.get('RetailPrice');
    const wholesalePrice = formData.get('WholesalePrice');
    
    if (!name || name.trim() === '') {
        alert('يرجى إدخال اسم المنتج');
        return false;
    }
    
    if (!retailPrice || parseFloat(retailPrice) <= 0) {
        alert('يرجى إدخال سعر البيع الصحيح');
        return false;
    }
    
    if (!wholesalePrice || parseFloat(wholesalePrice) <= 0) {
        alert('يرجى إدخال سعر الجملة الصحيح');
        return false;
    }
    
    return true;
}

// إرسال نموذج المنتج
function submitProductForm(formData) {
    const url = formData.get('ID') ? '/Products/Edit' : '/Products/Create';
    
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
            alert('تم حفظ المنتج بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حفظ المنتج: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error submitting product form:', error);
        alert('حدث خطأ أثناء حفظ المنتج');
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
    
    fetch(`/Products/Search?searchTerm=${encodeURIComponent(searchTerm)}`)
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
    
    // إعادة تحميل الصفحة لعرض جميع المنتجات
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
    
    const button = event.target.closest('.delete-product-btn');
    if (!button) return;
    
    const productId = button.getAttribute('data-product-id');
    const productName = button.getAttribute('data-product-name');
    
    if (confirm(`هل أنت متأكد من حذف المنتج "${productName}"؟`)) {
        deleteProduct(productId);
    }
}

// معالجة النقر على تعديل
function handleEditClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.edit-product-btn');
    if (!button) return;
    
    const productId = button.getAttribute('data-product-id');
    
    // الانتقال إلى صفحة التعديل
    window.location.href = `/Products/Edit/${productId}`;
}

// معالجة النقر على عرض
function handleViewClick(event) {
    event.preventDefault();
    event.stopPropagation();
    
    const button = event.target.closest('.view-product-btn');
    if (!button) return;
    
    const productId = button.getAttribute('data-product-id');
    
    // الانتقال إلى صفحة التفاصيل
    window.location.href = `/Products/Details/${productId}`;
}

// حذف المنتج
function deleteProduct(productId) {
    console.log('Deleting product:', productId);
    
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    const tokenValue = token ? token.value : '';
    
    fetch(`/Products/Delete/${productId}`, {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': tokenValue
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            alert('تم حذف المنتج بنجاح');
            window.location.reload();
        } else {
            alert('فشل في حذف المنتج: ' + (data.message || 'خطأ غير معروف'));
        }
    })
    .catch(error => {
        console.error('Error deleting product:', error);
        alert('حدث خطأ أثناء حذف المنتج');
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

// البحث عن المنتج بالاسم (للاستخدام في صفحات أخرى)
function searchProductByName(searchTerm) {
    return fetch(`/Products/SearchProductByName?term=${encodeURIComponent(searchTerm)}`)
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        });
}

// تصدير الدوال للاستخدام العام
window.Products = {
    initializePage,
    bindEvents,
    handleProductSubmit,
    validateProductForm,
    submitProductForm,
    handleSearch,
    performSearch,
    clearSearch,
    updateSearchResults,
    handleDeleteClick,
    handleEditClick,
    handleViewClick,
    deleteProduct,
    searchProductByName,
    showLoadingIndicator,
    hideLoadingIndicator
};
