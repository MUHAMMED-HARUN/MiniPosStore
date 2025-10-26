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
        private readonly IRawMaterialService _rawMaterialService;
        private readonly IImportOrderItemService _importOrderItemService;
        private readonly ICurrentUserService _currentUserService;
        public ImportOrdersController(IImportOrderService importOrderService, 
                                   ISupplierService supplierService, 
                                   IProductService productService,
                                   IRawMaterialService rawMaterialService,
                                   IImportOrderItemService importOrderItemService,ICurrentUserService currentUser)
        {
            _importOrderService = importOrderService;
            _supplierService = supplierService;
            _productService = productService;
            _rawMaterialService = rawMaterialService;
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
            ViewBag.ImportOrderId = id;
            return View(importOrder);
        }

        // POST: ImportOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImportOrderDTO ImportOrderDTO, string ItemIds = null)
        {
  

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

                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new
                            {
                                success = true,
                                message = "تم حفظ أمر الاستيراد بنجاح",
                                redirectUrl = Url.Action("Index", "ImportOrders")
                            });
                        }

                        // غير ذلك، تحويل عادي للصفحة
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

            // Get union items for this import order
            var unionFilter = new clsImportOrderItemUnionFilter
            {
                ImportOrderID = id
            };
            var unionItems = await _importOrderService.GetImportOrderItemUnionDTOs(unionFilter);
            importOrder.UnionItems = unionItems;

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
            ModelState.Remove("ProductName");
            ModelState.Remove("ID");
            ModelState.Remove("CurrencyType");
            ModelState.Remove("CurrencyName");
            ModelState.Remove("UOMSymbol");
            ModelState.Remove("UOMName");

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

        // POST: ImportOrders/DeleteUnionItem
        [HttpPost]
        public async Task<IActionResult> DeleteUnionItem(int itemId, int itemType, int importOrderId)
        {
            try
            {
                bool result = false;
                
                if (itemType == 1) // Product
                {
                    result = await _importOrderItemService.DeleteAsync(itemId);
                }
                else if (itemType == 2) // Raw Material
                {
                    result = await _importOrderService.DeleteRawMaterialItem(itemId);
                }
                
                if (result)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "تم حذف العنصر بنجاح" });
                    }
                    TempData["SuccessMessage"] = "تم حذف العنصر بنجاح";
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "فشل في حذف العنصر" });
                    }
                    TempData["ErrorMessage"] = "فشل في حذف العنصر";
                }
                
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "حدث خطأ أثناء حذف العنصر: " + ex.Message });
                }
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف العنصر: " + ex.Message;
                return Json(new { success = false });
            }
        }

        // GET: ImportOrders/EditUnionItem
        [HttpGet]
        public async Task<IActionResult> EditUnionItem(int itemId, int itemType, int importOrderId)
        {
            try
            {
                // تحميل البيانات المطلوبة للقوائم المنسدلة
                await PopulateDropDowns();
                clsImportOrderItemUnionFilter filter = new clsImportOrderItemUnionFilter();
                filter.ImportOrderItemID = itemId;
                filter.ItemType = itemType;
                filter.ImportOrderID = importOrderId;
                var importOrderItem = await _importOrderService.GetImportOrderItemUnionDTOs(filter);

                if (itemType == 1) // Product
                {

                    if (importOrderItem.FirstOrDefault() != null)
                    {
                        ViewBag.ImportOrderId = importOrderId;
                        ViewBag.ItemType = itemType;
                        return View("EditImportOrderItem", importOrderItem.FirstOrDefault()?.ToImportOrderItemModel());
                    }
                }
                else if (itemType == 2) // Raw Material
                {
                    if (importOrderItem.FirstOrDefault() != null)
                    {
                        ViewBag.ImportOrderId = importOrderId;
                        ViewBag.ItemType = itemType;
                        return View("EditImportRawMaterialItem", importOrderItem.FirstOrDefault()?.ToImportRawMaterialItemModel());
                    }
                }
                
                TempData["ErrorMessage"] = "العنصر غير موجود";
                return RedirectToAction("Edit", new { id = importOrderId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل العنصر: " + ex.Message;
                return RedirectToAction("Edit", new { id = importOrderId });
            }
        }

        // GET: ImportOrders/GetUnionItemForEdit
        [HttpGet]
        public async Task<IActionResult> GetUnionItemForEdit(int itemId, int itemType)
        {
            try
            {
                // تحميل البيانات المطلوبة للقوائم المنسدلة
                await PopulateDropDowns();
                
                if (itemType == 1) // Product
                {
                    var importOrderItem = await _importOrderItemService.GetByIdBALDTOAsync(itemId);
                    if (importOrderItem != null)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return PartialView("_EditImportOrderItem", importOrderItem);
                        }
                        
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
                    else
                    {
                        return Json(new { success = false, message = "المنتج غير موجود" });
                    }
                }
                else if (itemType == 2) // Raw Material
                {
                    var rawMaterialItem = await _importOrderService.GetRawMaterialItemByIdAsync(itemId);
                    if (rawMaterialItem != null)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return PartialView("_EditImportRawMaterialItem", rawMaterialItem);
                        }
                        
                        return Json(new
                        {
                            success = true,
                            item = new
                            {
                                id = rawMaterialItem.ID,
                                importOrderID = rawMaterialItem.ImportOrderID,
                                rawMaterialID = rawMaterialItem.RawMaterialID,
                                quantity = rawMaterialItem.Quantity,
                                sellingPrice = rawMaterialItem.SellingPrice
                            }
                        });
                    }
                    else
                    {
                        return Json(new { success = false, message = "المادة الخام غير موجودة" });
                    }
                }
                
                return Json(new { success = false, message = "نوع العنصر غير صحيح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // POST: ImportOrders/UpdateRawMaterialItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRawMaterialItem(clsImportRawMaterialItem ImportOrderItemDTO)
        {
            ModelState.Remove("ID");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _importOrderService.UpdateRawMaterialItem(ImportOrderItemDTO);
                    if (result)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "تم تحديث المادة الخام بنجاح" });
                        }
                        TempData["SuccessMessage"] = "تم تحديث المادة الخام بنجاح";
                    }
                    else
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = "فشل في تحديث المادة الخام" });
                        }
                        TempData["ErrorMessage"] = "فشل في تحديث المادة الخام";
                    }
                }
                catch (Exception ex)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "حدث خطأ أثناء تحديث المادة الخام: " + ex.Message });
                    }
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث المادة الخام: " + ex.Message;
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

        // POST: ImportOrders/AddRawMaterialItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRawMaterialItem(clsImportRawMaterialItem ImportOrderItemDTO)
        {
            ModelState.Remove("ID");
            ModelState.Remove("RawMaterial");
            ModelState.Remove("ImportOrder");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _importOrderService.AddRawMaterialItem(ImportOrderItemDTO);
                    if (result)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "تم إضافة المادة الخام بنجاح" });
                        }
                        TempData["SuccessMessage"] = "تم إضافة المادة الخام بنجاح";
                    }
                    else
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = "فشل في إضافة المادة الخام" });
                        }
                        TempData["ErrorMessage"] = "فشل في إضافة المادة الخام";
                    }
                }
                catch (Exception ex)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "حدث خطأ أثناء إضافة المادة الخام: " + ex.Message });
                    }
                    TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة المادة الخام: " + ex.Message;
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

        // GET: ImportOrders/GetImportOrderItems
        [HttpGet]
        public async Task<IActionResult> GetImportOrderItems(int importOrderId)
        {
            try
            {
                clsImportOrderItemUnionFilter itemUnionFilter = new clsImportOrderItemUnionFilter();
                itemUnionFilter.ImportOrderID = importOrderId;
              

                var Items = await _importOrderService.GetImportOrderItemUnionDTOs(itemUnionFilter);
             

             
                return PartialView("_ImportOrderUnionItemsList", Items);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: ImportOrders/GetUnionItems
        [HttpGet]
        public async Task<IActionResult> GetUnionItems(int importOrderId)
        {
            try
            {
                var unionFilter = new clsImportOrderItemUnionFilter
                {
                    ImportOrderID = importOrderId
                };
                var unionItems = await _importOrderService.GetImportOrderItemUnionDTOs(unionFilter);
                return PartialView("_ImportOrderUnionItemsList", unionItems);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // GET: ImportOrders/GetItemInfo
        [HttpGet]
        public async Task<IActionResult> GetItemInfo(int itemId, int itemType)
        {
            try
            {
                if (itemType == 1) // Product
                {
                    var product = await _productService.GetByIdBALDTOAsync(itemId);
                    if (product != null)
                    {
                        return Json(new
                        {
                            success = true,
                            itemId = product.ID,
                            itemType = 1,
                            name = product.Name,
                            description = product.Description,
                            sellingPrice = product.RetailPrice,
                            wholesalePrice = product.WholesalePrice,
                            availableQuantity = product.AvailableQuantity,
                            currencyName = product.CurrencyName,
                            uomName = product.UOMName,
                            uomSymbol = product.UOMSymbol
                        });
                    }
                }
                else if (itemType == 2) // Raw Material
                {
                    var rawMaterial = await _rawMaterialService.GetByIdBALDTOAsync(itemId);
                    if (rawMaterial != null)
                    {
                        return Json(new
                        {
                            success = true,
                            itemId = rawMaterial.ID,
                            itemType = 2,
                            name = rawMaterial.Name,
                            description = rawMaterial.Description,
                            sellingPrice = rawMaterial.PurchasePrice,
                            wholesalePrice = rawMaterial.PurchasePrice,
                            availableQuantity = rawMaterial.AvailableQuantity,
                            currencyName = rawMaterial.CurrencyTypeID,
                            uomName = rawMaterial.UOMName,
                            uomSymbol = rawMaterial.UOMName
                        });
                    }
                }
                return Json(new { success = false, message = "العنصر غير موجود" });
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

        // GET: ImportOrders/SearchRawMaterials
        [HttpGet]
        public async Task<IActionResult> SearchRawMaterials(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
                {
                    return Json(new List<object>());
                }

                clsRawMaterialFilter materialFilter = new clsRawMaterialFilter();
                materialFilter.Name = searchTerm;
                var filteredMaterials = await _rawMaterialService.GetAllBALDTOAsync(materialFilter);
                
                if (filteredMaterials != null && filteredMaterials.Any())
                {
                    var result = filteredMaterials.Select(material => new
                    {
                        id = material.ID,
                        name = material.Name,
                        purchasePrice = material.PurchasePrice,
                        availableQuantity = material.AvailableQuantity,
                        currencyType = "TRY", // قيمة افتراضية
                        uomName = material.UOMName
                    }).ToList();

                    return Json(result);
                }

                return Json(new List<object>());
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
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
                    // إذا كان الطلب AJAX، أعد HTML
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return PartialView("_EditImportOrderItem", importOrderItem);
                    }
                    
                    // وإلا أعد JSON
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

        // GET: ImportOrders/GetImportRawMaterialItemForEdit
        [HttpGet]
        public async Task<IActionResult> GetImportRawMaterialItemForEdit(int itemId)
        {
            try
            {
                var rawMaterialItem = await _importOrderService.GetRawMaterialItemByIdAsync(itemId);
                if (rawMaterialItem != null)
                {
                    // إذا كان الطلب AJAX، أعد HTML
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return PartialView("_EditImportRawMaterialItem", rawMaterialItem);
                    }
                    
                    // وإلا أعد JSON
                    return Json(new
                    {
                        success = true,
                        item = new
                        {
                            id = rawMaterialItem.ID,
                            importOrderID = rawMaterialItem.ImportOrderID,
                            rawMaterialID = rawMaterialItem.RawMaterialID,
                            quantity = rawMaterialItem.Quantity,
                            sellingPrice = rawMaterialItem.SellingPrice
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

        // GET: ImportOrders/GetImportOrderUnionItems
        [HttpGet]
        public async Task<IActionResult> GetImportOrderUnionItems(int importOrderId)
        {
            try
            {
                var unionFilter = new clsImportOrderItemUnionFilter
                {
                    ImportOrderID = importOrderId
                };
                var unionItems = await _importOrderService.GetImportOrderItemUnionDTOs(unionFilter);
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_ImportOrderUnionItemsList", unionItems);
                }
                
                return Json(new { success = true, items = unionItems });
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

        // GET: ImportOrders/ImportOrderItems - شاشة عناصر طلبات الاستيراد مع الفلاتر
        public async Task<IActionResult> ImportOrderItems(clsImportOrderItemUnionFilter filter)
        {
            try
            {
                // Populate dropdowns for filters
                await PopulateImportOrderItemsDropDowns();

                var importOrderItems = await _importOrderService.GetImportOrderItemUnionDTOs(filter);
                
                // Wrap results in filter model for the view convenience
                filter.OrderItemUnionDTOs = importOrderItems;

                return View(filter);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة عناصر طلبات الاستيراد: " + ex.Message;
                return View(new clsImportOrderItemUnionFilter { OrderItemUnionDTOs = new List<ImportOrderItemUnionDTO>() });
            }
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

                // تحميل قائمة المواد الخام
                var rawMaterials = await _rawMaterialService.GetAllBALDTOAsync(new clsRawMaterialFilter());
                ViewBag.RawMaterialList = new SelectList(rawMaterials, "ID", "Name");

                // تحميل قائمة حالات الدفع
                ViewBag.PaymentStatusList = new SelectList(clsGlobal.GetPaymentStatusList(), "Key", "Value");
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، إعداد قوائم فارغة
                ViewBag.SupplierList = new SelectList(new List<SupplierDTO>(), "SupplierID", "ShopName");
                ViewBag.ProductList = new SelectList(new List<ProductDTO>(), "ID", "Name");
                ViewBag.RawMaterialList = new SelectList(new List<RawMaterialDTO>(), "ID", "Name");
                ViewBag.PaymentStatusList = new SelectList(new[]
                {
                    new { Key = (byte)0, Value = "غير مدفوع" },
                    new { Key = (byte)1, Value = "مدفوع جزئياً" },
                    new { Key = (byte)2, Value = "مدفوع بالكامل" }
                }, "Key", "Value");
            }
        }

        private async Task PopulateImportOrderItemsDropDowns()
        {
            try
            {
                // Item Type dropdown
                ViewBag.ItemTypeList = new SelectList(new[]
                {
                    new { Key = 1, Value = "منتج" },
                    new { Key = 2, Value = "مادة خام" }
                }, "Key", "Value");

                // Currency Type dropdown
                ViewBag.CurrencyTypeList = new SelectList(new[]
                {
                    new { Key = 1, Value = "TRY" },
                    new { Key = 2, Value = "USD" },
                    new { Key = 3, Value = "EUR" }
                }, "Key", "Value");

                // UOM dropdown - يمكن تحسين هذا بجلب البيانات من قاعدة البيانات
                ViewBag.UOMList = new SelectList(new[]
                {
                    new { Key = 1, Value = "قطعة" },
                    new { Key = 2, Value = "كيلو" },
                    new { Key = 3, Value = "جرام" },
                    new { Key = 4, Value = "لتر" }
                }, "Key", "Value");
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، إعداد قوائم فارغة
                ViewBag.ItemTypeList = new SelectList(new List<object>(), "Key", "Value");
                ViewBag.CurrencyTypeList = new SelectList(new List<object>(), "Key", "Value");
                ViewBag.UOMList = new SelectList(new List<object>(), "Key", "Value");
            }
        }

        // POST: ImportOrders/UpdateTotalAmount
        //[HttpPost]
        //public async Task<IActionResult> UpdateTotalAmount([FromBody] UpdateTotalAmountRequest request)
        //{
        //    try
        //    {
        //        if (request == null || request.ImportOrderId <= 0)
        //        {
        //            return Json(new { success = false, message = "بيانات غير صحيحة" });
        //        }

        //        // تحديث المبلغ الإجمالي في قاعدة البيانات
        //        var importOrder = await _importOrderService.GetByIdBALDTOAsync(request.ImportOrderId);
        //        if (importOrder != null)
        //        {
        //            importOrder.TotalAmount = (float)request.TotalAmount;
        //            var result = await _importOrderService.UpdateBALDTOAsync(importOrder);
                    
        //            if (result)
        //            {
        //                return Json(new { success = true, message = "تم تحديث المبلغ الإجمالي بنجاح" });
        //            }
        //        }

        //        return Json(new { success = false, message = "فشل في تحديث المبلغ الإجمالي" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
        //    }
        //}
    }

    // نموذج طلب تحديث المبلغ الإجمالي
    public class UpdateTotalAmountRequest
    {
        public int ImportOrderId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
