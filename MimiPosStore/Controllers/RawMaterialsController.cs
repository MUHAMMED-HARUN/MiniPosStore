using BAL;
using BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
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
    public class RawMaterialsController : Controller
    {
        private readonly IRawMaterialService _rawMaterialService;
        private readonly ISupplierService _suppliersService;
        private readonly IUnitService _unitService;

        public RawMaterialsController(IRawMaterialService rawMaterialService, ISupplierService suppliersService, IUnitService unitService)
        {
            _rawMaterialService = rawMaterialService;
            _suppliersService = suppliersService;
            _unitService = unitService;
        }

        public async Task<IActionResult> Index(SharedModels.EF.Filters.clsRawMaterialFilter filter)
        {
            await PopulateDropDowns();
            var list = await _rawMaterialService.GetAllBALDTOAsync(filter);
            filter.rawMaterials = list;
            return View(filter);
        }

        public async Task<IActionResult> Save(int id)
        {
            await PopulateDropDowns();
            if (id == 0)
                return View(new clsRawMaterial { ActionDate = DateTime.Now });

            var dto = await _rawMaterialService.GetByIdAsync(id);
            return View(dto ?? new clsRawMaterial { ActionDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(clsRawMaterial dto)
        {

            ModelState.Remove("User");
            ModelState.Remove("UserID");
            ModelState.Remove("Supplier");
            ModelState.Remove("OrderItems");

            ModelState.Remove("unitOfMeasure");
            if (!ModelState.IsValid)
            {
                await PopulateDropDowns();
                return View(dto);
            }

            dto.Name = dto.Name.Trim();

            var ok = dto.ID == 0
                ? await _rawMaterialService.AddAsync(dto)
                : await _rawMaterialService.UpdateAsync(dto);

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
            var entity = await _rawMaterialService.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ok = await _rawMaterialService.DeleteAsync(id);
            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok ? "تم الحذف" : "فشل الحذف";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _rawMaterialService.GetByIdBALDTOAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        private async Task PopulateDropDowns()
        {
            var uoms = await _unitService.GetAllAsync();
            ViewBag.UOMList = new SelectList(uoms, "ID", "Name");

            var suppliers = await _suppliersService.GetAllAsync();
            ViewBag.SupplierList = new SelectList(suppliers, "ID", "StoreName");

            ViewBag.CurrencyList = new SelectList(clsGlobal.GetCurrencyTypeList(), "Key", "Value");
        }

        [HttpGet]
        public async Task<JsonResult> SearchRawMaterialByName(string term)
        {
            var filter = new SharedModels.EF.Filters.clsRawMaterialFilter
            {
                Name = term
            };
            var list = await _rawMaterialService.GetAllBALDTOAsync(filter);
            var material = list?.FirstOrDefault();
            var result = new
            {
                material.ID,
                material.Name,
                material.Description,
                material.PurchasePrice,
                material.AvailableQuantity,
                material.ProductionLossQuantity,
                material.UOMID,
                material.UOMName,
                material.CurrencyTypeID,
                material.MaterialSupplier,
                material.ReservedQuantity,
                material.SupplierName,
                material.ActionDate,
                material.UserID,
                WholesalePrice = material.PurchasePrice
            };

            return Json(result);
        }
    }
}


