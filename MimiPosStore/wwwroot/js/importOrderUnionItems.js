// Import Order Union Items Management
// JavaScript for handling edit and delete operations on union items

// تهيئة الصفحة
$(document).ready(function() {
    console.log('Import Order Union Items loaded');
    
    // ربط أحداث الأزرار بعد تحميل الصفحة
    bindEventListeners();
    
    // ربط أحداث النماذج
    bindFormEvents();
});

function bindEventListeners() {
    // ربط أحداث التعديل والحذف
    $('.edit-union-item-btn').off('click').on('click', handleEditClick);
    $('.delete-union-item-btn').off('click').on('click', handleDeleteClick);
}

// ربط أحداث النماذج
function bindFormEvents() {
    // ربط أحداث النماذج
    $(document).off('submit').on('submit', handleFormSubmit);
}

// معالجة إرسال النماذج
function handleFormSubmit(event) {
    event.preventDefault(); // منع السلوك الافتراضي
    event.stopPropagation(); // منع انتشار الحدث
    
    console.log('Form submitted:', event.target.id);
    
    if (event.target.id === 'editItemForm') {
        handleUpdateItem(event);
    } else if (event.target.id === 'editRawMaterialItemForm') {
        handleUpdateRawMaterialItem(event);
    }
}

// 🖋 عند الضغط على تعديل → فتح Modal للتعديل
function handleEditClick(event) {
    event.preventDefault(); // منع السلوك الافتراضي
    event.stopPropagation(); // منع انتشار الحدث
    
    const button = $(event.target).closest('.edit-union-item-btn');
    if (!button.length) return;
    
    const itemId = button.attr('data-item-id');
    const itemType = parseInt(button.attr('data-item-type'));
    const importOrderId = getImportOrderId();

    console.log('Edit button clicked:', { itemId, itemType, importOrderId });

    // فتح Modal للتعديل
    editUnionItem(itemId, itemType, importOrderId);
}

// 🗑 عند الضغط على حذف
function handleDeleteClick(event) {
    event.preventDefault(); // منع السلوك الافتراضي
    event.stopPropagation(); // منع انتشار الحدث
    
    const button = $(event.target).closest('.delete-union-item-btn');
    if (!button.length) return;
    
    const itemId = button.attr('data-item-id');
    const itemType = button.attr('data-item-type');
    const importOrderId = getImportOrderId();

    console.log('Delete button clicked:', { itemId, itemType, importOrderId });

    if (confirm('هل أنت متأكد من حذف هذا العنصر؟')) {
        deleteUnionItem(itemId, itemType, importOrderId);
    }
}

// دالة تعديل العنصر - فتح Modal
function editUnionItem(itemId, itemType, importOrderId) {
    console.log('Loading item for edit:', { itemId, itemType, importOrderId });
    
    $.ajax({
        url: '/ImportOrders/GetUnionItemForEdit',
        type: 'GET',
        data: { itemId: itemId, itemType: itemType },
        timeout: 10000,
        success: function(response) {
            console.log('Response received:', response);
            
            let editModalContent, editModal;
            
            if (itemType === 1) {
                // منتج
                editModalContent = $('#editItemModalContent');
                editModal = $('#editItemModal');
            } else if (itemType === 2) {
                // مادة خام
                editModalContent = $('#editRawMaterialItemModalContent');
                editModal = $('#editRawMaterialItemModal');
            }
            
            if (editModalContent.length && editModal.length) {
                // التحقق من أن المحتوى تم تحميله بشكل صحيح
                if (response && response.trim() !== '') {
                    editModalContent.html(response);
                    
                    // إعادة ربط أحداث النماذج بعد تحميل المحتوى
                    bindFormEvents();
                    
                    // إظهار Modal باستخدام Bootstrap
                    editModal.modal('show');
                    
                    console.log('Modal opened successfully with content');
                } else {
                    console.error('Empty response received');
                    alert('لم يتم تحميل البيانات بشكل صحيح');
                }
            } else {
                console.error('Modal elements not found for item type:', itemType);
                console.error('editModalContent:', editModalContent.length);
                console.error('editModal:', editModal.length);
                alert('خطأ في العثور على عناصر النافذة المنبثقة');
            }
        },
        error: function(xhr, status, error) {
            console.error('Error loading item for edit:', status, error);
            console.error('Response:', xhr.responseText);
            
            // محاولة تحليل الاستجابة كـ JSON
            try {
                const response = JSON.parse(xhr.responseText);
                if (response.message) {
                    alert('خطأ: ' + response.message);
                } else {
                    alert('خطأ في تحميل بيانات العنصر للتعديل: ' + xhr.status);
                }
            } catch (e) {
                alert('خطأ في تحميل بيانات العنصر للتعديل: ' + xhr.status);
            }
        }
    });
}

// حذف العنصر باستخدام Ajax
function deleteUnionItem(itemId, itemType, importOrderId) {
    console.log('Deleting item:', { itemId, itemType, importOrderId });
    
    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/ImportOrders/DeleteUnionItem',
        type: 'POST',
        data: {
            itemId: itemId,
            itemType: itemType,
            importOrderId: importOrderId
        },
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token
        },
        success: function(response) {
            if (response.success) {
                alert('تم حذف العنصر بنجاح');
                // إعادة تحميل القائمة بدلاً من الصفحة كاملة
                refreshUnionItemsList(importOrderId);
                
                // حساب المبلغ الإجمالي
                calculateTotalAmount();
            } else {
                alert('فشل في حذف العنصر: ' + (response.message || 'خطأ غير معروف'));
            }
        },
        error: function(xhr, status, error) {
            console.error('Error deleting item:', status, error);
            alert('حدث خطأ أثناء حذف العنصر.');
        }
    });
}

// معالجة تحديث المنتج
function handleUpdateItem(event) {
    event.preventDefault(); // منع السلوك الافتراضي
    event.stopPropagation(); // منع انتشار الحدث
    
    const form = event.target;
    const formData = new FormData(form);
    const token = $('input[name="__RequestVerificationToken"]').val();

    console.log('Updating product item:', formData);

    $.ajax({
        url: '/ImportOrders/UpdateItem',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token
        },
        success: function(response) {
            if (response.success) {
                // إغلاق Modal
                $('#editItemModal').modal('hide');
                
                // إعادة تحميل القائمة
                refreshUnionItemsList(getImportOrderId());
                
                // حساب المبلغ الإجمالي
                calculateTotalAmount();
                
                alert('تم تحديث المنتج بنجاح');
            } else {
                alert('فشل في تحديث المنتج: ' + (response.message || 'خطأ غير معروف'));
            }
        },
        error: function(xhr, status, error) {
            console.error('Error updating item:', status, error);
            alert('حدث خطأ أثناء تحديث المنتج');
        }
    });
}

// معالجة تحديث المادة الخام
function handleUpdateRawMaterialItem(event) {
    event.preventDefault(); // منع السلوك الافتراضي
    event.stopPropagation(); // منع انتشار الحدث
    
    const form = event.target;
    const formData = new FormData(form);
    const token = $('input[name="__RequestVerificationToken"]').val();

    console.log('Updating raw material item:', formData);

    $.ajax({
        url: '/ImportOrders/UpdateRawMaterialItem',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token
        },
        success: function(response) {
            if (response.success) {
                // إغلاق Modal
                $('#editRawMaterialItemModal').modal('hide');
                
                // إعادة تحميل القائمة
                refreshUnionItemsList(getImportOrderId());
                
                // حساب المبلغ الإجمالي
                calculateTotalAmount();
                
                alert('تم تحديث المادة الخام بنجاح');
            } else {
                alert('فشل في تحديث المادة الخام: ' + (response.message || 'خطأ غير معروف'));
            }
        },
        error: function(xhr, status, error) {
            console.error('Error updating raw material item:', status, error);
            alert('حدث خطأ أثناء تحديث المادة الخام');
        }
    });
}

// دالة مساعدة للحصول على ImportOrderId
function getImportOrderId() {
    // محاولة الحصول على ID من ViewBag أو data attribute
    const idFromAttr = $('[data-import-order-id]').attr('data-import-order-id');
    if (idFromAttr) {
        return idFromAttr;
    }
    
    // محاولة الحصول من hidden input
    const idFromInput = $('input[name="ImportOrderID"]').val();
    if (idFromInput) {
        return idFromInput;
    }
    
    // محاولة الحصول من URL
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    if (id) {
        return id;
    }
    
    // قيمة افتراضية
    return 0;
}

// دالة لإعادة ربط الأحداث بعد تحديث المحتوى
function rebindEvents() {
    // ربط الأحداث مرة أخرى
    bindEventListeners();
    bindFormEvents();
}

// دالة لإعادة تحميل قائمة العناصر
function refreshUnionItemsList(importOrderId) {
    console.log('Refreshing union items list for import order:', importOrderId);
    
    $.get(`/ImportOrders/GetImportOrderUnionItems?importOrderId=${importOrderId}`, function(response) {
        const itemsContainer = $('#importOrderItems');
        if (itemsContainer.length) {
            itemsContainer.html(response);
            // إعادة ربط الأحداث للعناصر الجديدة
            rebindEvents();
            
            // إعادة ربط أحداث النماذج
            if (window.ImportOrderEdit) {
                window.ImportOrderEdit.bindFormEvents();
            }
            
            // حساب المبلغ الإجمالي
            calculateTotalAmount();
            
            console.log('Union items list refreshed successfully');
        } else {
            console.error('Items container not found');
        }
    }).fail(function(xhr, status, error) {
        console.error('Error refreshing items list:', status, error);
    });
}

// دالة حساب المبلغ الإجمالي
function calculateTotalAmount() {
    let total = 0;
    
    // جمع أسعار الشراء لجميع العناصر
    $('.union-item-row').each(function() {
        const quantity = parseFloat($(this).find('.quantity').text().replace(/,/g, '')) || 0;
        const purchasePrice = parseFloat($(this).find('.purchase-price').text().replace(/,/g, '')) || 0;
        total += quantity * purchasePrice;
    });
    
    // تحديث حقل المبلغ الإجمالي
    $('#TotalAmount').val(total.toFixed(2));
    
    // تحديث عرض المبلغ الإجمالي
    $('#totalAmount').text(total.toFixed(2));
    
    // إرسال المبلغ إلى الخادم
    updateImportOrderTotal(total);
    
    console.log('Total amount calculated:', total);
}

// دالة إرسال المبلغ الإجمالي إلى الخادم
function updateImportOrderTotal(total) {
    const importOrderId = getImportOrderId();
    if (!importOrderId) return;
    
    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/ImportOrders/UpdateTotalAmount',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            importOrderId: importOrderId,
            totalAmount: total
        }),
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token
        },
        success: function(data) {
            if (data.success) {
                console.log('Total amount updated successfully');
            } else {
                console.error('Failed to update total amount:', data.message);
            }
        },
        error: function(xhr, status, error) {
            console.error('Error updating total amount:', error);
        }
    });
}

// تصدير الدوال للاستخدام العام
window.ImportOrderUnionItems = {
    bindEventListeners,
    bindFormEvents,
    handleFormSubmit,
    handleEditClick,
    handleDeleteClick,
    editUnionItem,
    deleteUnionItem,
    handleUpdateItem,
    handleUpdateRawMaterialItem,
    refreshUnionItemsList,
    rebindEvents,
    getImportOrderId,
    calculateTotalAmount,
    updateImportOrderTotal
};
