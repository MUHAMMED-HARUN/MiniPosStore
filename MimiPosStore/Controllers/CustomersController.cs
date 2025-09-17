using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BAL.Interfaces;
 
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SharedModels.EF.DTO;


namespace MimiPosStore.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IPeopleService _peopleService;

        public CustomersController(ICustomerService customerService, IPeopleService peopleService)
        {
            _customerService = customerService;
            _peopleService = peopleService;
        }

        // GET: Customers
        public async Task<IActionResult> Index(clsCustomerFilter filter)
        {
            try
            {
                var customers = await _customerService.GetAllBALDTOAsync(filter);
                // Wrap results in BAL filter model for the view convenience
                filter.customers = customers;
                
                return View(filter);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل قائمة العملاء: " + ex.Message;
                return View(new clsCustomerFilter { customers = new List<CustomerDTO>() });
            }
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonID")] clsCustomer customer)
        {
            if (ModelState.IsValid)
            {
                // التحقق من وجود الشخص
                var person = await _peopleService.GetByIdAsync(customer.PersonID);
                if (person == null)
                {
                    TempData["ErrorMessage"] = "الشخص المحدد غير موجود";
                    return View(customer);
                }

                // التحقق من عدم وجود عميل لهذا الشخص
                var existingCustomer = await _customerService.GetByPersonIdAsync(customer.PersonID);
                if (existingCustomer != null)
                {
                    TempData["WarningMessage"] = "هذا الشخص مسجل كعميل بالفعل";
                    return View(customer);
                }

                var result = await _customerService.AddAsync(customer);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم إضافة العميل بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة العميل";
                }
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID")] clsCustomer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _customerService.UpdateAsync(customer);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم تحديث بيانات العميل بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث بيانات العميل";
                }
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetByIdAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "تم حذف العميل بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف العميل";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetByIdBALDTOAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Customers/Search
        [HttpPost]
        public async Task<IActionResult> Search(string searchType, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                TempData["WarningMessage"] = "يرجى إدخال قيمة للبحث";
                return View();
            }

            CustomerDTO customer = null;

            switch (searchType)
            {
                case "CustomerID":
                    if (int.TryParse(searchValue, out int customerId))
                    {
                        customer = await _customerService.GetByIdBALDTOAsync(customerId);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "يرجى إدخال رقم صحيح للبحث بمعرف العميل";
                        return View();
                    }
                    break;

                case "PersonID":
                    if (int.TryParse(searchValue, out int personId))
                    {
                        var existingCustomer = await _customerService.GetByPersonIdAsync(personId);
                        if (existingCustomer != null)
                        {
                            customer = await _customerService.GetByIdBALDTOAsync(existingCustomer.ID);
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
                        var existingCustomer = await _customerService.GetByPersonIdAsync(person.ID);
                        if (existingCustomer != null)
                        {
                            customer = await _customerService.GetByIdBALDTOAsync(existingCustomer.ID);
                        }
                    }
                    break;

                default:
                    TempData["ErrorMessage"] = "نوع البحث غير صحيح";
                    return View();
            }

            if (customer == null)
            {
                TempData["InfoMessage"] = "لم يتم العثور على عميل بهذه البيانات";
            }

            ViewBag.SearchResult = customer;
            return View();
        }

        // GET: Customers/AddCustomer
        public IActionResult AddCustomer()
        {
            return View();
        }

        // POST: Customers/AddCustomer
        [HttpPost]
        public async Task<IActionResult> AddCustomer(string searchType, string searchValue)
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
                // التحقق من عدم وجود عميل لهذا الشخص
                var existingCustomer = await _customerService.GetByPersonIdAsync(person.ID);
                if (existingCustomer != null)
                {
                    TempData["WarningMessage"] = "هذا الشخص مسجل كعميل بالفعل";
                }
            }

            ViewBag.SearchResult = person;
            return View();
        }

        // POST: Customers/AddCustomerConfirm
        [HttpPost]
        public async Task<IActionResult> AddCustomerConfirm(int personId)
        {
            if (personId <= 0)
            {
                TempData["ErrorMessage"] = "معرف الشخص غير صحيح";
                return RedirectToAction(nameof(AddCustomer));
            }

            var person = await _peopleService.GetByIdAsync(personId);
            if (person == null)
            {
                TempData["ErrorMessage"] = "الشخص غير موجود";
                return RedirectToAction(nameof(AddCustomer));
            }

            // التحقق من عدم وجود عميل لهذا الشخص
            var existingCustomer = await _customerService.GetByPersonIdAsync(personId);
            if (existingCustomer != null)
            {
                TempData["WarningMessage"] = "هذا الشخص مسجل كعميل بالفعل";
                return RedirectToAction(nameof(AddCustomer));
            }

            var customer = new clsCustomer { PersonID = personId };
            var result = await _customerService.AddAsync(customer);
            if (result)
            {
                TempData["SuccessMessage"] = "تم إضافة العميل بنجاح";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة العميل";
                return RedirectToAction(nameof(AddCustomer));
            }
        }
    }
}
