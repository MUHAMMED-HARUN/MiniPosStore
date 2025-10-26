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
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ICurrentUserService _currentUserService;

        public ExpensesController(IExpenseService expenseService, ICurrentUserService currentUserService)
        {
            _expenseService = expenseService;
            _currentUserService = currentUserService;
        }

        // GET: Expenses/Index - شاشة المصروفات مع الفلاتر
        public async Task<IActionResult> Index(clsExpensesFilter filter)
        {
            try
            {
                // Populate dropdowns for filters
                await PopulateDropDowns();

                var expenses = await _expenseService.GetAllExpensesBALDTOAsync(filter);
                
                // Wrap results in filter model for the view convenience
                filter.expenses = expenses;

                return View(filter);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة المصروفات: " + ex.Message;
                return View(new clsExpensesFilter { expenses = new List<ExpensesDTO>() });
            }
        }

        // GET: Expenses/ExpenseTypes - شاشة أنواع المصروفات
        public async Task<IActionResult> ExpenseTypes(clsExpenseTypeFilter filter)
        {
            try
            {
                var expenseTypes = await _expenseService.GetAllExpenseTypesBALDTOAsync(filter);
                
                // Wrap results in filter model for the view convenience
                filter.expenseTypes = expenseTypes;

                return View(filter);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة أنواع المصروفات: " + ex.Message;
                return View(new clsExpenseTypeFilter { expenseTypes = new List<ExpenseTypeDTO>() });
            }
        }

        // GET: Expenses/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            var expense = new ExpensesDTO 
            { 
                ExpenseDate = DateTime.Now,
                ActionDate = DateTime.Now,
                ActionByUser =  _currentUserService.GetCurrentUserId()
            };
            return View(expense);
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(clsExpenses expenses)
        {
            ModelState.Remove("ExpenseType");
            ModelState.Remove("ActionByUser");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    expenses.ActionDate = DateTime.Now;
                    expenses.ActionByUser =  _currentUserService.GetCurrentUserId();

                    var result = await _expenseService.CreateExpenseAsync(expenses);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "تم إضافة المصروف بنجاح";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "فشل في إضافة المصروف";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ المصروف: " + ex.Message;
                }
            }
            
            await PopulateDropDowns();
            return View(expenses);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            await PopulateDropDowns();
            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, clsExpenses expenses)
        {
            if (id != expenses.ID)
            {
                return NotFound();
            }
            ModelState.Remove("User");
            ModelState.Remove("ExpenseType");
            ModelState.Remove("ActionByUser");
            if (ModelState.IsValid)
            {
                try
                {
                    expenses.ActionDate = DateTime.Now;
                    expenses.ActionByUser =  _currentUserService.GetCurrentUserId();

                    var result = await _expenseService.UpdateExpenseAsync(expenses);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "تم تحديث المصروف بنجاح";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "فشل في تحديث المصروف";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث المصروف: " + ex.Message;
                }
            }
            
            await PopulateDropDowns();
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetExpenseByIdBALDTOAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _expenseService.DeleteExpenseAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف المصروف بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف المصروف";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف المصروف: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var expense = await _expenseService.GetExpenseByIdBALDTOAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/CreateExpenseType
        public IActionResult CreateExpenseType()
        {
            return View(new ExpenseTypeDTO());
        }

        // POST: Expenses/CreateExpenseType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExpenseType(ExpenseTypeDTO expenseTypeDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _expenseService.CreateExpenseTypeBALDTOAsync(expenseTypeDTO);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "تم إضافة نوع المصروف بنجاح";
                        return RedirectToAction(nameof(ExpenseTypes));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "فشل في إضافة نوع المصروف";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ نوع المصروف: " + ex.Message;
                }
            }
            
            return View(expenseTypeDTO);
        }

        // GET: Expenses/EditExpenseType/5
        public async Task<IActionResult> EditExpenseType(int id)
        {
            var expenseType = await _expenseService.GetExpenseTypeByIdBALDTOAsync(id);
            if (expenseType == null)
            {
                return NotFound();
            }

            return View(expenseType);
        }

        // POST: Expenses/EditExpenseType/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExpenseType(int id, ExpenseTypeDTO expenseTypeDTO)
        {
            if (id != expenseTypeDTO.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _expenseService.UpdateExpenseTypeBALDTOAsync(expenseTypeDTO);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "تم تحديث نوع المصروف بنجاح";
                        return RedirectToAction(nameof(ExpenseTypes));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "فشل في تحديث نوع المصروف";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث نوع المصروف: " + ex.Message;
                }
            }
            
            return View(expenseTypeDTO);
        }

        // GET: Expenses/DeleteExpenseType/5
        public async Task<IActionResult> DeleteExpenseType(int id)
        {
            var expenseType = await _expenseService.GetExpenseTypeByIdBALDTOAsync(id);
            if (expenseType == null)
            {
                return NotFound();
            }

            return View(expenseType);
        }

        // POST: Expenses/DeleteExpenseType/5
        [HttpPost, ActionName("DeleteExpenseType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExpenseTypeConfirmed(int id)
        {
            try
            {
                var result = await _expenseService.DeleteExpenseTypeAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم حذف نوع المصروف بنجاح";
                }
                else
                {
                    TempData["ErrorMessage"] = "فشل في حذف نوع المصروف";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف نوع المصروف: " + ex.Message;
            }

            return RedirectToAction(nameof(ExpenseTypes));
        }

        private async Task PopulateDropDowns()
        {
            try
            {
                // تحميل قائمة أنواع المصروفات
                var expenseTypes = await _expenseService.GetAllExpenseTypesBALDTOAsync();
                ViewBag.ExpenseTypeList = new SelectList(expenseTypes, "ID", "Name");

                // تحميل قائمة أنواع الإجراءات
                ViewBag.ActionTypeList = new SelectList(new[]
                {
                    new { Key = (byte)1, Value = "إضافة" },
                    new { Key = (byte)2, Value = "تحديث" },
                    new { Key = (byte)3, Value = "حذف" }
                }, "Key", "Value");
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، إعداد قوائم فارغة
                ViewBag.ExpenseTypeList = new SelectList(new List<ExpenseTypeDTO>(), "ID", "Name");
                ViewBag.ActionTypeList = new SelectList(new[]
                {
                    new { Key = (byte)1, Value = "إضافة" },
                    new { Key = (byte)2, Value = "تحديث" },
                    new { Key = (byte)3, Value = "حذف" }
                }, "Key", "Value");
            }
        }
    }
}
