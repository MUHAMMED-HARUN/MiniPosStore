using BAL;
 
using BAL.Interfaces;
using BAL.Mappers;
using SharedModels.EF.DTO;
using SharedModels.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;

namespace MimiPosStore.Controllers
{
    [Authorize]

    public class ImportOrdersController : Controller
    {
        private readonly IImportOrderService _importOrderService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly IImportOrderItemService _importOrderItemService;
        private readonly ICurrentUserService _currentUserService;
        public ImportOrdersController(IImportOrderService importOrderService, 
                                   ISupplierService supplierService, 
                                   IProductService productService,
                                   IImportOrderItemService importOrderItemService,ICurrentUserService currentUser)
        {
            _importOrderService = importOrderService;
            _supplierService = supplierService;
            _productService = productService;
            _importOrderItemService = importOrderItemService;
            _currentUserService = currentUser;
        }

        // GET: ImportOrders
        public async Task<IActionResult> Index(clsImportOrderFilter filter)
        {
            try
            {
                // Populate dropdowns for filters
                ViewBag.PaymentStatusList = new SelectList(BAL.clsGlobal.GetPaymentStatusList(), "Key", "Value");

                var importOrders = await _importOrderService.GetAllSummaryBALDTOAsync(filter);

                // Wrap results in BAL filter model for the view convenience
                filter.importOrders = importOrders;

                return View(filter);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة أوامر الاستيراد: " + ex.Message;
                return View(new clsImportOrderFilter { importOrders = new List<ImportOrderDTO>() });
            }
        }

        // GET: ImportOrders/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            var importOrder = new ImportOrderDTO 
            { 
                ImportDate = DateTime.Now,
                PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Pending), // غير مدفوع
                ActionDate = DateTime.Now
            };
            return View(importOrder);
        }

        // POST: ImportOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportOrderDTO ImportOrderDTO)
        {

            List<string> Column = new List<string>();
            Column.Add("ImportDate");
            Column.Add("ImportOrderID");
            Column.Add("SupplierID");
            Column.Add("PaidAmount");

            var toRemoveKeys = ModelState.Keys.Where(key => !Column.Contains(key)).ToList();
            toRemoveKeys.ForEach(k => ModelState.Remove(k));
            string Requierd = "الحقل مطلوب";

            if (ModelState.IsValid)
            {
                try
                {
                    ImportOrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Pending);

                    var result = await _importOrderService.AddBALDTOAsync(ImportOrderDTO);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "تم إضافة أمر الاستيراد بنجاح";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "فشل في إضافة أمر الاستيراد";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء حفظ أمر الاستيراد: " + ex.Message);
                }
            }
            
            await PopulateDropDowns();
            return View(ImportOrderDTO);
        }

        // GET: ImportOrders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var importOrder = await _importOrderService.GetByIdWithItemsBALDTOAsync(id);
            if (importOrder == null)
            {
                return NotFound();
            }

            await PopulateDropDowns();
      
      
            


            return View(importOrder);
        }

        // POST: ImportOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImportOrderDTO ImportOrderDTO, string ItemIds = null)
        {
            if (id != ImportOrderDTO.ImportOrderID)
            {
                return NotFound();
            }

            List<string> Column = new List<string>();
            Column.Add("ImportDate");
            Column.Add("ImportOrderID");
            Column.Add("SupplierID");
            Column.Add("PaymentStatus");
            Column.Add("PaidAmount");

            var toRemoveKeys = ModelState.Keys.Where(key => !Column.Contains(key)).ToList();
            toRemoveKeys.ForEach(k => ModelState.Remove(k));

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _importOrderService.UpdateBALDTOAsync(ImportOrderDTO);
                    if (result)
                    {
                        // إذا كانت هناك قائمة ItemIds، استخدمها بدلاً من العناصر الموجودة
                        if (!string.IsNullOrEmpty(ItemIds))
                        {
                            try
                            {
                                var itemIdsArray = JsonSerializer.Deserialize<int[]>(ItemIds);
                                if (itemIdsArray != null && itemIdsArray.Length > 0)
                                {
                                    await _productService.IncreaseProductQuantityAsync(itemIdsArray, _currentUserService.GetCurrentUserId());
                                }
                            }
                            catch (Exception ex)
                            {
                               
                            }
                        }
          

                        // إذا كان الطلب AJAX، أعد JSON
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "تم تحديث أمر الاستيراد بنجاح" });
                        }

                        TempData["SuccessMessage"] = "تم تحديث أمر الاستيراد بنجاح";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = "فشل في تحديث أمر الاستيراد" });
                        }
                        TempData["ErrorMessage"] = "فشل في تحديث أمر الاستيراد";
                    }
                }
                catch (Exception ex)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "حدث خطأ أثناء تحديث أمر الاستيراد: " + ex.Message });
                    }
                    ModelState.AddModelError("", "حدث خطأ أثناء تحديث أمر الاستيراد: " + ex.Message);
                }
            }
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Json(new { success = false, errors = errors });
            }
            
            await PopulateDropDowns();
            return View(ImportOrderDTO);
        }

        // GET: ImportOrders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var importOrder = await _importOrderService.GetByIdWithItemsBALDTOAsync(id);
            if (importOrder == null)
            {
                return NotFound();
            }

            return View(importOrder);
        }

        // GET: ImportOrders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var importOrder = await _importOrderService.GetByIdBALDTOAsync(id);
            if (importOrder == null)
            {
                return NotFound();
            }

            return View(importOrder);
        }

        // POST: ImportOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _importOrderService.DeleteAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف أمر الاستيراد بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف أمر الاستيراد";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف أمر الاستيراد: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ImportOrders/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return RedirectToAction(nameof(Index));
                }

                // يمكن إضافة منطق البحث هنا
                var importOrders = await _importOrderService.GetAllSummaryBALDTOAsync();
                var filteredOrders = importOrders.Where(io => 
                    io.SupplierName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    io.ImportOrderID.ToString().Contains(searchTerm)
                ).ToList();

                ViewBag.SearchTerm = searchTerm;
                return View("Index", filteredOrders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء البحث: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ImportOrders/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(ImportOrderItemDTO ImportOrderItemDTO)
        {
            ModelState.Remove("UOMName");
            ModelState.Remove("UOMSymbol");
            ModelState.Remove("CurrencyName");
            ModelState.Remove("CurrencyType");
            ModelState.Remove("ProductName");


            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _importOrderItemService.AddBALDTOAsync(ImportOrderItemDTO);
                    if (result)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "تم إضافة العنصر بنجاح" });
                        }
                        TempData["SuccessMessage"] = "تم إضافة العنصر بنجاح";
                    }
                    else
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = "فشل في إضافة العنصر" });
                        }
                        TempData["ErrorMessage"] = "فشل في إضافة العنصر";
                    }
                }
                catch (Exception ex)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "حدث خطأ أثناء إضافة العنصر: " + ex.Message });
                    }
                    TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة العنصر: " + ex.Message;
                }
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new { success = false, errors = errors });
                }
            }

            return RedirectToAction(nameof(Edit), new { id = ImportOrderItemDTO.ImportOrderID });
        }

        // POST: ImportOrders/UpdateItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItem(ImportOrderItemDTO ImportOrderItemDTO)
        {
            ModelState.Remove("ProductName");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _importOrderItemService.UpdateBALDTOAsync(ImportOrderItemDTO);
                    if (result)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "تم تحديث العنصر بنجاح" });
                        }
                        TempData["SuccessMessage"] = "تم تحديث العنصر بنجاح";
                    }
                    else
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = "فشل في تحديث العنصر" });
                        }
                        TempData["ErrorMessage"] = "فشل في تحديث العنصر";
                    }
                }
                catch (Exception ex)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "حدث خطأ أثناء تحديث العنصر: " + ex.Message });
                    }
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث العنصر: " + ex.Message;
                }
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new { success = false, errors = errors });
                }
            }

            return RedirectToAction(nameof(Edit), new { id = ImportOrderItemDTO.ImportOrderID });
        }

        // POST: ImportOrders/DeleteItem
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int itemId, int importOrderId)
        {
            try
            {
                var result = await _importOrderItemService.DeleteAsync(itemId);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف العنصر بنجاح";
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف العنصر";
                    return Json(new { success = false });

                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف العنصر: " + ex.Message;
                return Json(new { success = false });

            }


        }

        // GET: ImportOrders/GetImportOrderItems
        [HttpGet]
        public async Task<IActionResult> GetImportOrderItems(int importOrderId)
        {
            try
            {
                var importOrderItems = await _importOrderItemService.GetByImportOrderIdBALDTOAsync(importOrderId);
                return PartialView("_ImportOrderItemsList", importOrderItems);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: ImportOrders/GetProductInfo
        [HttpGet]
        public async Task<IActionResult> GetProductInfo(int productId)
        {
            try
            {
                var product = await _productService.GetByIdBALDTOAsync(productId);
                if (product != null)
                {
                    return Json(new
                    {
                        success = true,
                        retailPrice = product.RetailPrice,
                        wholesalePrice = product.WholesalePrice,
                        availableQuantity = product.AvailableQuantity,
                        currencyType = product.CurrencyName,
                        uomName = product.UOMName
                    });
                }
                return Json(new { success = false, message = "المنتج غير موجود" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: ImportOrders/GetImportOrderItemForEdit
        [HttpGet]
        public async Task<IActionResult> GetImportOrderItemForEdit(int itemId)
        {
            try
            {
                var importOrderItem = await _importOrderItemService.GetByIdBALDTOAsync(itemId);
                if (importOrderItem != null)
                {
                    return Json(new
                    {
                        success = true,
                        item = new
                        {
                            importOrderItemID = importOrderItem.ImportOrderItemID,
                            importOrderID = importOrderItem.ImportOrderID,
                            productID = importOrderItem.ProductID,
                            quantity = importOrderItem.Quantity,
                            sellingPrice = importOrderItem.SellingPrice
                        }
                    });
                }
                return Json(new { success = false, message = "العنصر غير موجود" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: ImportOrders/UpdatePaymentStatus
        [HttpGet]
        public async Task<IActionResult> UpdatePaymentStatus(int id, byte paymentStatus)
        {
            try
            {
                var result = await _importOrderService.UpdatePaymentStatusAsync(id, paymentStatus);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم تحديث حالة الدفع بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في تحديث حالة الدفع";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث حالة الدفع: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ImportOrders/AddPayment
        [HttpGet]
        public async Task<IActionResult> AddPayment(int id)
        {
            var importOrder = await _importOrderService.GetByIdBALDTOAsync(id);
            if (importOrder == null)
            {
                return NotFound();
            }

            ViewBag.RemainingAmount = await _importOrderService.GetRemainingAmountAsync(importOrder);
            return View(importOrder);
        }

        // GET: ImportOrders/AddItems
        [HttpGet]
        public async Task<IActionResult> AddItems()
        {
            try
            {
                var importOrders = await _importOrderService.GetAllSummaryBALDTOAsync();
                await PopulateDropDowns();
                return View(importOrders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة أوامر الاستيراد: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ImportOrders/AddPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayment(int id, float paymentAmount)
        {
            try
            {
                var result = await _importOrderService.AddPaymentAsync(id, paymentAmount);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم إضافة الدفعة بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في إضافة الدفعة";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة الدفعة: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDowns()
        {
            try
            {
                // تحميل قائمة الموردين
                var suppliers = await _supplierService.GetAllAsync();
        
                ViewBag.SupplierList = new SelectList(suppliers, "ID", "StoreName");

                // تحميل قائمة المنتجات
                var products = await _productService.GetAllProductsBALDTOAsync();
                ViewBag.ProductList = new SelectList(products, "ID", "Name");

                // تحميل قائمة حالات الدفع
                ViewBag.PaymentStatusList = new SelectList(clsGlobal.GetPaymentStatusList(), "Key", "Value");
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، إعداد قوائم فارغة
                ViewBag.SupplierList = new SelectList(new List<SupplierDTO>(), "SupplierID", "ShopName");
                ViewBag.ProductList = new SelectList(new List<ProductDTO>(), "ID", "Name");
                ViewBag.PaymentStatusList = new SelectList(new[]
                {
                    new { Key = (byte)0, Value = "غير مدفوع" },
                    new { Key = (byte)1, Value = "مدفوع جزئياً" },
                    new { Key = (byte)2, Value = "مدفوع بالكامل" }
                }, "Key", "Value");
            }
        }
    }
}
