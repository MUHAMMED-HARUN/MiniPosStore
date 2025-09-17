using SharedModels.EF.DTO;
using SharedModels.EF.Models;
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


 
        Task<List<ImportOrderItemDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<ImportOrderItemDTO>> GetByCurrencyTypeAsync(string currencyType);
        Task<List<ImportOrderItemDTO>> GetByUOMAsync(string uomName);
        Task<List<ImportOrderItemDTO>> GetHighValueItemsAsync(float minAmount = 1000);




    }
}

