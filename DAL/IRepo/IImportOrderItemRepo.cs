using DAL.EF.DTO;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IImportOrderItemRepo
    {
        // Basic CRUD Operations
        Task<bool> AddAsync(clsImportOrderItem importOrderItem);
        Task<bool> UpdateAsync(clsImportOrderItem importOrderItem);
        Task<bool> DeleteAsync(int importOrderItemID);
        Task<clsImportOrderItem> GetByIdAsync(int importOrderItemID);
        Task<List<clsImportOrderItem>> GetAllAsync();
        Task<List<clsImportOrderItem>> GetByImportOrderIdAsync(int importOrderID);
        Task<List<clsImportOrderItem>> GetByProductIdAsync(int productID);

        // DTO Methods
        Task<ImportOrderItemDTO> GetByIdDTOAsync(int importOrderItemID);
        Task<List<ImportOrderItemDTO>> GetAllDTOAsync();
        Task<List<ImportOrderItemDTO>> GetByImportOrderIdDTOAsync(int importOrderID);
        Task<List<ImportOrderItemDTO>> GetByProductIdDTOAsync(int productID);
        Task<List<ImportOrderItemDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<ImportOrderItemDTO>> GetByCurrencyTypeAsync(string currencyType);
        Task<List<ImportOrderItemDTO>> GetByUOMAsync(string uomName);
        Task<List<ImportOrderItemDTO>> GetHighValueItemsAsync(float minAmount = 1000);

        // Summary DTO Methods
        Task<ImportOrderItemDTO> GetByIdSummaryDTOAsync(int importOrderItemID);
        Task<List<ImportOrderItemDTO>> GetAllSummaryDTOAsync();
        Task<List<ImportOrderItemDTO>> GetByImportOrderIdSummaryDTOAsync(int importOrderID);
    }
}

