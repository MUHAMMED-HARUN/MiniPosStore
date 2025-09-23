using BAL.Interfaces;
using DAL.EF.AppDBContext;
using DAL.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimiPosStore.Models;
using SharedModels.EF.Models;
using SharedModels.EF.SP;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MimiPosStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDBContext Context;
        IProductService productService;
        public HomeController(ILogger<HomeController> logger,AppDBContext appDB,IProductService product)
        {
            _logger = logger;
            Context = appDB;
            productService = product;
        }
        
        public async Task<IActionResult> Index()
        {
            try
            {
                // إحصائيات سريعة
                var today = DateTime.Today;

                var ordersCount = await Context.Orders
                    .Where(o => o.OrderDate.Date == today)
                    .CountAsync();

                var productsCount = await Context.Products.CountAsync();
                var customersCount = await Context.Customers.CountAsync();
                var suppliersCount = await Context.Suppliers.CountAsync();
                var importOrdersCount = await Context.ImportOrders.Where(o => o.ImportDate.Date == today).CountAsync();

                var todaySales = await Context.Orders
                    .Where(o => o.OrderDate.Date == today)
                    .SumAsync(o => o.TotalAmount);

                double NetProfit = await productService.GetNetProfit(new clsNetProfit_SP { TargetDate=DateTime.Now});





                ViewBag.TodayOrders = ordersCount;
                ViewBag.ProductsCount = productsCount;
                ViewBag.CustomersCount = customersCount;
                ViewBag.NetProfit = NetProfit;
                ViewBag.ImportOrdersCount = importOrdersCount;
                ViewBag.TodaySales = todaySales;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحميل الإحصائيات");
                ViewBag.TodayOrders = 0;
                ViewBag.ProductsCount = 0;
                ViewBag.CustomersCount = 0;
                ViewBag.SuppliersCount = 0;
                ViewBag.ImportOrdersCount = 0;
                ViewBag.TodaySales = 0;
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
