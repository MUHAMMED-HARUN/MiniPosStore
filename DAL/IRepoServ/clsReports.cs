using DAL.EF.AppDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    /// <summary>
    /// Professional Reports Repository using IServiceScopeFactory for parallel operations
    /// This approach provides better control over DbContext lifecycle and scope management
    /// </summary>
    public class clsReportsRepo
    {
        private readonly AppDBContext _dBContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public clsReportsRepo(AppDBContext dBContext, IServiceScopeFactory scopeFactory)
        {
            _dBContext = dBContext ?? throw new ArgumentNullException(nameof(dBContext));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }
        public async Task<float> OrderSales(DateTime startDate, DateTime endDate)
        {
            float totalSales = await _dBContext.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).Select(o => o.TotalAmount).SumAsync();

            return totalSales;
        }
        /// <summary>
        /// Calculate total net profit from orders using parallel processing
        /// Thread-safe implementation with proper DbContext scope management
        /// </summary>
        public async Task<float> TotalNetProfitreportOrders(DateTime startDate, DateTime endDate)
        {
            var OrdersID = await _dBContext.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Select(o => o.ID)
                .ToListAsync();

            if (!OrdersID.Any())
                return 0;

            // Thread-safe parallel execution using proper Task return values
            var itemProfitTask = GetItemProfit(OrdersID);
            var materialProfitTask = GetMaterialProfit(OrdersID);

            // انتظار انتهاء المهام وجمع النتائج بطريقة thread-safe
            var itemProfit = await itemProfitTask;
            var materialProfit = await materialProfitTask;
            
            return itemProfit + (float)materialProfit;
        }
        public async Task<float> RemainingOrderDebt(DateTime startDate, DateTime endDate)
        {
            float RemainingDebt = 0;
            RemainingDebt = await _dBContext.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).
             Select(o => o.TotalAmount - o.PaidAmount).SumAsync();
            return RemainingDebt;
        }
        /// <summary>
        /// Professional thread-safe implementation using IServiceScopeFactory
        /// Each Task gets its own DbContext scope for complete isolation
        /// </summary>
        public async Task<float> ImportOrderReport(DateTime startDate, DateTime endDate)
        {
            var importOrderIds = await _dBContext.ImportOrders
                .Where(io => io.ImportDate >= startDate && io.ImportDate <= endDate)
                .Select(io => io.ID)
                .ToListAsync();

            if (!importOrderIds.Any())
                return 0;

            // Thread-safe parallel execution with proper scope management
            var materialCostTask = GetImportMaterialCost(importOrderIds);
            var productCostTask = GetImportProductCost(importOrderIds);

            // انتظار انتهاء المهام وجمع النتائج بطريقة thread-safe
            var materialCost = await materialCostTask;
            var productCost = await productCostTask;
            
            return (float)(materialCost + productCost);
        }
        public async Task<float> RemainingImportOrderDebt(DateTime startDate, DateTime endDate)
        {
            float totalPaidImports = await _dBContext.ImportOrders
                .Where(io => io.ImportDate >= startDate && io.ImportDate <= endDate)
                .Select(io =>io.TotalAmount- io.PaidAmount)
                .SumAsync();
            return totalPaidImports;
        }
        public async Task<float> GetTotalExpenses(DateTime startDate, DateTime endDate)
        {
            float totalExpenses = await _dBContext.Expenses
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .Select(e => e.Amount)
                .SumAsync();
            return totalExpenses;
        }
        public async Task<float> GetStoreNetProfit(DateTime startDate, DateTime endDate)
        {
            float orderNetProfit = await TotalNetProfitreportOrders(startDate, endDate);
            float totalExpenses = await GetTotalExpenses(startDate, endDate);
            float OrderDebt =await RemainingOrderDebt(startDate, endDate);
            float storeNetProfit = orderNetProfit - totalExpenses;
            return storeNetProfit;
        }

        #region Private Helper Methods for Thread-Safe Operations

        /// <summary>
        /// Get item profit using separate DbContext scope
        /// </summary>
        private async Task<float> GetItemProfit(List<int> orderIds)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            return await context.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderID))
                .Select(oi => (oi.Quantity * (oi.SellingPrice - oi.WholesalePrice)) - (oi.PriceAdjustment ?? 0))
                .SumAsync();
        }

        /// <summary>
        /// Get material profit using separate DbContext scope
        /// </summary>
        private async Task<double> GetMaterialProfit(List<int> orderIds)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            return await context.RawMaterialOrderItems
                .Where(oi => orderIds.Contains(oi.OrderID))
                .Select(oi => oi.Quantity * (oi.SellingPrice - oi.WholesalePrice))
                .SumAsync();
        }

        /// <summary>
        /// Get import material cost using separate DbContext scope
        /// </summary>
        private async Task<double> GetImportMaterialCost(List<int> importOrderIds)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            return await context.ImportRawMaterialItems
                .Where(rmoi => importOrderIds.Contains(rmoi.ImportOrderID))
                .Select(rmoi => rmoi.Quantity * rmoi.SellingPrice)
                .SumAsync();
        }

        /// <summary>
        /// Get import product cost using separate DbContext scope
        /// </summary>
        private async Task<float> GetImportProductCost(List<int> importOrderIds)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            return await context.ImportOrderItems
                .Where(poi => importOrderIds.Contains(poi.ImportOrderID))
                .Select(poi => poi.Quantity * poi.SellingPrice)
                .SumAsync();
        }

        #endregion
    }
}
