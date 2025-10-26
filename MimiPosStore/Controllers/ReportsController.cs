using BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimiPosStore.Models;
using System;
using System.Threading.Tasks;

namespace MimiPosStore.Controllers
{
    /// <summary>
    /// Professional Reports Controller with comprehensive error handling and clean architecture
    /// Follows Controller best practices and provides excellent user experience
    /// </summary>
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportsService _reportsService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportsService reportsService, ILogger<ReportsController> logger)
        {
            _reportsService = reportsService ?? throw new ArgumentNullException(nameof(reportsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Main reports dashboard page
        /// </summary>
        public IActionResult Index()
        {
            try
            {
                var model = new ReportsViewModel();
                _logger.LogInformation("Reports dashboard accessed by user");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reports dashboard");
                TempData["ErrorMessage"] = "حدث خطأ في تحميل صفحة التقارير";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Generate comprehensive reports for specified date range
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReports(ReportsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.ErrorMessage = "الرجاء التحقق من البيانات المدخلة";
                    return View("Index", model);
                }

                if (!model.IsValidDateRange())
                {
                    ModelState.AddModelError("", "نطاق التاريخ غير صحيح");
                    model.ErrorMessage = "تاريخ البداية يجب أن يكون قبل تاريخ النهاية ولا يمكن أن يكون في المستقبل";
                    return View("Index", model);
                }

                model.IsLoading = true;
                _logger.LogInformation("Generating reports for date range: {StartDate} to {EndDate}", 
                    model.StartDate, model.EndDate);

                // Fetch all reports data in parallel for better performance

                model.OrderSales = await _reportsService.GetOrderSalesAsync(model.StartDate, model.EndDate);
                model.OrderNetProfit = await _reportsService.GetOrderNetProfitAsync(model.StartDate, model.EndDate);
                model.RemainingOrderDebt = await _reportsService.GetRemainingOrderDebtAsync(model.StartDate, model.EndDate);
                model.ImportOrderCost = await _reportsService.GetImportOrderCostAsync(model.StartDate, model.EndDate);
                model.RemainingImportDebt = await _reportsService.GetRemainingImportDebtAsync(model.StartDate, model.EndDate);
                model.TotalExpenses = await _reportsService.GetTotalExpensesAsync(model.StartDate, model.EndDate);
                model.StoreNetProfit = await _reportsService.GetStoreNetProfitAsync(model.StartDate, model.EndDate);
           



                model.IsLoading = false;
                model.HasData = true;

                _logger.LogInformation("Reports generated successfully for date range: {StartDate} to {EndDate}", 
                    model.StartDate, model.EndDate);

                TempData["SuccessMessage"] = "تم إنشاء التقارير بنجاح";
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating reports for date range: {StartDate} to {EndDate}", 
                    model.StartDate, model.EndDate);

                model.IsLoading = false;
                model.HasData = false;
                model.ErrorMessage = "حدث خطأ في إنشاء التقارير. الرجاء المحاولة مرة أخرى.";
                
                TempData["ErrorMessage"] = "حدث خطأ في إنشاء التقارير";
                return View("Index", model);
            }
        }

        /// <summary>
        /// AJAX endpoint for getting reports data
        /// Provides better user experience with real-time data loading
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GetReportsData([FromBody] ReportsRequest request)
        {
            try
            {
                if (request == null || request.StartDate > request.EndDate)
                {
                    return Json(new ReportsResponse 
                    { 
                        Success = false, 
                        Message = "نطاق التاريخ غير صحيح" 
                    });
                }

                _logger.LogInformation("AJAX request for reports data: {StartDate} to {EndDate}", 
                    request.StartDate, request.EndDate);

                var model = new ReportsViewModel
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                };

                // Fetch all data in parallel
                var tasks = new[]
                {
                    _reportsService.GetOrderSalesAsync(request.StartDate, request.EndDate),
                    _reportsService.GetOrderNetProfitAsync(request.StartDate, request.EndDate),
                    _reportsService.GetRemainingOrderDebtAsync(request.StartDate, request.EndDate),
                    _reportsService.GetImportOrderCostAsync(request.StartDate, request.EndDate),
                    _reportsService.GetRemainingImportDebtAsync(request.StartDate, request.EndDate),
                    _reportsService.GetTotalExpensesAsync(request.StartDate, request.EndDate),
                    _reportsService.GetStoreNetProfitAsync(request.StartDate, request.EndDate)
                };

                var results = await Task.WhenAll(tasks);

                model.OrderSales = results[0];
                model.OrderNetProfit = results[1];
                model.RemainingOrderDebt = results[2];
                model.ImportOrderCost = results[3];
                model.RemainingImportDebt = results[4];
                model.TotalExpenses = results[5];
                model.StoreNetProfit = results[6];
                model.HasData = true;

                return Json(new ReportsResponse 
                { 
                    Success = true, 
                    Message = "تم جلب البيانات بنجاح",
                    Data = model 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AJAX reports request");
                return Json(new ReportsResponse 
                { 
                    Success = false, 
                    Message = "حدث خطأ في جلب البيانات" 
                });
            }
        }

        /// <summary>
        /// Quick reports for today, this week, this month
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> QuickReport(string period = "today")
        {
            try
            {
                DateTime startDate, endDate;
                
                switch (period.ToLower())
                {
                    case "today":
                        startDate = endDate = DateTime.Today;
                        break;
                    case "week":
                        startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                        endDate = DateTime.Today;
                        break;
                    case "month":
                        startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        endDate = DateTime.Today;
                        break;
                    default:
                        return BadRequest("فترة غير صحيحة");
                }

                var model = new ReportsViewModel
                {
                    StartDate = startDate,
                    EndDate = endDate
                };

                // This would trigger the report generation
                return RedirectToAction("GenerateReports", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating quick report for period: {Period}", period);
                TempData["ErrorMessage"] = "حدث خطأ في إنشاء التقرير السريع";
                return RedirectToAction("Index");
            }
        }
    }
}
