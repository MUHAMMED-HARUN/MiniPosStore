# تحديثات نظام المنتجات

## التعديلات المنجزة

### 1. تحديث هيكلية DTO
- **DAL/EF/DTO/ProductDTO.cs**: تم إضافة حقل `CurrencyType` من نوع `clsGlobal.enCurrencyType`
- **BAL/DTOBal/clsProductDTO.cs**: تم تحديثه ليرث من `ProductDTO` مع إضافة خاصية `ProductImage` للتعامل مع رفع الصور

### 2. إضافة Custom Attributes
- **BAL/CustomAttributes/ImageValidationAttribute.cs**: تم إنشاء custom attribute للتحقق من صحة الصور المرفوعة
  - يتحقق من امتداد الملف (JPG, PNG, GIF, BMP)
  - يتحقق من حجم الملف (أقل من 5 ميجابايت)
  - يتحقق من نوع MIME

### 3. إنشاء ملف clsUtil للتعامل مع الصور
- **DAL/clsUtil.cs**: يحتوي على دوال:
  - `SaveImage()`: لحفظ الصور مع إنشاء اسم فريد باستخدام GUID
  - `DeleteImage()`: لحذف الصور من النظام

### 4. إنشاء ملف JavaScript للتعامل مع رفع الملفات
- **MimiPosStore/wwwroot/js/fileUpload.js**: يحتوي على:
  - `FileUploadManager` class للتعامل مع رفع الملفات
  - دوال لاختيار ومسح الصور
  - التحقق من صحة النماذج
  - إظهار رسائل للمستخدم

### 5. تحديث ProductService
- **BAL/Services/ProductService.cs**: تم تحديثه ليدعم:
  - رفع الصور عند إنشاء/تحديث المنتج
  - حذف الصور عند حذف المنتج
  - التعامل مع نوع العملة

### 6. إنشاء شاشة موحدة للإضافة والتعديل
- **MimiPosStore/Views/Products/Save.cshtml**: شاشة واحدة تستخدم `SaveMode` للتمييز بين الإضافة والتعديل
  - تحتوي على جميع حقول المنتج
  - دعم رفع الصور مع معاينة
  - اختيار نوع العملة من dropdown
  - التحقق من صحة البيانات

### 7. تحديث ProductsController
- **MimiPosStore/Controllers/ProductsController.cs**: تم تحديثه ليدعم:
  - شاشة `Save` موحدة للإضافة والتعديل
  - رفع الصور وحفظها في `wwwroot/product/imgs`
  - التعامل مع نوع العملة

### 8. تحديث Index View
- **MimiPosStore/Views/Products/Index.cshtml**: تم تحديثه ليعرض:
  - صور المنتجات
  - نوع العملة
  - روابط محدثة للشاشة الموحدة

### 9. إنشاء مجلدات الصور
- `MimiPosStore/wwwroot/product/imgs/`: لحفظ صور المنتجات
- `MimiPosStore/wwwroot/images/`: للصور العامة

## الميزات الجديدة

### 1. دعم نوع العملة
- إضافة enum `enCurrencyType` مع الخيارات:
  - TRY (ليرة تركية)
  - USD (دولار أمريكي)
  - EUR (يورو)

### 2. رفع الصور
- دعم رفع صور المنتجات
- معاينة الصور قبل الحفظ
- حفظ الصور بأسماء فريدة (GUID)
- التحقق من صحة الصور

### 3. شاشة موحدة
- شاشة واحدة للإضافة والتعديل
- استخدام `SaveMode` للتمييز بين العمليتين
- واجهة مستخدم محسنة

### 4. التحقق من البيانات
- استخدام Data Annotations
- Custom Attributes للتحقق من الصور
- التحقق من صحة النماذج في JavaScript

## كيفية الاستخدام

### إضافة منتج جديد
1. انتقل إلى قائمة المنتجات
2. اضغط على "إضافة منتج جديد"
3. املأ البيانات المطلوبة
4. اختر نوع العملة
5. ارفع صورة المنتج (اختياري)
6. اضغط على "إضافة"

### تعديل منتج موجود
1. من قائمة المنتجات، اضغط على "تعديل"
2. قم بتعديل البيانات المطلوبة
3. يمكنك تغيير الصورة أو الاحتفاظ بالصورة الحالية
4. اضغط على "تحديث"

### حذف منتج
1. من قائمة المنتجات، اضغط على "حذف"
2. تأكد من الحذف
3. سيتم حذف المنتج وصورته

## ملاحظات تقنية

1. **مسار الصور**: يتم حفظ الصور في `wwwroot/product/imgs/` مع أسماء فريدة
2. **نوع العملة**: يتم حفظه كـ string في قاعدة البيانات
3. **التحقق**: يتم التحقق من الصور في جانب العميل والخادم
4. **الأمان**: يتم التحقق من نوع وحجم الملفات المرفوعة

## الملفات المحدثة

- `DAL/EF/DTO/ProductDTO.cs`
- `BAL/DTOBal/clsProductDTO.cs`
- `BAL/CustomAttributes/ImageValidationAttribute.cs`
- `DAL/clsUtil.cs`
- `BAL/Services/ProductService.cs`
- `BAL/Interfaces/IProductService.cs`
- `DAL/EF/DTO/DTOExtensions.cs`
- `BAL/DTOBal/BALDTOExtensions.cs`
- `MimiPosStore/Controllers/ProductsController.cs`
- `MimiPosStore/Views/Products/Save.cshtml`
- `MimiPosStore/Views/Products/Index.cshtml`
- `MimiPosStore/wwwroot/js/fileUpload.js`

## الملفات الجديدة

- `BAL/CustomAttributes/ImageValidationAttribute.cs`
- `DAL/clsUtil.cs`
- `MimiPosStore/Views/Products/Save.cshtml`
- `MimiPosStore/wwwroot/js/fileUpload.js`
- `MimiPosStore/wwwroot/product/imgs/` (مجلد)
- `MimiPosStore/wwwroot/images/` (مجلد)
