# تحديثات نظام المنتجات - النسخة المصححة

## التصحيحات المنجزة

### 1. نقل clsUtil إلى BAL ✅
- تم نقل `clsUtil.cs` من `DAL/` إلى `BAL/`
- تم تحديث namespace من `DAL` إلى `BAL`

### 2. تحديث Interface ليتعامل مع Models مباشرة ✅
- **IProductService**: تم تحديثه ليتعامل مع `clsProduct` بدلاً من `ProductDTO`
- **IProductRepo**: تم تحديثه ليتعامل مع `clsProduct` بدلاً من `ProductDTO`

### 3. تحديث ProductService ليتعامل مع Models مباشرة ✅
- تم إزالة التحويلات غير الضرورية بين DTO و Model
- تم إضافة دالة `HandleImageUpload` لمعالجة رفع الصور بشكل مرن
- الدالة تتعامل مع الحالات التالية:
  - إذا تم رفع صورة جديدة: تحذف القديمة وتحفظ الجديدة
  - إذا لم يتم رفع صورة جديدة: تحتفظ بالصورة الحالية

### 4. تحديث ProductRepo ليتعامل مع Models مباشرة ✅
- تم إزالة التحويلات إلى DTO
- تم إضافة `CurrencyType` في عملية التحديث

### 5. تحديث ProductsController ✅
- تم تحديثه ليتعامل مع `clsProduct` مباشرة
- تم إضافة معالجة مرنة للصور:
  - **للإضافة**: إذا تم رفع صورة، يتم حفظها
  - **للتعديل**: 
    - إذا تم رفع صورة جديدة: تحذف القديمة وتحفظ الجديدة
    - إذا لم يتم رفع صورة: تحتفظ بالصورة الحالية

### 6. تحديث Views ✅
- **Index.cshtml**: تم تحديثه ليتعامل مع `clsProduct`
- **Delete.cshtml**: تم تحديثه ليتعامل مع `clsProduct`
- تم إضافة دالة `ParseCurrencyType` في Views لتحويل string إلى enum

## معالجة الصور المرنة

### عند التعديل بدون تغيير الصورة:
```csharp
// في ProductsController - Save Action
if (productDto.ProductImage != null)
{
    // رفع صورة جديدة
    product.ImagePath = BAL.clsUtil.SaveImage(productDto.ProductImage, uploadPath);
    // حذف الصورة القديمة
    if (!string.IsNullOrEmpty(currentProduct.ImagePath))
    {
        BAL.clsUtil.DeleteImage(currentProduct.ImagePath, uploadPath);
    }
}
// إذا لم يتم رفع صورة جديدة، يتم الاحتفاظ بالصورة الحالية
```

### دالة HandleImageUpload في ProductService:
```csharp
public string HandleImageUpload(IFormFile imageFile, string currentImagePath, string uploadPath)
{
    // إذا تم رفع صورة جديدة
    if (imageFile != null && imageFile.Length > 0)
    {
        // حذف الصورة القديمة إذا كانت موجودة
        if (!string.IsNullOrEmpty(currentImagePath))
        {
            clsUtil.DeleteImage(currentImagePath, uploadPath);
        }
        
        // حفظ الصورة الجديدة
        return clsUtil.SaveImage(imageFile, uploadPath);
    }
    
    // إذا لم يتم رفع صورة جديدة، احتفظ بالصورة الحالية
    return currentImagePath;
}
```

## الملفات المحدثة

### الملفات المصححة:
- `BAL/clsUtil.cs` (منقول من DAL)
- `BAL/Interfaces/IProductService.cs`
- `BAL/Services/ProductService.cs`
- `DAL/IRepo/IProductRepo.cs`
- `DAL/IRepoServ/ProductRepo.cs`
- `MimiPosStore/Controllers/ProductsController.cs`
- `MimiPosStore/Views/Products/Index.cshtml`
- `MimiPosStore/Views/Products/Delete.cshtml`

### الملفات الجديدة:
- `BAL/CustomAttributes/ImageValidationAttribute.cs`
- `MimiPosStore/Views/Products/Save.cshtml`
- `MimiPosStore/wwwroot/js/fileUpload.js`
- `MimiPosStore/wwwroot/product/imgs/` (مجلد)
- `MimiPosStore/wwwroot/images/` (مجلد)

## الميزات المحسنة

### 1. معالجة مرنة للصور ✅
- عند التعديل بدون تغيير الصورة: يتم الاحتفاظ بالصورة الحالية
- عند رفع صورة جديدة: يتم حذف القديمة وحفظ الجديدة
- عند الإضافة: يتم حفظ الصورة إذا تم رفعها

### 2. التعامل المباشر مع Models ✅
- Interface يتعامل مع Models مباشرة
- Service يتعامل مع Models مباشرة
- Repository يتعامل مع Models مباشرة
- Controller يتعامل مع Models مباشرة

### 3. دعم نوع العملة ✅
- إضافة enum `enCurrencyType` مع الخيارات: TRY, USD, EUR
- حفظ نوع العملة في قاعدة البيانات كـ string
- عرض نوع العملة في الواجهة

### 4. شاشة موحدة للإضافة والتعديل ✅
- شاشة واحدة تستخدم `SaveMode` للتمييز بين العمليتين
- دعم رفع الصور مع معاينة
- التحقق من صحة البيانات

## كيفية الاستخدام

### إضافة منتج جديد:
1. انتقل إلى قائمة المنتجات
2. اضغط على "إضافة منتج جديد"
3. املأ البيانات المطلوبة
4. اختر نوع العملة
5. ارفع صورة المنتج (اختياري)
6. اضغط على "إضافة"

### تعديل منتج موجود:
1. من قائمة المنتجات، اضغط على "تعديل"
2. قم بتعديل البيانات المطلوبة
3. **للصورة**:
   - إذا أردت تغيير الصورة: ارفع صورة جديدة
   - إذا لم ترد تغيير الصورة: اترك حقل الصورة فارغاً
4. اضغط على "تحديث"

### حذف منتج:
1. من قائمة المنتجات، اضغط على "حذف"
2. تأكد من الحذف
3. سيتم حذف المنتج وصورته

## ملاحظات تقنية

1. **مسار الصور**: يتم حفظ الصور في `wwwroot/product/imgs/` مع أسماء فريدة (GUID)
2. **نوع العملة**: يتم حفظه كـ string في قاعدة البيانات
3. **التحقق**: يتم التحقق من الصور في جانب العميل والخادم
4. **الأمان**: يتم التحقق من نوع وحجم الملفات المرفوعة
5. **المرونة**: النظام يتعامل بمرونة مع الصور عند التعديل

## الإجابة على سؤالك

**"إذا قمت بالتعديل ولم أقم بتغيير الصورة هل سيتعامل معها بشكل مرن أم ماذا؟"**

✅ **نعم، النظام يتعامل بشكل مرن تماماً:**

1. **إذا لم تقم برفع صورة جديدة**: سيتم الاحتفاظ بالصورة الحالية
2. **إذا رفعت صورة جديدة**: سيتم حذف الصورة القديمة وحفظ الصورة الجديدة
3. **إذا كان المنتج بدون صورة**: لن يتم تغيير شيء

هذا يتم من خلال الكود التالي في Controller:
```csharp
// معالجة الصورة الجديدة إذا تم رفعها
if (productDto.ProductImage != null)
{
    product.ImagePath = BAL.clsUtil.SaveImage(productDto.ProductImage, uploadPath);
    // حذف الصورة القديمة
    if (!string.IsNullOrEmpty(currentProduct.ImagePath))
    {
        BAL.clsUtil.DeleteImage(currentProduct.ImagePath, uploadPath);
    }
}
// إذا لم يتم رفع صورة جديدة، يتم الاحتفاظ بالصورة الحالية
```
