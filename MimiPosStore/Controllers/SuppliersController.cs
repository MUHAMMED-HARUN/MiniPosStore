using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BAL.Interfaces;
using BAL.BALDTO;
using DAL.EF.Models;
using System.Threading.Tasks;

namespace MimiPosStore.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IPeopleService _peopleService;

        public SuppliersController(ISupplierService supplierService, IPeopleService peopleService)
        {
            _supplierService = supplierService;
            _peopleService = peopleService;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllBALDTOAsync();
            return View(suppliers);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonID,StoreName,StoreAddress")] clsSupplier supplier)
        {
            if (ModelState.IsValid)
            {
                // التحقق من وجود الشخص
                var person = await _peopleService.GetByIdAsync(supplier.PersonID);
                if (person == null)
                {
                    TempData["ErrorMessage"] = "الشخص المحدد غير موجود";
                    return View(supplier);
                }

                // التحقق من عدم وجود مورد لهذا الشخص
                var existingSupplier = await _supplierService.GetByPersonIdAsync(supplier.PersonID);
                if (existingSupplier != null)
                {
                    TempData["WarningMessage"] = "هذا الشخص مسجل كمورد بالفعل";
                    return View(supplier);
                }

                var result = await _supplierService.AddAsync(supplier);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم إضافة المورد بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة المورد";
                }
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _supplierService.GetByIdAsync(id.Value);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,StoreName,StoreAddress")] clsSupplier supplier)
        {
            if (id != supplier.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _supplierService.UpdateAsync(supplier);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم تحديث بيانات المورد بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث بيانات المورد";
                }
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _supplierService.GetByIdAsync(id.Value);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _supplierService.DeleteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "تم حذف المورد بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف المورد";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _supplierService.GetByIdBALDTOAsync(id.Value);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Suppliers/Search
        [HttpPost]
        public async Task<IActionResult> Search(string searchType, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                TempData["WarningMessage"] = "يرجى إدخال قيمة للبحث";
                return View();
            }

            SupplierBALDTO supplier = null;

            switch (searchType)
            {
                case "SupplierID":
                    if (int.TryParse(searchValue, out int supplierId))
                    {
                        supplier = await _supplierService.GetByIdBALDTOAsync(supplierId);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "يرجى إدخال رقم صحيح للبحث بمعرف المورد";
                        return View();
                    }
                    break;

                case "PersonID":
                    if (int.TryParse(searchValue, out int personId))
                    {
                        var existingSupplier = await _supplierService.GetByPersonIdAsync(personId);
                        if (existingSupplier != null)
                        {
                            supplier = await _supplierService.GetByIdBALDTOAsync(existingSupplier.ID);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "يرجى إدخال رقم صحيح للبحث بمعرف الشخص";
                        return View();
                    }
                    break;

                case "PhoneNumber":
                    var person = await _peopleService.GetByPhoneNumberAsync(searchValue);
                    if (person != null)
                    {
                        var existingSupplier = await _supplierService.GetByPersonIdAsync(person.ID);
                        if (existingSupplier != null)
                        {
                            supplier = await _supplierService.GetByIdBALDTOAsync(existingSupplier.ID);
                        }
                    }
                    break;

                default:
                    TempData["ErrorMessage"] = "نوع البحث غير صحيح";
                    return View();
            }

            if (supplier == null)
            {
                TempData["InfoMessage"] = "لم يتم العثور على مورد بهذه البيانات";
            }

            ViewBag.SearchResult = supplier;
            return View();
        }

        // GET: Suppliers/AddSupplier
        public IActionResult AddSupplier()
        {
            return View();
        }

        // POST: Suppliers/AddSupplier
        [HttpPost]
        public async Task<IActionResult> AddSupplier(string searchType, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                TempData["WarningMessage"] = "يرجى إدخال قيمة للبحث";
                return View();
            }

            clsPerson person = null;

            switch (searchType)
            {
                case "ID":
                    if (int.TryParse(searchValue, out int id))
                    {
                        person = await _peopleService.GetByIdAsync(id);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "يرجى إدخال رقم صحيح للبحث بالمعرف";
                        return View();
                    }
                    break;

                case "PhoneNumber":
                    person = await _peopleService.GetByPhoneNumberAsync(searchValue);
                    break;

                default:
                    TempData["ErrorMessage"] = "نوع البحث غير صحيح";
                    return View();
            }

            if (person == null)
            {
                TempData["InfoMessage"] = "لم يتم العثور على شخص بهذه البيانات";
            }
            else
            {
                // التحقق من عدم وجود مورد لهذا الشخص
                var existingSupplier = await _supplierService.GetByPersonIdAsync(person.ID);
                if (existingSupplier != null)
                {
                    TempData["WarningMessage"] = "هذا الشخص مسجل كمورد بالفعل";
                }
            }

            ViewBag.SearchResult = person;
            return View();
        }

        // POST: Suppliers/AddSupplierConfirm
        [HttpPost]
        public async Task<IActionResult> AddSupplierConfirm(int personId, string storeName, string storeAddress)
        {
            if (personId <= 0)
            {
                TempData["ErrorMessage"] = "معرف الشخص غير صحيح";
                return RedirectToAction(nameof(AddSupplier));
            }

            var person = await _peopleService.GetByIdAsync(personId);
            if (person == null)
            {
                TempData["ErrorMessage"] = "الشخص غير موجود";
                return RedirectToAction(nameof(AddSupplier));
            }

            // التحقق من عدم وجود مورد لهذا الشخص
            var existingSupplier = await _supplierService.GetByPersonIdAsync(personId);
            if (existingSupplier != null)
            {
                TempData["WarningMessage"] = "هذا الشخص مسجل كمورد بالفعل";
                return RedirectToAction(nameof(AddSupplier));
            }

            var supplier = new clsSupplier 
            { 
                PersonID = personId,
                StoreName = storeName,
                StoreAddress = storeAddress
            };
            var result = await _supplierService.AddAsync(supplier);
            if (result)
            {
                TempData["SuccessMessage"] = "تم إضافة المورد بنجاح";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة المورد";
                return RedirectToAction(nameof(AddSupplier));
            }
        }
    }
}
