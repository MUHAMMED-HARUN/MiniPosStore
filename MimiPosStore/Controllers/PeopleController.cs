using BAL.Interfaces;
using DAL.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace MimiPosStore.Controllers
{
    [Authorize]

    public class PeopleController : Controller
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            var people = await _peopleService.GetAllAsync();
            return View(people);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber")] clsPerson person)
        {
            if (ModelState.IsValid)
            {
                var result = await _peopleService.AddAsync(person);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم إضافة الشخص بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة الشخص";
                }
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _peopleService.GetByIdAsync(id.Value);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber")] clsPerson person)
        {
            if (id != person.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _peopleService.UpdateAsync(person);
                if (result)
                {
                    TempData["SuccessMessage"] = "تم تحديث بيانات الشخص بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث بيانات الشخص";
                }
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _peopleService.GetByIdAsync(id.Value);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _peopleService.DeleteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "تم حذف الشخص بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الشخص";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _peopleService.GetByIdAsync(id.Value);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: People/Search
        [HttpPost]
        public async Task<IActionResult> Search(string searchType, string searchValue)
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

            ViewBag.SearchResult = person;
            return View();
        }
    }
}
