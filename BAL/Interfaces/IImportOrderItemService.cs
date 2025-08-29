using DAL.EF.Models;
using BAL.BALDTO;

namespace BAL.Interfaces
{
    public interface IImportOrderItemService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsImportOrderItem importOrderItem { get; set; }
        
        // Basic CRUD Operations
        Task<bool> AddAsync(clsImportOrderItem importOrderItem);
        Task<bool> UpdateAsync(clsImportOrderItem importOrderItem);
        Task<bool> DeleteAsync(int importOrderItemID);
        Task<clsImportOrderItem> GetByIdAsync(int importOrderItemID);
        Task<List<clsImportOrderItem>> GetAllAsync();
        Task<List<clsImportOrderItem>> GetByImportOrderIdAsync(int importOrderID);
        Task<List<clsImportOrderItem>> GetByProductIdAsync(int productID);
        
        // BALDTO Methods
        Task<ImportOrderItemBALDTO> GetByIdBALDTOAsync(int importOrderItemID);
        Task<List<ImportOrderItemBALDTO>> GetAllBALDTOAsync();
        Task<List<ImportOrderItemBALDTO>> GetByImportOrderIdBALDTOAsync(int importOrderID);
        Task<List<ImportOrderItemBALDTO>> GetByProductIdBALDTOAsync(int productID);
        Task<bool> AddBALDTOAsync(ImportOrderItemBALDTO importOrderItemBALDTO);
        Task<bool> UpdateBALDTOAsync(ImportOrderItemBALDTO importOrderItemBALDTO);
        
        // Enhanced DTO Methods with Business Logic
        Task<List<ImportOrderItemBALDTO>> GetHighValueItemsBALDTOAsync(float minAmount = 1000);
        Task<List<ImportOrderItemBALDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate);
        Task<List<ImportOrderItemBALDTO>> GetByCurrencyTypeBALDTOAsync(string currencyType);
        Task<List<ImportOrderItemBALDTO>> GetByUOMBALDTOAsync(string uomName);
        
        // Summary Methods for Performance
        Task<List<ImportOrderItemBALDTO>> GetAllSummaryBALDTOAsync();
        Task<ImportOrderItemBALDTO> GetByIdSummaryBALDTOAsync(int importOrderItemID);
        Task<List<ImportOrderItemBALDTO>> GetByImportOrderIdSummaryBALDTOAsync(int importOrderID);
        
        // Business Logic Methods
        Task<float> GetTotalAmountByImportOrderAsync(int importOrderID);
        Task<float> GetTotalAmountByProductAsync(int productID);
        Task<int> GetItemsCountByImportOrderAsync(int importOrderID);
        Task<bool> HasProductInImportOrderAsync(int importOrderID, int productID);
        Task<bool> UpdateQuantityAsync(int importOrderItemID, float newQuantity);
        Task<bool> UpdateSellingPriceAsync(int importOrderItemID, float newSellingPrice);
        
        // Bulk Operations
        Task<bool> AddMultipleItemsAsync(List<clsImportOrderItem> importOrderItems);
        Task<bool> AddMultipleItemsBALDTOAsync(List<ImportOrderItemBALDTO> importOrderItemBALDTOs);
        Task<bool> DeleteMultipleItemsAsync(int[] importOrderItemIDs);
        
        Task<bool> Save();
    }
}
