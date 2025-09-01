
$(document).ready(function () {
    function getOrderItemsList() {
        var list = [];

        // اجمع القيم من الخاصية data-item-id لكل صف عناصر موجود في القائمة المعروضة
        document.querySelectorAll('#orderItems .order-item-row').forEach(function (row) {
            var idAttr = row.getAttribute('data-item-id');
            var itemId = parseInt(idAttr);
            if (!isNaN(itemId)) {
                list.push(itemId);
            }
        });

        return list;
    }
        // تحديث معلومات المنتج عند تغيير المنتج
        $('#productSelect').change(function () {
            var productId = $(this).val();
            if (productId) {
                loadProductInfo(productId);
            } else {
                clearProductInfo();
            }
        });

    // حساب المبلغ الإجمالي للعنصر
    $('#quantityInput, #sellingPriceInput').on('input', function () {
        calculateItemTotal();
    });

    // تحميل معلومات المنتج
    function loadProductInfo(productId) {
        $.get('/Orders/GetProductInfo', { productId: productId })
            .done(function (data) {
                if (data.success) {
                    $('#basePrice').text(data.retailPrice + ' ' + data.currencyType);
                    $('#availableQuantity').text(data.availableQuantity);
                    $('#unitOfMeasure').text(data.uomName);

                    // تعيين السعر الأساسي كسعر البيع تلقائياً
                    $('#sellingPriceInput').val(data.retailPrice);
                    $('#availableQuantityDisplay').val(data.availableQuantity);
                    calculateItemTotal();
                } else {
                    clearProductInfo();
                    alert('خطأ في تحميل معلومات المنتج: ' + data.message);
                }
            })
            .fail(function () {
                clearProductInfo();
                alert('حدث خطأ في الاتصال بالخادم');
            });
    }

    function clearProductInfo() {
        $('#basePrice').text('-');
        $('#availableQuantity').text('-');
        $('#unitOfMeasure').text('-');
        $('#availableQuantityDisplay').val('');
        $('#sellingPriceInput').val('');
        $('#itemTotalAmount').val('');
    }

    function calculateItemTotal() {
        var quantity = parseFloat($('#quantityInput').val()) || 0;
        var price = parseFloat($('#sellingPriceInput').val()) || 0;
        var total = quantity * price;
        $('#itemTotalAmount').val(total.toFixed(2));
    }

    // دالة لجمع معرفات عناصر الطلب
    function getOrderItemsList() {
        var list = [];

        // اجمع القيم من الخاصية data-item-id لكل صف عناصر موجود في القائمة المعروضة
        document.querySelectorAll('#orderItems .order-item-row').forEach(function (row) {
            var idAttr = row.getAttribute('data-item-id');
            var itemId = parseInt(idAttr);
            if (!isNaN(itemId)) {
                list.push(itemId);
            }
        });

        return list;
    }

    // إعادة تحميل عناصر الطلب بعد إضافة عنصر جديد
    $('#addItemForm').on('submit', function () {
        var form = $(this);
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: form.serialize(),
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: function (response) {
                if (response.success) {
                    $('#addItemModal').modal('hide');
                    // إعادة تحميل عناصر الطلب
                    if (typeof loadOrderItems === 'function') {
                        loadOrderItems();
                    }
                    // إعادة تعيين النموذج
                    form[0].reset();
                    clearProductInfo();
                    // عرض رسالة نجاح
                    if (typeof showSuccessMessage === 'function') {
                        showSuccessMessage(response.message);
                    }
                } else {
                    alert(response.message || 'حدث خطأ أثناء إضافة العنصر');
                }
            },
            error: function (xhr) {
                if (xhr.responseJSON && xhr.responseJSON.errors) {
                    // عرض أخطاء التحقق
                    var errorHtml = '<div class="alert alert-danger"><ul>';
                    $.each(xhr.responseJSON.errors, function (key, value) {
                        errorHtml += '<li>' + value.join(', ') + '</li>';
                    });
                    errorHtml += '</ul></div>';
                    $('.modal-body .alert-danger').remove();
                    $('.modal-body').prepend(errorHtml);
                } else {
                    alert('حدث خطأ أثناء إضافة العنصر');
                }
            }
        });
        return false; // منع الإرسال العادي للنموذج
    });

    $('#orderForm').on('submit', function (e) {
        e.preventDefault(); // منع الإرسال الافتراضي

        // احسب الإجمالي وعدد العناصر
        calculateItemTotal();
        var itemsCount = $('#orderItems .order-item-row').length || 0;
        $('#ItemsCount').val(itemsCount);

        // اجمع قائمة item IDs
        var itemIds = getOrderItemsList();

        // احصل على بيانات الفورم
        var formData = new FormData(this);

        // أضف قائمة item IDs كبيانات إضافية
        formData.append('ItemIds', JSON.stringify(itemIds));

        // أرسل البيانات عبر AJAX
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    alert('تم حفظ أمر الاستيراد بنجاح');
                    window.location.href = '/Orders/Index';
                } else {
                    alert('حدث خطأ أثناء الحفظ: ' + (response.message || 'خطأ غير معروف'));
                }
            },
            error: function () {
                alert('حدث خطأ في الاتصال بالخادم');
            }
        });
    });

});
