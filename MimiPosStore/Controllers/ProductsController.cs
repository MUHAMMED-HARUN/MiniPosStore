using BAL;
using BAL.BALDTO;
using BAL.Interfaces;
using DAL.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MimiPosStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsBALDTOAsync();
            return View(products);
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

                return View(new ProductBALDTO());
            }

            return View(product);
        }

        // POST: Products/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductBALDTO productBALDTO)
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
                    // TODO: Get current user ID from authentication
                    string currentUserId = "95a74952-19d3-4339-8e71-2f1835eea812"; // Placeholder
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "product", "imgs");

                    // معالجة الصورة
                    if (productBALDTO.ID == 0)
                    {
                        // إضافة منتج جديد
                        if (productBALDTO.ProductImage != null)
                        {
                            productBALDTO.ImagePath = BAL.clsUtil.SaveImage(productBALDTO.ProductImage, uploadPath);
                        }
                        await _productService.CreateProductBALDTOAsync(productBALDTO, currentUserId, uploadPath);
                        TempData["SuccessMessage"] = "تم إضافة المنتج بنجاح";
                    }
                    else
                    {
                        // تحديث منتج موجود
                        // الحصول على المنتج الحالي للحصول على مسار الصورة الحالي
                        var currentProduct = await _productService.GetProductByIdBALDTOAsync(productBALDTO.ID);
                        productBALDTO.ImagePath = currentProduct.ImagePath;
                        
                        // معالجة الصورة الجديدة إذا تم رفعها
                        if (productBALDTO.ProductImage != null)
                        {
                            productBALDTO.ImagePath = BAL.clsUtil.SaveImage(productBALDTO.ProductImage, uploadPath);
                            // حذف الصورة القديمة
                            if (!string.IsNullOrEmpty(currentProduct.ImagePath))
                            {
                                BAL.clsUtil.DeleteImage(currentProduct.ImagePath, uploadPath);
                            }
                        }
                        
                        await _productService.UpdateProductBALDTOAsync(productBALDTO, currentUserId, uploadPath);
                        TempData["SuccessMessage"] = "تم تحديث المنتج بنجاح";
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
            return View(productBALDTO);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdBALDTOAsync(id);
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
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            var products = await _productService.SearchProductsBALDTOAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View("Index", products);
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
