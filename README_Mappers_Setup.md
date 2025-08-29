# إعداد المappers بين Models و BAL DTOs ✅

## نظرة عامة
تم إنشاء نظام مappers شامل في مجلد BAL لتحويل البيانات بين Models و DTOs المختلفة. هذا النظام يسهل عملية التحويل ويقلل من تكرار الكود.

## الملفات المنشأة

### 1. OrderMapper.cs
**الموقع:** `BAL/Mappers/OrderMapper.cs`

**الوظائف:**
- تحويل `clsOrder` إلى `clsOrderDTO`
- تحويل `clsOrderDTO` إلى `clsOrder`
- تحويل `clsOrderItem` إلى `clsOrderItemsDTO`
- تحويل `clsOrderItemsDTO` إلى `clsOrderItem`
- تحويل قوائم البيانات
- تحويل بين DAL DTOs و BAL DTOs

**الاستخدام:**
```csharp
// تحويل Model إلى BAL DTO
var orderDTO = order.ToBALDTO();

// تحويل BAL DTO إلى Model
var order = orderDTO.ToModel();

// تحويل قائمة
var orderDTOs = orders.ToBALDTOList();
```

### 2. ProductMapper.cs
**الموقع:** `BAL/Mappers/ProductMapper.cs`

**الوظائف:**
- تحويل `clsProduct` إلى `clsProductDTO`
- تحويل `clsProductDTO` إلى `clsProduct`
- تحويل قوائم البيانات
- تحويل بين DAL DTOs و BAL DTOs

**الاستخدام:**
```csharp
// تحويل Model إلى BAL DTO
var productDTO = product.ToBALDTO();

// تحويل BAL DTO إلى Model
var product = productDTO.ToModel();
```

### 3. CustomerMapper.cs
**الموقع:** `BAL/Mappers/CustomerMapper.cs`

**الوظائف:**
- تحويل `clsCustomer` إلى `clsCustomerDTO`
- تحويل `clsCustomerDTO` إلى `clsCustomer`
- تحويل `clsPerson` إلى `clsPersonDTO`
- تحويل `clsPersonDTO` إلى `clsPerson`
- تحويل قوائم البيانات
- تحويل بين DAL DTOs و BAL DTOs

**الاستخدام:**
```csharp
// تحويل Model إلى BAL DTO
var customerDTO = customer.ToBALDTO();

// تحويل BAL DTO إلى Model
var customer = customerDTO.ToModel();
```

### 4. EntityMapper.cs
**الموقع:** `BAL/Mappers/EntityMapper.cs`

**الوظائف:**
- Mapper عام يجمع جميع عمليات التحويل
- نقطة وصول واحدة لجميع المappers
- تبسيط الاستخدام في Controllers

**الاستخدام:**
```csharp
using BAL.Mappers;

// جميع عمليات التحويل متاحة من خلال EntityMapper
var orderDTO = order.ToBALDTO();
var productDTO = product.ToBALDTO();
var customerDTO = customer.ToBALDTO();
```

## الميزات

### 1. Extension Methods
- استخدام Extension Methods لسهولة الاستخدام
- استدعاء مباشر على الكائنات
- كود أكثر وضوحاً وقابلية للقراءة

### 2. Null Safety
- فحص القيم الفارغة (null)
- معالجة آمنة للبيانات المرتبطة
- تجنب أخطاء NullReferenceException

### 3. Navigation Properties
- تحويل تلقائي للبيانات المرتبطة
- دعم Include في Entity Framework
- تحويل متداخل للقوائم

### 4. Bidirectional Mapping
- تحويل من Model إلى DTO
- تحويل من DTO إلى Model
- تحويل بين DAL DTOs و BAL DTOs

## التحديثات في Controllers

### OrdersController
تم تحديث `OrdersController` لاستخدام المappers:

```csharp
// قبل التحديث
var clsOrderDto = new clsOrderDTO
{
    ID = order.ID,
    CustomerID = order.CustomerID,
    // ... باقي الخصائص
};

// بعد التحديث
var clsOrderDto = order.ToBALDTO();
```

**التحديثات المنجزة:**
- دالة `Save(int id)` - تحويل `clsOrder` إلى `clsOrderDTO`
- دالة `Save(clsOrderDTO orderDto)` - تحويل `clsOrderDTO` إلى `clsOrder`
- دالة `AddItem` - تحويل `clsOrderItemsDTO` إلى `clsOrderItem`
- دالة `UpdateItem` - تحويل `clsOrderItemsDTO` إلى `clsOrderItem`

## كيفية الاستخدام

### 1. في Controllers
```csharp
using BAL.Mappers;

public class OrdersController : Controller
{
    public async Task<IActionResult> Save(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        var orderDTO = order.ToBALDTO(); // تحويل سريع
        return View(orderDTO);
    }
}
```

### 2. في Services
```csharp
public async Task<List<clsOrderDTO>> GetAllOrdersAsync()
{
    var orders = await _orderRepo.GetAllAsync();
    return orders.ToBALDTOList(); // تحويل قائمة كاملة
}
```

### 3. في Repositories
```csharp
public async Task<bool> CreateAsync(clsOrderDTO orderDTO)
{
    var order = orderDTO.ToModel(); // تحويل للـ Model
    return await _context.Orders.AddAsync(order);
}
```

## المزايا

### 1. تقليل تكرار الكود
- إزالة التحويلات اليدوية
- كود أكثر تنظيماً
- صيانة أسهل

### 2. تحسين الأداء
- تحويل محسن
- تقليل عمليات التحويل غير الضرورية
- استخدام Extension Methods

### 3. سهولة الصيانة
- تغييرات مركزية
- إضافة خصائص جديدة بسهولة
- اختبار أسهل

### 4. قابلية التوسع
- إضافة مappers جديدة بسهولة
- دعم أنواع بيانات جديدة
- مرونة في التصميم

## الخطوات التالية

### 1. تحديث باقي Controllers
- تحديث `ProductsController`
- تحديث `CustomersController`
- تحديث `PeopleController`

### 2. إضافة المزيد من المappers
- Mappers للتقارير
- Mappers للإحصائيات
- Mappers مخصصة حسب الحاجة

### 3. اختبار المappers
- اختبار التحويلات
- اختبار الأداء
- اختبار الحالات الاستثنائية

## النتيجة
✅ **تم إنشاء نظام مappers شامل:**
- تحويل سريع وآمن بين Models و DTOs
- كود أكثر تنظيماً وقابلية للصيانة
- أداء محسن
- قابلية للتوسع والتطوير
