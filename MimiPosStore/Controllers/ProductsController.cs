using BAL;
 
using BAL.Interfaces;
using SharedModels.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using SharedModels.EF.DTO;

namespace MimiPosStore.Controllers
{
    [Authorize]

    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICurrentUserService _CurentUserService; 

        public ProductsController(IProductService productService, IWebHostEnvironment webHostEnvironment,ICurrentUserService currentUser)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
            _CurentUserService = currentUser;
        }

        // GET: Products
        public async Task<IActionResult> Index(SharedModels.EF.Filters.clsProductFilter filter)
        {
            // Populate dropdowns for filters
            ViewBag.UOMList = new SelectList(await _productService.GetAllUOMAsync(), "Name", "Name");
            ViewBag.CurrencyList = new SelectList(BAL.clsGlobal.GetCurrencyTypeList(), "Key", "Value");

            var products = await _productService.GetAllProductsAsync(filter);

            // Wrap results in BAL filter model for the view convenience
        filter.products = products;
            return View(filter);
        }

        // GET: Products/Save (للإضافة)


        // GET: Products/Save/5 (للتعديل)
        public async Task<IActionResult> Save(int id)
        {
            var product = await _productService.GetProductByIdBALDTOAsync(id);
                await PopulateUOMDropDown();
            ViewBag.CurrencyList = new SelectList(clsGlobal.GetCurrencyTypeList(), "Key", "Value");
            if (product == null)
            {

                return View(new ProductDTO());
            }

            return View(product);
        }

        // POST: Products/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductDTO ProductDTO)
        {
            ModelState.Remove("ImagePath");
            ModelState.Remove("ProductImage");
            ModelState.Remove("UOMName");
            ModelState.Remove("UOMSymbol");
            ModelState.Remove("ActionByUser");
            ModelState.Remove("CurrencyName");
         
            if (ModelState.IsValid)
            {
                try
                {
                    ProductDTO.Name = ProductDTO.Name.Trim();
                    // TODO: Get current user id from authentication
                    string currentUserId = _CurentUserService.GetCurrentUserId(); // Placeholder
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "product", "imgs");

                    // معالجة الصورة
                    if (ProductDTO.ID == 0)
                    {
                        // إضافة منتج جديد
                        if (ProductDTO.ProductImage != null)
                        {
                            ProductDTO.ImagePath = BAL.clsUtil.SaveImage(ProductDTO.ProductImage, uploadPath);
                        }
                        bool result =   await _productService.CreateProductDTOAsync(ProductDTO, currentUserId, uploadPath);
                        if(result)
                        TempData["SuccessMessage"] = "تم إضافة المنتج بنجاح";
                        else
                            TempData["ErrorMessage"] = "اسم المنتج هذا محجوز بالفعل اختر اسم اخر";



                    }
                    else
                    {
                        // تحديث منتج موجود
                        // الحصول على المنتج الحالي للحصول على مسار الصورة الحالي
                        var currentProduct = await _productService.GetProductByIdBALDTOAsync(ProductDTO.ID);
                        ProductDTO.ImagePath = currentProduct.ImagePath;
                        
                        // معالجة الصورة الجديدة إذا تم رفعها
                        if (ProductDTO.ProductImage != null)
                        {
                            ProductDTO.ImagePath = BAL.clsUtil.SaveImage(ProductDTO.ProductImage, uploadPath);
                            // حذف الصورة القديمة
                            if (!string.IsNullOrEmpty(currentProduct.ImagePath))
                            {
                                BAL.clsUtil.DeleteImage(currentProduct.ImagePath, uploadPath);
                            }
                        }
                        
                        
                        bool result =await _productService.UpdateProductDTOAsync(ProductDTO, currentUserId, uploadPath);
                           if (result)
                            TempData["SuccessMessage"] = "تم تحديث المنتج بنجاح";
                           else
                            TempData["ErrorMessage"] ="اسم المنتج هذا محجوز بالفعل اختر اسم اخر";

                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء حفظ المنتج: " + ex.Message);
                }
            }
            ViewBag.CurrencyList = new SelectList(clsGlobal.GetCurrencyTypeList(), "Key", "Value");
            await PopulateUOMDropDown();
            return View(ProductDTO);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "product", "imgs");
                var result = await _productService.DeleteProductAsync(id, uploadPath);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف المنتج بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف المنتج";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف المنتج: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Search
        public IActionResult Search(string searchTerm)
        {
            // Redirect to Index with filter query (search by ItemName)
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new { Name = searchTerm });
        }

        [HttpGet]
        public async Task<JsonResult> SearchProductByNmae(string term)
        {
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    return Json(new List<object>());
                }

                var products = await _productService.SearchProductByNameBALDTOAsync(term);
                
                if (products != null)
                {

                    var result = new
                    {
                        success = true,
                        id = products.ID,
                        name = products.Name,
                        retailPrice = products.RetailPrice,
                        wholesalePrice = products.WholesalePrice,
                        availableQuantity = products.AvailableQuantity,
                        currencyType = products.CurrencyName,
                        uomName = products.UOMName
                    };

                    return Json(result);
                }

                return Json(new List<object>());
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }


        private async Task PopulateUOMDropDown()
        {
            var uoms = await _productService.GetAllUOMAsync();
            ViewBag.UOMList = new SelectList(uoms, "ID", "Name");
        }

        // دالة مساعدة لتحويل string إلى enum
        private BAL.clsGlobal.enCurrencyType ParseCurrencyType(string currencyType)
        {
            if (Enum.TryParse<BAL.clsGlobal.enCurrencyType>(currencyType, out var result))
            {
                return result;
            }
            return BAL.clsGlobal.enCurrencyType.TRY; // القيمة الافتراضية
        }
    }
}
