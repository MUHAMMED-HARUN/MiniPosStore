
    $(document).ready(function () {
        // تعيين ImportOrderID
        var importOrderId = $("#ImportOrderID").val();
    $('#importOrderIdInput').val(importOrderId);

    // تحديث معلومات المنتج عند تغيير المنتج
    $('#productSelect').change(function () {
        var productId = $(this).val();
    if (productId) {
        $.get('/ImportOrders/GetProductInfo', { productId: productId }, function (data) {
            if (data.success) {
                $('#basePrice').text(data.retailPrice + ' ' + data.currencyType);
                $('#availableQuantity').text(data.availableQuantity);
                $('#currencyType').text(data.currencyType);
                $('#unitOfMeasure').text(data.uomName);

                // تعيين سعر البيع الافتراضي
                $('#sellingPriceInput').val(data.retailPrice);

                // حساب المبلغ الإجمالي
                calculateItemTotal();
            } else {
                $('#productInfo').html('<p class="text-danger">خطأ في تحميل معلومات المنتج</p>');
            }
        });
        } else {
        $('#productInfo').html('<p class="mb-1">السعر الأساسي: <span id="basePrice">-</span></p>' +
            '<p class="mb-1">الكمية المتوفرة: <span id="availableQuantity">-</span></p>' +
            '<p class="mb-1">العملة: <span id="currencyType">-</span></p>' +
            '<p class="mb-0">وحدة القياس: <span id="unitOfMeasure">-</span></p>');
        }
    });

    // حساب المبلغ الإجمالي عند تغيير الكمية أو السعر
    $('#quantityInput, #sellingPriceInput').on('input', function () {
        calculateItemTotal();
    });

    function calculateItemTotal() {
        var quantity = parseFloat($('#quantityInput').val()) || 0;
    var sellingPrice = parseFloat($('#sellingPriceInput').val()) || 0;
    var total = quantity * sellingPrice;
    $('#itemTotalAmount').val(total.toFixed(2));
        }


});

// حساب وتجميع المبالغ الإجمالية لكل عناصر أمر الاستيراد وتحديث حقل TotalAmount
$(document).ready(function () {
    function parseFloatSafe(text) {
        return parseFloat(String(text).replace(/[^\d.-]/g, '')) || 0;
    }
    function getOrderItemsList() {
        var list = [];

        // اجمع القيم من الخاصية data-item-id لكل صف عناصر موجود في القائمة المعروضة
        document.querySelectorAll('#importOrderItemsList .import-order-item-row').forEach(function (row) {
            var idAttr = row.getAttribute('data-item-id');
            var itemId = parseInt(idAttr);
            if (!isNaN(itemId)) {
                list.push(itemId);
            }
        });

        return list;
    }
    function calculateImportOrderTotalFromList() {
        var totalAmount = 0;

        // اجمع من القائمة داخل الحاوية إن وجدت
        $('#importOrderItemsList .item-total').each(function () {
            totalAmount += parseFloatSafe($(this).text());
        });

        // في حال عدم وجود الحاوية، حاول من جميع الصفوف كاحتياط
        if (totalAmount === 0) {
            $('.import-order-item-row .item-total').each(function () {
                totalAmount += parseFloatSafe($(this).text());
            });
        }

        // حدّث حقل المبلغ الإجمالي بالصيغة العشرية
        if ($('#TotalAmount').length) {
            $('#TotalAmount').val(totalAmount.toFixed(2));
        }
    }

    // حساب أولي عند فتح الصفحة
    calculateImportOrderTotalFromList();

    // راقب تغيّرات قائمة العناصر لإعادة الحساب تلقائياً
    var listContainer = document.getElementById('importOrderItemsList');
    if (listContainer && window.MutationObserver) {
        var observer = new MutationObserver(function () {
            calculateImportOrderTotalFromList();
        });
        observer.observe(listContainer, { childList: true, subtree: true });
    }

    // معالج إرسال النموذج: اجمع item IDs وأرسل مع بيانات الفورم
    $('#editImportOrderForm').on('submit', function (e) {
        e.preventDefault(); // منع الإرسال الافتراضي

        // احسب الإجمالي وعدد العناصر
        calculateImportOrderTotalFromList();
        var itemsCount = $('#importOrderItemsList .import-order-item-row').length || 0;
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
                    window.location.href = '/ImportOrders/Index';
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