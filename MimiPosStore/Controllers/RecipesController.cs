using BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimiPosStore.Controllers
{
    [Authorize] 
    public class RecipesController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IProductService _productService;

        public RecipesController(IRecipeService recipeService, IProductService productService)
        {
            _recipeService = recipeService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(SharedModels.EF.Filters.clsRecipeFilter filter)
        {
            var list = await _recipeService.GetAllBALDTOAsync(filter);
            filter.recipes = list;
            return View(filter);
        }

        public async Task<IActionResult> Save(int id)
        {
            await PopulateDropDowns();
            if (id == 0)
                return View(new clsRecipe { ActionDate = DateTime.Now });

            var dto = await _recipeService.GetByIdAsync(id);
            return View(dto ?? new clsRecipe { ActionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(clsRecipe dto)
        {
            ModelState.Remove(nameof(dto.UserID));
            ModelState.Remove(nameof(dto.User));
            ModelState.Remove(nameof(dto.Product));
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(dto);
            }

            var ok = dto.ID == 0
                ? await _recipeService.AddAsync(dto)
                : await _recipeService.UpdateAsync(dto);

            if (ok)
            {
                TempData["SuccessMessage"] = dto.ID == 0 ? "تمت الإضافة بنجاح" : "تم التعديل بنجاح";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "فشلت العملية";
            await PopulateDropDowns();
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _recipeService.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await _recipeService.DeleteAsync(id);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok ? "تم الحذف" : "فشل الحذف";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductProduction(int RecipeID)
        {
            try
            {
                var result = await _recipeService.ProductProduction(RecipeID);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم إنتاج المنتج بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في إنتاج المنتج - تحقق من توفر المواد الخام";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إنتاج المنتج: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _recipeService.GetByIdBALDTOAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        private async Task PopulateDropDowns()
        {
            var products = await _productService.GetAllProductsBALDTOAsync();
            ViewBag.ProductList = new SelectList(products, "ID", "Name");
        }
    }
}


