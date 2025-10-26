using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    /// <summary>
    /// Interface for Reports Service - handles all business logic for reports generation
    /// Follows SOLID principles and provides clear contract for reports functionality
    /// </summary>
    public interface IReportsService
    {
        /// <summary>
        /// Get total sales amount for orders within specified date range
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Total sales amount</returns>
        Task<float> GetOrderSalesAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get net profit from orders within specified date range
        /// Calculates profit from both regular products and raw materials
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Total net profit from orders</returns>
        Task<float> GetOrderNetProfitAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get remaining debt from orders within specified date range
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Total remaining debt amount</returns>
        Task<float> GetRemainingOrderDebtAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get total cost of import orders within specified date range
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Total import orders cost</returns>
        Task<float> GetImportOrderCostAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get remaining debt from import orders within specified date range
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Total remaining import debt amount</returns>
        Task<float> GetRemainingImportDebtAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get total expenses within specified date range
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Total expenses amount</returns>
        Task<float> GetTotalExpensesAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get store net profit within specified date range
        /// Calculates: Order Net Profit - Total Expenses
        /// </summary>
        /// <param name="startDate">Start date for the report period</param>
        /// <param name="endDate">End date for the report period</param>
        /// <returns>Store net profit amount</returns>
        Task<float> GetStoreNetProfitAsync(DateTime startDate, DateTime endDate);
    }
}
