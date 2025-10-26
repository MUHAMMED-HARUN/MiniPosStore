//// Import Order Management
//// JavaScript for managing import orders

//// تهيئة الصفحة
//$(document).ready(function() {
//    console.log('Import Order page loaded');

//    // تهيئة الصفحة
//    initializePage();

//    // ربط الأحداث
//    bindEvents();

//    // تحميل البيانات الأولية
//    loadInitialData();
//});

//// تهيئة الصفحة
//function initializePage() {
//    console.log('Initializing Import Order page...');

//    // تحميل العناصر عند فتح الصفحة
//    refreshItemsList();

//    // تهيئة معلومات المورد
//    initializeSupplierInfo();
//}

//// ربط الأحداث
//function bindEvents() {
//    // ربط تغيير المورد
//    $('#SupplierID').on('change', handleSupplierChange);

//    // ربط أحداث النماذج
//    bindFormEvents();
//}

//// ربط أحداث النماذج
//function bindFormEvents() {
//    // ربط نموذج أمر الاستيراد الرئيسي
//    $('#importOrderForm').on('submit', handleImportOrderSubmit);
//}

//// تهيئة معلومات المورد
//function initializeSupplierInfo() {
//    const supplierId = $('#SupplierID').val();
//    if (supplierId) {
//        updateSupplierInfo(supplierId);
//    }
//}

//// معالجة تغيير المورد
//function handleSupplierChange(event) {
//    const supplierId = $(event.target).val();
//    if (supplierId) {
//        updateSupplierInfo(supplierId);
//    } else {
//        clearSupplierInfo();
//    }
//}

//// تحديث معلومات المورد
//function updateSupplierInfo(supplierId) {
//    console.log('Updating supplier info for ID:', supplierId);

//    // يمكن إضافة AJAX call هنا لجلب معلومات المورد
//    // $.get(`/Suppliers/GetInfo/${supplierId}`, function(data) {
//    //     // تحديث معلومات المورد في الصفحة
//    // });
//}

//// مسح معلومات المورد
//function clearSupplierInfo() {
//    $('#supplierInfo').html('<p class="text-muted">اختر مورداً لعرض المعلومات</p>');
//}

//// معالجة إرسال نموذج أمر الاستيراد
//function handleImportOrderSubmit(event) {
//    event.preventDefault();
//    event.stopPropagation();

//    console.log('Submitting import order form...');

//    const form = event.target;
//    const formData = new FormData(form);

//    // التحقق من صحة البيانات
//    if (!validateImportOrderForm(formData)) {
//        return;
//    }

//    // إرسال البيانات
//    submitImportOrderForm(formData);
//}

//// التحقق من صحة نموذج أمر الاستيراد
//function validateImportOrderForm(formData) {
//    const supplierId = formData.get('SupplierID');
//    const importDate = formData.get('ImportDate');

//    if (!supplierId || supplierId === '') {
//        alert('يرجى اختيار المورد');
//        return false;
//    }

//    if (!importDate || importDate.trim() === '') {
//        alert('يرجى إدخال تاريخ الاستيراد');
//        return false;
//    }

//    return true;
//}

//// إرسال نموذج أمر الاستيراد
//function submitImportOrderForm(formData) {
//    const url = formData.get('ImportOrderID') ? '/ImportOrders/Edit' : '/ImportOrders/Create';

//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function(data) {
//            if (data.success) {
//                alert('تم حفظ أمر الاستيراد بنجاح');
//                if (data.redirectUrl) {
//                    window.location.href = data.redirectUrl;
//                }
//            } else {
//                alert('فشل في حفظ أمر الاستيراد: ' + (data.message || 'خطأ غير معروف'));
//            }
//        },
//        error: function(xhr, status, error) {
//            console.error('Error submitting import order form:', error);
//            alert('حدث خطأ أثناء حفظ أمر الاستيراد');
//        }
//    });
//}

//// تحميل البيانات الأولية
//function loadInitialData() {
//    const importOrderId = getImportOrderId();
//    if (importOrderId > 0) {
//        // تحميل قائمة العناصر
//        refreshItemsList();

//        // حساب المبلغ الإجمالي
//        calculateTotalAmount();
//    }
//}

//// إعادة تحميل قائمة العناصر
//function refreshItemsList() {
//    const importOrderId = getImportOrderId();
//    if (!importOrderId) return;

//    console.log('Refreshing items list for import order:', importOrderId);

//    $.get(`/ImportOrders/GetImportOrderItems?importOrderId=${importOrderId}`, function(html) {
//        const itemsContainer = $('#importOrderItems');
//        if (itemsContainer.length) {
//            itemsContainer.html(html);

//            // إعادة ربط الأحداث للعناصر الجديدة
//            if (window.ImportOrderUnionItems) {
//                window.ImportOrderUnionItems.rebindEvents();
//                window.ImportOrderUnionItems.bindFormEvents();

//                // حساب المبلغ الإجمالي
//                window.ImportOrderUnionItems.calculateTotalAmount();
//            }

//            // إعادة ربط أحداث النماذج
//            if (window.ImportOrderEdit) {
//                window.ImportOrderEdit.bindFormEvents();
//            }

//            console.log('Items list refreshed successfully');
//        }
//    }).fail(function(xhr, status, error) {
//        console.error('Error refreshing items list:', error);
//    });
//}

//// تحديث الإجماليات
//function updateTotals() {
//    // يمكن إضافة منطق تحديث الإجماليات هنا
//    calculateTotalAmount();
//}

//// حساب المبلغ الإجمالي
//function calculateTotalAmount() {
//    let total = 0;

//    // جمع أسعار الشراء لجميع العناصر
//    $('.union-item-row').each(function() {
//        const quantity = parseFloat($(this).find('.quantity').text().replace(/,/g, '')) || 0;
//        const purchasePrice = parseFloat($(this).find('.purchase-price').text().replace(/,/g, '')) || 0;
//        total += quantity * purchasePrice;
//    });

//    // تحديث حقل المبلغ الإجمالي
//    $('#TotalAmount').val(total.toFixed(2));

//    // تحديث عرض المبلغ الإجمالي
//    $('#totalAmount').text(total.toFixed(2));

//    console.log('Total amount calculated:', total);
//}

//// دالة مساعدة للحصول على ImportOrderId
//function getImportOrderId() {
//    // محاولة الحصول على ID من ViewBag أو data attribute
//    const idFromAttr = $('[data-import-order-id]').attr('data-import-order-id');
//    if (idFromAttr) {
//        return parseInt(idFromAttr);
//    }

//    // إذا لم يتم العثور عليه، حاول من حقل مخفي في النموذج الرئيسي
//    const idFromForm = $('#importOrderForm input[name="ImportOrderID"]').val();
//    if (idFromForm) {
//        return parseInt(idFromForm);
//    }

//    // قيمة افتراضية
//    return 0;
//}

//// تصدير الدوال للاستخدام العام
//window.ImportOrder = {
//    initializePage,
//    bindEvents,
//    handleSupplierChange,
//    updateSupplierInfo,
//    clearSupplierInfo,
//    handleImportOrderSubmit,
//    validateImportOrderForm,
//    submitImportOrderForm,
//    loadInitialData,
//    refreshItemsList,
//    updateTotals,
//    calculateTotalAmount,
//    getImportOrderId
//};


// 🧩 Import Order Management
// JavaScript for managing import orders

$(document).ready(function () {
    console.log("✅ Import Order page loaded");

    initializePage();
    bindEvents();
    loadInitialData();
});

// 🧱 تهيئة الصفحة
function initializePage() {
    console.log("Initializing Import Order page...");
    refreshItemsList();
    initializeSupplierInfo();
}

// 🔗 ربط الأحداث العامة
function bindEvents() {
    // عند تغيير المورد
    $("#SupplierID").off("change").on("change", handleSupplierChange);

    // عند إرسال نموذج أمر الاستيراد
    $("#importOrderForm").off("submit").on("submit", function (e) {
        e.preventDefault();
        e.stopPropagation();
        console.log("🟢 تم الضغط على زر حفظ أمر الاستيراد");
        handleImportOrderSubmit(e);
    });
}

// 🧾 تهيئة معلومات المورد
function initializeSupplierInfo() {
    const supplierId = $("#SupplierID").val();
    if (supplierId) updateSupplierInfo(supplierId);
}

// 🧩 عند تغيير المورد
function handleSupplierChange(e) {
    const supplierId = $(e.target).val();
    if (supplierId) updateSupplierInfo(supplierId);
    else clearSupplierInfo();
}

// 🧭 تحديث معلومات المورد
function updateSupplierInfo(supplierId) {
    console.log("🔍 تحديث معلومات المورد ID:", supplierId);

    $.get(`/Suppliers/GetInfo/${supplierId}`)
        .done(function (data) {
            if (data && data.success) {
                $("#supplierInfo").html(`
                    <p><strong>اسم المتجر:</strong> ${data.name}</p>
                    <p><strong>الهاتف:</strong> ${data.phone}</p>
                    <p><strong>العنوان:</strong> ${data.address}</p>
                `);
            } else {
                clearSupplierInfo();
            }
        })
        .fail(() => {
            console.warn("⚠️ خطأ أثناء جلب معلومات المورد");
            clearSupplierInfo();
        });
}

// 🧹 مسح معلومات المورد
function clearSupplierInfo() {
    $("#supplierInfo").html('<p class="text-muted">اختر مورداً لعرض المعلومات</p>');
}

// 🧮 معالجة إرسال نموذج أمر الاستيراد
function handleImportOrderSubmit(event) {
    console.log("🚀 إرسال نموذج أمر الاستيراد...");
    const form = $("#importOrderForm")[0];
    const formData = new FormData(form);

    if (!validateImportOrderForm(formData)) {
        console.warn("❌ فشل التحقق من صحة البيانات");
        return;
    }

    submitImportOrderForm(formData);
}

// ✅ التحقق من صحة النموذج
function validateImportOrderForm(formData) {
    const supplierId = formData.get("SupplierID");
    const importDate = formData.get("ImportDate");

    if (!supplierId || supplierId === "") {
        alert("يرجى اختيار المورد");
        return false;
    }

    if (!importDate || importDate.trim() === "") {
        alert("يرجى إدخال تاريخ الاستيراد");
        return false;
    }

    return true;
}

// 📤 إرسال نموذج أمر الاستيراد
function submitImportOrderForm(formData) {
    const isEdit = formData.get("ImportOrderID") && formData.get("ImportOrderID") !== "0";
    const url = isEdit ? "/ImportOrders/Edit" : "/ImportOrders/Create";

    $.ajax({
        url: url,
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                alert("✅ تم حفظ أمر الاستيراد بنجاح");
                if (data.redirectUrl) window.location.href = data.redirectUrl;
            } else {
                alert("❌ فشل في حفظ أمر الاستيراد: " + (data.message || "خطأ غير معروف"));
            }
        },
        error: function (xhr) {
            console.error("⚠️ خطأ أثناء إرسال أمر الاستيراد:", xhr.responseText);
            alert("حدث خطأ أثناء حفظ أمر الاستيراد");
        },
    });
}

// 🔁 تحميل البيانات الأولية
function loadInitialData() {
    const importOrderId = getImportOrderId();
    if (importOrderId > 0) {
        refreshItemsList();
        calculateTotalAmount();
    }
}

// 🔄 إعادة تحميل العناصر
function refreshItemsList() {
    const importOrderId = getImportOrderId();
    if (!importOrderId) return;

    console.log("🔁 تحديث عناصر أمر الاستيراد:", importOrderId);

    $.get(`/ImportOrders/GetImportOrderItems?importOrderId=${importOrderId}`)
        .done(function (html) {
            const itemsContainer = $("#importOrderItems");
            itemsContainer.html(html);

            // ربط الأحداث للعناصر الجديدة
            if (window.ImportOrderUnionItems) {
                window.ImportOrderUnionItems.rebindEvents();
                window.ImportOrderUnionItems.bindFormEvents();
                window.ImportOrderUnionItems.calculateTotalAmount();
            }

            // إعادة ربط الأحداث للنماذج
            if (window.ImportOrderEdit) {
                window.ImportOrderEdit.bindFormEvents();
            }

            console.log("✅ تم تحديث العناصر بنجاح");
        })
        .fail(() => console.error("⚠️ خطأ أثناء تحميل العناصر"));
}

// 💰 حساب المبلغ الإجمالي
function calculateTotalAmount() {
    let total = 0;

    $(".union-item-row").each(function () {
        const qty = parseFloat($(this).find(".quantity").text().replace(/,/g, "")) || 0;
        const price = parseFloat($(this).find(".purchase-price").text().replace(/,/g, "")) || 0;
        total += qty * price;
    });

    $("#TotalAmount").val(total.toFixed(2));
    $("#totalAmount").text(total.toFixed(2));
    console.log("💰 Total amount:", total.toFixed(2));
}

// 🔎 جلب رقم أمر الاستيراد الحالي
function getImportOrderId() {
    const idAttr = $("[data-import-order-id]").attr("data-import-order-id");
    if (idAttr) return parseInt(idAttr);
    const idFromForm = $("#importOrderForm input[name='ImportOrderID']").val();
    if (idFromForm) return parseInt(idFromForm);
    return 0;
}

// 🌍 تصدير الدوال العامة
window.ImportOrder = {
    initializePage,
    bindEvents,
    handleSupplierChange,
    updateSupplierInfo,
    clearSupplierInfo,
    handleImportOrderSubmit,
    validateImportOrderForm,
    submitImportOrderForm,
    loadInitialData,
    refreshItemsList,
    calculateTotalAmount,
    getImportOrderId,
};
