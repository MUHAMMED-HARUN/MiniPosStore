using SharedModels.EF.DTO;
using SharedModels.EF.Models;


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
        Task<ImportOrderItemDTO> GetByIdBALDTOAsync(int importOrderItemID);
        Task<List<ImportOrderItemDTO>> GetAllBALDTOAsync();
        Task<List<ImportOrderItemDTO>> GetByImportOrderIdBALDTOAsync(int importOrderID);
        Task<List<ImportOrderItemDTO>> GetByProductIdBALDTOAsync(int productID);
        Task<bool> AddBALDTOAsync(ImportOrderItemDTO ImportOrderItemDTO);
        Task<bool> UpdateBALDTOAsync(ImportOrderItemDTO ImportOrderItemDTO);
        Task<bool> UpdateBALDTOAsync(clsImportOrderItem ImportOrderItem);

        // Enhanced DTO Methods with Business Logic
        Task<List<ImportOrderItemDTO>> GetHighValueItemsBALDTOAsync(float minAmount = 1000);
        Task<List<ImportOrderItemDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate);
        Task<List<ImportOrderItemDTO>> GetByCurrencyTypeBALDTOAsync(string currencyType);
        Task<List<ImportOrderItemDTO>> GetByUOMBALDTOAsync(string uomName);
        
        // Summary Methods for Performance
        
        // Business Logic Methods
        Task<float> GetTotalAmountByImportOrderAsync(int importOrderID);
        Task<float> GetTotalAmountByProductAsync(int productID);
        Task<int> GetItemsCountByImportOrderAsync(int importOrderID);
        Task<bool> HasProductInImportOrderAsync(int importOrderID, int productID);
        Task<bool> UpdateQuantityAsync(int importOrderItemID, float newQuantity);
        Task<bool> UpdateSellingPriceAsync(int importOrderItemID, float newSellingPrice);
        
        // Bulk Operations
        Task<bool> AddMultipleItemsAsync(List<clsImportOrderItem> importOrderItems);
        Task<bool> AddMultipleItemsBALDTOAsync(List<ImportOrderItemDTO> ImportOrderItemDTOs);
        Task<bool> DeleteMultipleItemsAsync(int[] importOrderItemIDs);
        
        Task<bool> Save();
    }
}
