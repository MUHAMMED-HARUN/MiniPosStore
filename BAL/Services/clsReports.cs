using BAL.Interfaces;
using DAL.IRepoServ;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BAL.Services
{
    /// <summary>
    /// Professional Reports Service implementing clean business logic
    /// Follows SOLID principles and provides comprehensive error handling
    /// </summary>
    public class ReportsService : IReportsService
    {
        private readonly clsReportsRepo _reportsRepository;
        private readonly ILogger<ReportsService> _logger;

        public ReportsService(clsReportsRepo reportsRepository, ILogger<ReportsService> logger)
        {
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<float> GetOrderSalesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Fetching order sales from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.OrderSales(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order sales for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<float> GetOrderNetProfitAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Calculating order net profit from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.TotalNetProfitreportOrders(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating order net profit for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<float> GetRemainingOrderDebtAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Fetching remaining order debt from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.RemainingOrderDebt(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remaining order debt for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<float> GetImportOrderCostAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Fetching import order costs from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.ImportOrderReport(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting import order costs for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<float> GetRemainingImportDebtAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Fetching remaining import debt from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.RemainingImportOrderDebt(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting remaining import debt for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<float> GetTotalExpensesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Fetching total expenses from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.GetTotalExpenses(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total expenses for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<float> GetStoreNetProfitAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                ValidateDateRange(startDate, endDate);
                _logger.LogInformation("Calculating store net profit from {StartDate} to {EndDate}", startDate, endDate);
                
                return await _reportsRepository.GetStoreNetProfit(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating store net profit for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        /// <summary>
        /// Validates that the date range is logical and not in the future
        /// </summary>
        private static void ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be later than end date");

            if (startDate > DateTime.Today)
                throw new ArgumentException("Start date cannot be in the future");

            if (endDate > DateTime.Today)
                throw new ArgumentException("End date cannot be in the future");
        }
    }
}
