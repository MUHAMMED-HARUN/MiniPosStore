using BAL;
using BAL.BALDTO;
using BAL.Interfaces;
using BAL.Mappers;
using DAL.EF.DTO;
using DAL.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MimiPosStore.Controllers
{
    [Authorize]

    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;


        public OrdersController(IOrderService orderService, ICustomerService customerService, IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _orderService.GetAllBALDTOAsync();
                return View(orders);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة الطلبات: " + ex.Message;
                return View(new List<OrderBALDTO>());
            }
        }

        // GET: Orders/Save (للإضافة)
        //public async Task<IActionResult> Save()
        //{
        //    await PopulateDropDowns();
        //    return View(new OrderBALDTO { OrderDate = DateTime.Now });
        //}

        // GET: Orders/Save/5 (للتعديل)
        public async Task<IActionResult> Save(int id)
        {
            var order = await _orderService.GetByIdBALDTOAsync(id);
            await PopulateDropDowns();
            
            if (order == null)
            {
                order = new OrderBALDTO();
                order.CustomerID = 1;
                order.OrderDate = DateTime.Now;
                order.TotalAmount = 0;
                order.PaidAmount = 0;
                order.PaymentStatus =((int)clsGlobal.enPaymentStatus.Pending);
                order.ActionByUser = "95a74952-19d3-4339-8e71-2f1835eea812"; // Placeholder for current user ID
                order.ActionDate = DateTime.Now;
                bool Resut = await _orderService.CreateBALDTOAsync(order);
                return View(order);
            }

            return View(order);
        }

        //// POST: Orders/Save
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Save(OrderBALDTO orderBALDTO, string ItemIds=null)
        //{

        //    ModelState.Remove("PhoneNumber");
        //    ModelState.Remove("LastName");
        //    ModelState.Remove("FirstName");
        //    ModelState.Remove("ActionByUser");

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // TODO: Get current user ID from authentication
        //            string currentUserId = "95a74952-19d3-4339-8e71-2f1835eea812"; // Placeholder
        //            orderBALDTO.ActionByUser = currentUserId;
        //            // معالجة الطلب
        //            if (orderBALDTO.ID == 0)
        //            {
        //                // إضافة طلب جديد
        //                await _orderService.CreateBALDTOAsync(orderBALDTO);
        //                TempData["SuccessMessage"] = "تم إضافة الطلب بنجاح";
        //            }
        //            else
        //            {
        //                // تحديث طلب موجود
        //               bool  result =  await _orderService.UpdateBALDTOAsync(orderBALDTO);

        //                if (result)
        //                {
        //                    if (!string.IsNullOrEmpty(ItemIds))
        //                    {
        //                        try
        //                        {
        //                            var ArrayItemsID = JsonSerializer.Deserialize<int[]>(ItemIds);
        //                            if (ArrayItemsID.Length > 0 && ArrayItemsID != null) 
        //                         await   _productService.DecreaseProductQuantityAsync(ArrayItemsID, currentUserId);
        //                        }
        //                        catch(ArgumentException e)
        //                        {
        //                            throw new ArgumentException(e.Message);
        //                        }
        //                    }
        //                    TempData["SuccessMessage"] = "تم تحديث الطلب بنجاح";
        //                }
        //                else
        //                {
        //                    TempData["ErrorMessage"] = "فشل في تحديث الطلب";
        //                }

        //            }

        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "حدث خطأ أثناء حفظ الطلب: " + ex.Message);
        //            TempData["ErrorMessage"] = "فشل في تحديث الطلب" + " " + ex.Message;
        //            ;

        //        }
        //    }

        //    await PopulateDropDowns();
        //    return View(orderBALDTO);
        //}

        // GET: Orders/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(OrderBALDTO orderBALDTO, string? ItemIds = null)
        {
            ModelState.Remove("PhoneNumber");
            ModelState.Remove("LastName");
            ModelState.Remove("FirstName");
            ModelState.Remove("ActionByUser");

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "بيانات الطلب غير صحيحة" });
            }

            try
            {
                string currentUserId = "95a74952-19d3-4339-8e71-2f1835eea812"; // Placeholder
                orderBALDTO.ActionByUser = currentUserId;

                if (orderBALDTO.ID == 0)
                {
                    await _orderService.CreateBALDTOAsync(orderBALDTO);
                    return Json(new { success = true, message = "تم إضافة الطلب بنجاح" });
                }
                else
                {
                    bool result = await _orderService.UpdateBALDTOAsync(orderBALDTO);
                    if (!string.IsNullOrEmpty(ItemIds))
                    {
                        var ArrayItemsID = JsonSerializer.Deserialize<int[]>(ItemIds);
                        if (ArrayItemsID?.Length > 0)
                            await _productService.DecreaseProductQuantityAsync(ArrayItemsID, currentUserId);
                    }

                    if (result)
                    {
                        return Json(new { success = true, message = "تم تحديث الطلب بنجاح" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "فشل في تحديث الطلب" });
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                return Json(new { success = false, message = argEx.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حفظ الطلب: " + ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                string currentUserId = "95a74952-19d3-4339-8e71-2f1835eea812"; // Placeholder
                var result = await _orderService.DeleteAsync(id, currentUserId);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف الطلب بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف الطلب";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الطلب: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return RedirectToAction(nameof(Index));
                }

                var orders = await _orderService.SearchOrdersAsync(searchTerm);
                var orderBALDTOs = orders.ToOrderBALDTOList();
                ViewBag.SearchTerm = searchTerm;
                return View("Index", orderBALDTOs);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء البحث: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Orders/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(OrderItemsBALDTO orderItemBALDTO)
        {
            ModelState.Remove("ProductName");
            ModelState.Remove("ID");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _orderService.AddItemBALDTOAsync(orderItemBALDTO);
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

            return RedirectToAction(nameof(Save), new { id = orderItemBALDTO.OrderID });
        }

        // POST: Orders/UpdateItem
        [HttpPost]
        public async Task<IActionResult> UpdateItem(OrderItemsBALDTO orderItemBALDTO)
        {
            ModelState.Remove("ProductName");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _orderService.UpdateItemBALDTOAsync(orderItemBALDTO);
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

            return RedirectToAction(nameof(Save), new { id = orderItemBALDTO.OrderID });
        }

        // POST: Orders/DeleteItem
        [HttpPost]

        public async Task<IActionResult> DeleteItem(int itemId, int orderId)
        {
            try
            {
                var result = await _orderService.DeleteItem(itemId);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف العنصر بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف العنصر";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف العنصر: " + ex.Message;
            }

            return RedirectToAction(nameof(Save), new { id = orderId });
        }

        // GET: Orders/GetOrderItems
        [HttpGet]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            try
            {
                var orderItems = await _orderService.GetOrderItemsByOrderIdBALDTOAsync(orderId);
                return PartialView("_OrderItemsList", orderItems);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Orders/GetProductInfo
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

        // GET: Orders/GetOrderItemForEdit
        [HttpGet]
        public async Task<IActionResult> GetOrderItemForEdit(int itemId)
        {
            try
            {
                var orderItem = await _orderService.GetOrderItemByIdBALDTOAsync(itemId);
                if (orderItem != null)
                {
                    return Json(new
                    {
                        success = true,
                        item = new
                        {
                            id = orderItem.ID,
                            orderID = orderItem.OrderID,
                            productID = orderItem.ProductID,
                            quantity = orderItem.Quantity,
                            sellingPrice = orderItem.SellingPrice,
                            availableQuantity= orderItem.AvailableQuantity

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

        // GET: Orders/UpdatePaymentStatus
        [HttpGet]
        public async Task<IActionResult> UpdatePaymentStatus(int id, byte paymentStatus)
        {
            try
            {
                var result = await _orderService.UpdatePaymentStatusAsync(id, paymentStatus);
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

        // GET: Orders/AddPayment
        [HttpGet]
        public async Task<IActionResult> AddPayment(int id)
        {
            var order = await _orderService.GetByIdBALDTOAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.RemainingAmount = await _orderService.GetRemainingAmountAsync(id);
            return View(order);
        }

        // POST: Orders/AddPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPayment(int id, float paymentAmount)
        {
            try
            {
                var result = await _orderService.AddPaymentAsync(id, paymentAmount);
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
                // تحميل قائمة العملاء
                var customers = await _customerService.GetAllBALDTOAsync();
                ViewBag.CustomerList = new SelectList(customers, "CustomerID", "FirstName");

                // تحميل قائمة المنتجات
                var products = await _productService.GetAllProductsBALDTOAsync();
                ViewBag.ProductList = new SelectList(products, "ID", "Name");

                // تحميل قائمة حالات الدفع
                ViewBag.PaymentStatusList = new SelectList(clsGlobal.GetPaymentStatusList(), "Key", "Value");
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، إعداد قوائم فارغة
                ViewBag.CustomerList = new SelectList(new List<CustomerBALDTO>(), "CustomerID", "FirstName");
                ViewBag.ProductList = new SelectList(new List<ProductBALDTO>(), "ID", "Name");
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
