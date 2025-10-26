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
    public class RecipeInfosController : Controller
    {
        private readonly IRecipeInfoService _recipeInfoService;
        private readonly IRecipeService _recipeService;
        private readonly IRawMaterialService _rawMaterialService;

        public RecipeInfosController(IRecipeInfoService recipeInfoService, IRecipeService recipeService, IRawMaterialService rawMaterialService)
        {
            _recipeInfoService = recipeInfoService;
            _recipeService = recipeService;
            _rawMaterialService = rawMaterialService;
        }

        public async Task<IActionResult> Index(SharedModels.EF.Filters.clsRecipeInfoFilter filter)
        {
            var list = await _recipeInfoService.GetAllBALDTOAsync(filter);
            filter.recipeInfos = list;
            return View(filter);
        }

        public async Task<IActionResult> Save(int id)
        {
            await PopulateDropDowns();
            if (id == 0)
                return View(new clsRecipeInfo { ActionDate = DateTime.Now });

            var dto = await _recipeInfoService.GetByIdAsync(id);
            return View(dto ?? new clsRecipeInfo { ActionDate = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> Save(clsRecipeInfo dto)    
        {
            ModelState.Remove(nameof(dto.UserID));
            ModelState.Remove(nameof(dto.User));
            ModelState.Remove(nameof(dto.Recipe));
            ModelState.Remove(nameof(dto.RawMaterial));
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(dto);
            }

            var ok = dto.ID == 0
                ? await _recipeInfoService.AddAsync(dto)
                : await _recipeInfoService.UpdateAsync(dto);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = ok, message = ok ? (dto.ID == 0 ? "تمت الإضافة بنجاح" : "تم التعديل بنجاح") : "فشلت العملية" });
            }

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
            var entity = await _recipeInfoService.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await _recipeInfoService.DeleteAsync(id);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok ? "تم الحذف" : "فشل الحذف";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _recipeInfoService.GetByIdBALDTOAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        private async Task PopulateDropDowns()
        {
            var recipes = await _recipeService.GetAllBALDTOAsync();
            ViewBag.RecipeList = new SelectList(recipes, "ID", "Name");

            var materials = await _rawMaterialService.GetAllBALDTOAsync();
            ViewBag.MaterialList = new SelectList(materials, "ID", "Name");
        }

        [HttpGet]
        public async Task<IActionResult> GetByRecipeId(int recipeId)
        {
            try
            {
                var all = await _recipeInfoService.GetAllBALDTOAsync();
                var items = all.Where(x => x.RecipeID == recipeId).ToList();
                return PartialView("~/Views/Recipes/_RecipeInfosList.cshtml", items);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetForEdit(int itemId)
        {
            try
            {
                var item = await _recipeInfoService.GetByIdAsync(itemId);
                if (item == null)
                    return Json(new { success = false, message = "العنصر غير موجود" });

                return Json(new
                {
                    success = true,
                    item = new
                    {
                        id = item.ID,
                        recipeID = item.RecipeID,
                        rawMaterialID = item.RawMaterialID,
                        requiredQty = item.RequiredMaterialQuantity,
             
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            try
            {
                var ok = await _recipeInfoService.DeleteAsync(id);
                return Json(new { success = ok });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}


