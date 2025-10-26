//// Import Order Edit Page JavaScript
//// إدارة صفحة تعديل أمر الاستيراد

//$(document).ready(function() {
//    console.log('Import Order Edit page loaded');

//    // تهيئة الصفحة
//    initializePage();

//    // ربط الأحداث
//    bindEvents();

//    // تحميل البيانات الأولية
//    loadInitialData();

//    // إعادة ربط الأحداث بعد تحميل النماذج
//    setTimeout(function() {
//        console.log('Re-binding events after modals are loaded...');
//        bindEvents();
//    }, 500);
//});

//// تهيئة الصفحة
//function initializePage() {
//    console.log('Initializing Import Order Edit page...');

//    // فحص وجود الـ Modals
//    const addItemModal = $('#addItemModal');
//    const addRawMaterialModal = $('#addRawMaterialItemModal');

//    console.log('addItemModal:', addItemModal.length);
//    console.log('addRawMaterialModal:', addRawMaterialModal.length);

//    // فحص وجود الأزرار
//    const addProductBtn = $('#addProductBtn');
//    const addRawMaterialBtn = $('#addRawMaterialBtn');

//    console.log('addProductBtn:', addProductBtn.length);
//    console.log('addRawMaterialBtn:', addRawMaterialBtn.length);
//}

//// ربط الأحداث
//function bindEvents() {
//    // ربط أزرار الـ Modals
//    bindModalButtons();

//    // ربط أحداث النماذج
//    bindFormEvents();

//    // ربط أحداث البحث
//    bindSearchEvents();
//}

//// ربط أزرار الـ Modals
//function bindModalButtons() {
//    $('#addProductBtn').off('click').on('click', function() {
//        console.log('Add product button clicked');
//        openAddProductModal();
//    });

//    $('#addRawMaterialBtn').off('click').on('click', function() {
//        console.log('Add raw material button clicked');
//        openAddRawMaterialModal();
//    });
//}

//// فتح Modal إضافة المنتج
//function openAddProductModal() {
//    const modal = $('#addItemModal');
//    if (modal.length) {
//        modal.on('shown.bs.modal', function() {
//            console.log('🟢 AddItemModal ظهر الآن');
//            initializeAddProductForm();

//            // إعادة ربط أحداث النموذج
//            bindFormEvents();
//        });

//        modal.modal('show');
//    } else {
//        console.error('Add product modal not found');
//    }
//}

//// فتح Modal إضافة المادة الخام
//function openAddRawMaterialModal() {
//    const modal = $('#addRawMaterialItemModal');
//    if (modal.length) {
//        modal.on('shown.bs.modal', function() {
//            console.log('🟢 AddRawMaterialModal ظهر الآن');
//            initializeAddRawMaterialForm();

//            // إعادة ربط أحداث النموذج
//            bindFormEvents();
//        });

//        modal.modal('show');
//    } else {
//        console.error('Add raw material modal not found');
//    }
//}

//// تهيئة نموذج إضافة المنتج
//function initializeAddProductForm() {
//    console.log('Initializing add product form...');

//    // تعيين ImportOrderID
//    const importOrderId = getImportOrderId();
//    $('#importOrderIdInput').val(importOrderId);

//    // ربط أحداث البحث
//    bindProductSearchEvents();

//    // ربط أحداث الحساب
//    bindProductCalculationEvents();

//    // إعادة ربط أحداث النموذج
//    console.log('Re-binding form events in initializeAddProductForm...');
//    bindFormEvents();
//}

//// تهيئة نموذج إضافة المادة الخام
//function initializeAddRawMaterialForm() {
//    console.log('Initializing add raw material form...');

//    // تعيين ImportOrderID
//    const importOrderId = getImportOrderId();
//    $('#addRawMaterialImportOrderIdInput').val(importOrderId);

//    // ربط أحداث البحث
//    bindRawMaterialSearchEvents();

//    // ربط أحداث الحساب
//    bindRawMaterialCalculationEvents();

//    // إعادة ربط أحداث النموذج
//    console.log('Re-binding form events in initializeAddRawMaterialForm...');
//    bindFormEvents();
//}

//// ربط أحداث البحث عن المنتجات
//function bindProductSearchEvents() {
//    console.log('Binding product search events...');
//    const searchBtn = $('#btnSearchProduct');
//    console.log('Search button found:', searchBtn.length);

//    if (searchBtn.length) {
//        searchBtn.off('click').on('click', function() {
//            console.log('🔍 Search button clicked');
//            searchProduct();
//        });
//        console.log('✅ Product search events bound');
//    } else {
//        console.error('❌ Search button not found');
//    }
//}

//// ربط أحداث البحث عن المواد الخام
//function bindRawMaterialSearchEvents() {
//    console.log('Binding raw material search events...');
//    const searchBtn = $('#btnSearchRawMaterial');
//    console.log('Raw material search button found:', searchBtn.length);

//    if (searchBtn.length) {
//        searchBtn.off('click').on('click', function() {
//            console.log('🔍 Raw material search button clicked');
//            searchRawMaterial();
//        });
//        console.log('✅ Raw material search events bound');
//    } else {
//        console.error('❌ Raw material search button not found');
//    }
//}

//// البحث عن المنتج
//function searchProduct() {
//    const searchTerm = $('#editProductSearch').val().trim();
//    if (!searchTerm) {
//        alert('يرجى إدخال اسم المنتج');
//        return;
//    }

//    console.log('Searching for product:', searchTerm);

//    $.ajax({
//        url: '/Products/SearchProductByNmae',
//        type: 'GET',
//        data: { term: searchTerm },
//        dataType: 'json',
//        success: function(data) {
//            if (data.success) {
//                // ملء عناصر الـ form
//                $('#editProductID').val(data.id);
//                $('#editProductSearch').val(data.name);
//                $('#sellingPriceInput').val(data.retailPrice);

//                // إخفاء نتائج البحث
//                $('#searchResults').empty();

//                // تحميل معلومات المنتج
//                loadProductInfo(data.id);
//            } else {
//                $('#searchResults').html('<div class="list-group-item">لا توجد نتائج</div>');
//            }
//        },
//        error: function() {
//            console.error('Error searching for product');
//            alert('حدث خطأ أثناء البحث');
//        }
//    });
//}

//// البحث عن المادة الخام
//function searchRawMaterial() {
//    const searchTerm = $('#addRawMaterialSearch').val().trim();
//    if (!searchTerm) {
//        alert('يرجى إدخال اسم المادة الخام');
//        return;
//    }

//    console.log('Searching for raw material:', searchTerm);

//    $.ajax({
//        url: '/ImportOrders/SearchRawMaterials',
//        type: 'GET',
//        data: { searchTerm: searchTerm },
//        dataType: 'json',
//        success: function(data) {
//            if (data && data.length > 0) {
//                // أخذ أول نتيجة وملء الـ form مباشرة
//                const firstItem = data[0];

//                // ملء عناصر الـ form
//                $('#addRawMaterialID').val(firstItem.id);
//                $('#addRawMaterialSearch').val(firstItem.name);
//                $('#addRawMaterialSellingPriceInput').val(firstItem.purchasePrice);

//                // تعيين الكمية الافتراضية
//                $('#addRawMaterialQuantityInput').val(1);

//                // إخفاء نتائج البحث
//                $('#rawMaterialSearchResults').empty();

//                // حساب الإجمالي
//                calculateRawMaterialTotal();
//            } else {
//                $('#rawMaterialSearchResults').html('<div class="list-group-item text-muted">لا توجد نتائج</div>');
//            }
//        },
//        error: function() {
//            console.error('Error searching for raw material');
//            alert('حدث خطأ أثناء البحث');
//        }
//    });
//}

//// تحميل معلومات المنتج
//function loadProductInfo(productId) {
//    $.ajax({
//        url: '/ImportOrders/GetProductInfo',
//        type: 'GET',
//        data: { productId: productId },
//        dataType: 'json',
//        success: function(data) {
//            if (data.success) {
//                // تحديث معلومات المنتج
//                $('#basePrice').text(data.retailPrice + ' ' + data.currencyType);
//                $('#availableQuantity').text(data.availableQuantity);
//                $('#currencyType').text(data.currencyType);
//                $('#unitOfMeasure').text(data.uomName);

//                // تعيين سعر البيع الافتراضي
//                $('#sellingPriceInput').val(data.retailPrice);

//                // حساب المبلغ الإجمالي
//                calculateItemTotal();
//            } else {
//                clearProductInfo();
//                alert('خطأ في تحميل معلومات المنتج: ' + data.message);
//            }
//        },
//        error: function() {
//            console.error('Error loading product info');
//            clearProductInfo();
//            alert('حدث خطأ في الاتصال بالخادم');
//        }
//    });
//}

//// مسح معلومات المنتج
//function clearProductInfo() {
//    $('#basePrice').text('-');
//    $('#availableQuantity').text('-');
//    $('#currencyType').text('-');
//    $('#unitOfMeasure').text('-');
//    $('#sellingPriceInput').val('');
//    $('#itemTotalAmount').val('');
//    $('#editProductID').val('');
//}

//// ربط أحداث الحساب للمنتج
//function bindProductCalculationEvents() {
//    console.log('Binding product calculation events...');
//    const quantityInput = $('#quantityInput');
//    const sellingPriceInput = $('#sellingPriceInput');

//    console.log('Quantity input found:', quantityInput.length);
//    console.log('Selling price input found:', sellingPriceInput.length);

//    if (quantityInput.length && sellingPriceInput.length) {
//        quantityInput.off('input').on('input', calculateItemTotal);
//        sellingPriceInput.off('input').on('input', calculateItemTotal);
//        console.log('✅ Product calculation events bound');
//    } else {
//        console.error('❌ Product calculation inputs not found');
//    }
//}

//// ربط أحداث الحساب للمادة الخام
//function bindRawMaterialCalculationEvents() {
//    $('#addRawMaterialQuantityInput, #addRawMaterialSellingPriceInput').off('input').on('input', calculateRawMaterialTotal);
//}

//// حساب المبلغ الإجمالي للمنتج
//function calculateItemTotal() {
//    const quantity = parseFloat($('#quantityInput').val()) || 0;
//    const sellingPrice = parseFloat($('#sellingPriceInput').val()) || 0;
//    const total = quantity * sellingPrice;

//    $('#itemTotalAmount').val(total.toFixed(2));
//}

//// حساب المبلغ الإجمالي للمادة الخام
//function calculateRawMaterialTotal() {
//    const quantity = parseFloat($('#addRawMaterialQuantityInput').val()) || 0;
//    const price = parseFloat($('#addRawMaterialSellingPriceInput').val()) || 0;
//    const total = quantity * price;

//    $('#addRawMaterialTotalInput').val(total.toFixed(2));
//}

//// ربط أحداث النماذج
//function bindFormEvents() {
//    console.log('Binding form events...');

//    // ربط نموذج إضافة المنتج
//    const addItemForm = $('#addItemForm');
//    console.log('addItemForm found:', addItemForm.length);

//    if (addItemForm.length) {
//        addItemForm.off('submit').on('submit', handleAddProductSubmit);
//        console.log('✅ Add item form event bound');
//    } else {
//        console.error('❌ Add item form not found');
//    }

//    // ربط نموذج إضافة المادة الخام
//    const addRawMaterialForm = $('#addRawMaterialItemForm');
//    console.log('addRawMaterialItemForm found:', addRawMaterialForm.length);

//    if (addRawMaterialForm.length) {
//        addRawMaterialForm.off('submit').on('submit', handleAddRawMaterialSubmit);
//        console.log('✅ Add raw material form event bound');
//    } else {
//        console.error('❌ Add raw material form not found');
//    }
//}

//// معالجة إرسال نموذج إضافة المنتج
//function handleAddProductSubmit(event) {
//    console.log('🚀 handleAddProductSubmit called!');
//    event.preventDefault();
//    event.stopPropagation();

//    console.log('Adding product to import order...');

//    const formData = new FormData(event.target);

//    // طباعة بيانات النموذج للتشخيص
//    for (let [key, value] of formData.entries()) {
//        console.log(`${key}: ${value}`);
//    }

//    $.ajax({
//        url: '/ImportOrders/AddItem',
//        type: 'POST',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function(data) {
//            console.log('✅ Add product success:', data);
//            if (data.success) {
//                // إغلاق Modal
//                $('#addItemModal').modal('hide');

//                // إعادة تحميل قائمة العناصر
//                refreshItemsList();

//                // حساب المبلغ الإجمالي
//                calculateTotalAmount();

//                alert('تم إضافة المنتج بنجاح');
//            } else {
//                alert('فشل في إضافة المنتج: ' + (data.message || 'خطأ غير معروف'));
//            }
//        },
//        error: function(xhr, status, error) {
//            console.error('❌ Error adding product:', status, error);
//            console.error('Response:', xhr.responseText);
//            alert('حدث خطأ أثناء إضافة المنتج');
//        }
//    });
//}

//// معالجة إرسال نموذج إضافة المادة الخام
//function handleAddRawMaterialSubmit(event) {
//    event.preventDefault();
//    event.stopPropagation();

//    console.log('Adding raw material to import order...');

//    const formData = new FormData(event.target);

//    $.ajax({
//        url: '/ImportOrders/AddRawMaterialItem',
//        type: 'POST',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function(data) {
//            if (data.success) {
//                // إغلاق Modal
//                $('#addRawMaterialItemModal').modal('hide');

//                // إعادة تحميل قائمة العناصر
//                refreshItemsList();

//                // حساب المبلغ الإجمالي
//                calculateTotalAmount();

//                alert('تم إضافة المادة الخام بنجاح');
//            } else {
//                alert('فشل في إضافة المادة الخام: ' + (data.message || 'خطأ غير معروف'));
//            }
//        },
//        error: function() {
//            console.error('Error adding raw material');
//            alert('حدث خطأ أثناء إضافة المادة الخام');
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

//    $.get('/ImportOrders/GetImportOrderUnionItems', { importOrderId: importOrderId }, function(html) {
//        $('#importOrderItems').html(html);

//        // إعادة ربط الأحداث للعناصر الجديدة
//        if (window.ImportOrderUnionItems) {
//            window.ImportOrderUnionItems.rebindEvents();
//        }

//        // إعادة ربط أحداث النماذج
//        bindFormEvents();

//        // حساب المبلغ الإجمالي
//        calculateTotalAmount();

//        console.log('Items list refreshed successfully');
//    }).fail(function() {
//        console.error('Error refreshing items list');
//    });
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

//    // إرسال المبلغ إلى الخادم
//    updateImportOrderTotal(total);

//    console.log('Total amount calculated:', total);
//}

//// إرسال المبلغ الإجمالي إلى الخادم
//function updateImportOrderTotal(total) {
//    const importOrderId = getImportOrderId();
//    if (!importOrderId) return;

//    $.ajax({
//        url: '/ImportOrders/UpdateTotalAmount',
//        type: 'POST',
//        contentType: 'application/json',
//        data: JSON.stringify({
//            importOrderId: importOrderId,
//            totalAmount: total
//        }),
//        success: function(data) {
//            if (data.success) {
//                console.log('Total amount updated successfully');
//            } else {
//                console.error('Failed to update total amount:', data.message);
//            }
//        },
//        error: function() {
//            console.error('Error updating total amount');
//        }
//    });
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
//window.ImportOrderEdit = {
//    initializePage,
//    bindEvents,
//    openAddProductModal,
//    openAddRawMaterialModal,
//    searchProduct,
//    searchRawMaterial,
//    loadProductInfo,
//    clearProductInfo,
//    calculateItemTotal,
//    calculateRawMaterialTotal,
//    handleAddProductSubmit,
//    handleAddRawMaterialSubmit,
//    refreshItemsList,
//    calculateTotalAmount,
//    updateImportOrderTotal,
//    getImportOrderId
//};




$(document).ready(function () {
    console.log("✅ Import Order Edit Page Loaded");

    // 🔹 ربط الأزرار الخاصة بالمودالات
    $("#addProductBtn").on("click", function () {
        console.log("🟢 فتح نافذة إضافة المنتج");
        $("#addItemModal").modal("show");
    });

    $("#addRawMaterialBtn").on("click", function () {
        console.log("🟣 فتح نافذة إضافة المادة الخام");
        $("#addRawMaterialItemModal").modal("show");
    });

    // 🔹 عند ظهور نافذة المنتج
    $("#addItemModal").on("shown.bs.modal", function () {
        console.log("🟢 AddItemModal ظهر الآن");

        // ربط النموذج
        $("#addItemForm").off("submit").on("submit", function (e) {
            e.preventDefault();
            submitProductForm();
        });

        // ربط زر البحث
        $("#btnSearchProduct").off("click").on("click", function () {
            searchProduct();
        });

        // الحساب التلقائي
        $("#quantityInput, #sellingPriceInput").off("input").on("input", calculateItemTotal);
    });

    // 🔹 عند ظهور نافذة المادة الخام
    $("#addRawMaterialItemModal").on("shown.bs.modal", function () {
        console.log("🟣 AddRawMaterialItemModal ظهر الآن");

        // ربط النموذج
        $("#addRawMaterialItemForm").off("submit").on("submit", function (e) {
            e.preventDefault();
            submitRawMaterialForm();
        });

        // زر البحث
        $("#btnSearchRawMaterial").off("click").on("click", function () {
            searchRawMaterial();
        });

        // الحساب التلقائي
        $("#addRawMaterialQuantityInput, #addRawMaterialSellingPriceInput").off("input").on("input", calculateRawMaterialTotal);
    });

    // 🧮 حساب الإجمالي للمنتج
    function calculateItemTotal() {
        const qty = parseFloat($("#quantityInput").val()) || 0;
        const price = parseFloat($("#sellingPriceInput").val()) || 0;
        $("#itemTotalAmount").val((qty * price).toFixed(2));
    }

    // 🧮 حساب الإجمالي للمادة الخام
    function calculateRawMaterialTotal() {
        const qty = parseFloat($("#addRawMaterialQuantityInput").val()) || 0;
        const price = parseFloat($("#addRawMaterialSellingPriceInput").val()) || 0;
        $("#addRawMaterialTotalInput").val((qty * price).toFixed(2));
    }

    // 🔍 البحث عن المنتج
    function searchProduct() {
        const term = $("#editProductSearch").val().trim();
        if (!term) return alert("يرجى إدخال اسم المنتج");

        console.log("🔍 البحث عن المنتج:", term);
        $.get("/Products/SearchProductByNmae", { term: term })
            .done(function (data) {
                if (data && data.success) {
                    $("#editProductID").val(data.id);
                    $("#sellingPriceInput").val(data.retailPrice);
                    $("#basePrice").text(`${data.retailPrice} ${data.currencyType}`);
                    $("#availableQuantity").text(data.availableQuantity);
                    $("#currencyType").text(data.currencyType);
                    $("#unitOfMeasure").text(data.uomName);
                    calculateItemTotal();
                } else {
                    alert("❌ لم يتم العثور على المنتج");
                }
            })
            .fail(() => alert("⚠️ حدث خطأ أثناء البحث عن المنتج"));
    }

    // 🔍 البحث عن المادة الخام
    function searchRawMaterial() {
        const term = $("#addRawMaterialSearch").val().trim();
        if (!term) return alert("يرجى إدخال اسم المادة الخام");

        console.log("🔍 البحث عن المادة الخام:", term);
        $.get("/ImportOrders/SearchRawMaterials", { searchTerm: term })
            .done(function (data) {
                if (data && data.length > 0) {
                    const first = data[0];
                    $("#addRawMaterialID").val(first.id);
                    $("#addRawMaterialSellingPriceInput").val(first.purchasePrice);
                    $("#addRawMaterialQuantityInput").val(1);
                    calculateRawMaterialTotal();
                } else {
                    alert("❌ لم يتم العثور على المادة الخام");
                }
            })
            .fail(() => alert("⚠️ حدث خطأ أثناء البحث عن المادة الخام"));
    }

    // 📤 إرسال نموذج إضافة المنتج
    function submitProductForm() {
        console.log("🚀 إرسال نموذج المنتج...");
        const form = $("#addItemForm")[0];
        const formData = new FormData(form);

        $.ajax({
            url: "/ImportOrders/AddItem",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    $("#addItemModal").modal("hide");
                    alert("✅ تم إضافة المنتج بنجاح");
                    refreshItemsList();
                    calculateTotalAmount();
                } else {
                    alert("❌ فشل في إضافة المنتج");
                }
            },
            error: function (xhr) {
                console.error("❌ خطأ أثناء الإرسال:", xhr.responseText);
                alert("⚠️ حدث خطأ أثناء إرسال البيانات");
            }
        });
    }

    // 📤 إرسال نموذج إضافة المادة الخام
    function submitRawMaterialForm() {
        console.log("🚀 إرسال نموذج المادة الخام...");
        const form = $("#addRawMaterialItemForm")[0];
        const formData = new FormData(form);

        $.ajax({
            url: "/ImportOrders/AddRawMaterialItem",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    $("#addRawMaterialItemModal").modal("hide");
                    alert("✅ تم إضافة المادة الخام بنجاح");
                    refreshItemsList();
                    calculateTotalAmount();
                } else {
                    alert("❌ فشل في إضافة المادة الخام");
                }
            },
            error: function (xhr) {
                console.error("❌ خطأ أثناء الإرسال:", xhr.responseText);
                alert("⚠️ حدث خطأ أثناء إرسال البيانات");
            }
        });
    }

    // 🔄 إعادة تحميل قائمة العناصر بعد الإضافة
    function refreshItemsList() {
        const orderId = getImportOrderId();
        if (!orderId) return;

        console.log("🔁 إعادة تحميل العناصر للأمر:", orderId);
        $.get("/ImportOrders/GetImportOrderUnionItems", { importOrderId: orderId })
            .done(function (html) {
                $("#importOrderItems").html(html);
                calculateTotalAmount();
            })
            .fail(() => console.error("⚠️ خطأ أثناء إعادة تحميل العناصر"));
    }

    // 💰 حساب المبلغ الإجمالي للأمر
    function calculateTotalAmount() {
        let total = 0;
        $(".union-item-row").each(function () {
            const qty = parseFloat($(this).find(".quantity").text().replace(/,/g, "")) || 0;
            const price = parseFloat($(this).find(".purchase-price").text().replace(/,/g, "")) || 0;
            total += qty * price;
        });
        $("#TotalAmount").val(total.toFixed(2));
        $("#totalAmount").text(total.toFixed(2));
        updateImportOrderTotal(total);
    }

    // 🔁 تحديث المجموع في السيرفر
    function updateImportOrderTotal(total) {
        const orderId = getImportOrderId();
        if (!orderId) return;

        $.ajax({
            url: "/ImportOrders/UpdateTotalAmount",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ importOrderId: orderId, totalAmount: total }),
            success: function (res) {
                if (res.success) console.log("✅ تم تحديث المجموع في السيرفر");
                else console.warn("⚠️ فشل في تحديث المجموع");
            },
        });
    }

    // 🔹 دالة مساعدة للحصول على ImportOrderID
    function getImportOrderId() {
        const idAttr = $("[data-import-order-id]").attr("data-import-order-id");
        if (idAttr) return parseInt(idAttr);
        const idFromForm = $("#importOrderForm input[name='ImportOrderID']").val();
        if (idFromForm) return parseInt(idFromForm);
        return 0;
    }
});
