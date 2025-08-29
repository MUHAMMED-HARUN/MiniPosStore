
    $(document).ready(function () {
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
});
