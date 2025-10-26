استثناء حدث OrderConfirmed - السبب الجذري والحلول

ملخص المشكلة
- يظهر استثناء عند تنفيذ حدث OrderConfirmed أثناء عملية تحديث/حفظ الطلب.
- مسار الاستدعاء: DAL.clsDALUtil.ExecuteFilterCommands -> OrderRepo.GetOrderItemUnionDTOs -> RawMaterialService.DeReserveQuantityByOrderID / المعالج داخل مُنشئ RawMaterialService -> OrderService.OnOrderConfirmed -> OrderService.UpdateBALDTOAsync -> OrdersController.Save.

الأسباب الجذرية
1) استدعاءات متزامنة لحزم غير متزامنة
- استخدام ‎.Result على مهام async (مثل GetOrderItemUnionDTOs(...).Result) قد يؤدي إلى deadlock أو استثناءات بسبب تعارض سياق التنفيذ.

2) استخدام DbContext بشكل متزامن داخل معالجات الأحداث
- معالجات الأحداث قامت باستعلامات قاعدة البيانات بينما كان نفس الـ DbContext الخاص بالطلب قيد الاستخدام، مما سبب عمليات متزامنة على نفس الاتصال.

الإصلاحات المنفذة
- إضافة AsyncEventHandler<TEventArgs> وتحويل جميع الأحداث إلى نمط async/Task.
- انتظار (await) استدعاء الأحداث داخل OrderService بدل الاستدعاء المتزامن.
- استبدال ‎.Result بـ await في:
  * OrderService (لـ GetOrderItemUnionDTOs)
  * OrdersController (لـ GetOrderItemUnionDTOs)
- داخل RawMaterialService:
  * إزالة اشتراك مكرر لحدث OrderConfirmedEvent كان ينادي DeReserveQuantityByOrderID مباشرة.
  * حقن IServiceScopeFactory وإنشاء scope جديد داخل DeReserveQuantityByOrderID وداخل OnOrderConfirmd.
  * تنفيذ الاستعلام عن عناصر union عبر نسخة IOrderService ضمن scope جديد، ثم تنفيذ DeReserveQuantity/DecreaseAsync على كل عنصر مع await بشكل آمن.

توصيات
- تجنب استخدام ‎.Result / ‎.GetAwaiter().GetResult() داخل مسارات ASP.NET Core.
- عند تنفيذ أعمال خلفية/أحداث تتعامل مع DbContext أو المستودعات، أنشئ DI scope جديد لتفادي التعارض.
- إزالة السطر التجريبي `order.ID = 3;` داخل OrderService بعد CreateAsync لأنه يكتب قيمة خاطئة للمعرف.

الحالة
- فحوصات Lint/الأنواع مرت بنجاح للملفات المعدلة.
- ينبغي أن يعمل تدفق OrderConfirmed بدون استثناءات تزامن DbContext.
