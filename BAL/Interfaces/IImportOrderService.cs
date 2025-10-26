using BAL;
using BAL.Events.ImportOrderEvents;
using BAL.Events;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IImportOrderService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsImportOrder importOrder { get; set; }
        
        // Events
        event AsyncEventHandler<ImportOrderConfirmedEventArgs> ImportOrderConfirmedEvent;
        event AsyncEventHandler<ImportOrderItemUnionAddedEventArgs> ImportOrderItemUnionAddedEvent;
        event AsyncEventHandler<ImportOrderItemUnionUpdatedEventArgs> ImportOrderItemUnionUpdatedEvent;
        event AsyncEventHandler<ImportOrderItemUnionDeletedEventArgs> ImportOrderItemUnionDeletedEvent;
        
        // Basic CRUD Operations
        Task<bool> AddAsync(clsImportOrder importOrder);
        Task<bool> UpdateAsync(clsImportOrder importOrder);
        Task<bool> DeleteAsync(int importOrderID);
        Task<clsImportOrder> GetByIdAsync(int importOrderID);
        Task<List<clsImportOrder>> GetAllAsync();
        Task<clsImportOrder> GetBySupplierIdAsync(int supplierID);
        
        // BALDTO Methods
        Task<ImportOrderDTO> GetByIdBALDTOAsync(int importOrderID);
        Task<List<ImportOrderDTO>> GetAllBALDTOAsync();
        Task<List<ImportOrderDTO>> GetBySupplierIdBALDTOAsync(int supplierID);
        Task<bool> AddBALDTOAsync(ImportOrderDTO ImportOrderDTO);
        Task<bool> UpdateBALDTOAsync(ImportOrderDTO ImportOrderDTO);
        
        // Enhanced DTO Methods with Business Logic
        Task<ImportOrderDTO> GetByIdWithItemsBALDTOAsync(int importOrderID);
        Task<List<ImportOrderDTO>> GetAllWithItemsBALDTOAsync();
        Task<List<ImportOrderDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate);
        Task<List<ImportOrderDTO>> GetByPaymentStatusBALDTOAsync(byte paymentStatus);
        Task<List<ImportOrderDTO>> GetUnpaidOrdersBALDTOAsync();
        Task<List<ImportOrderDTO>> GetPartiallyPaidOrdersBALDTOAsync();
        Task<List<ImportOrderDTO>> GetFullyPaidOrdersBALDTOAsync();
        
        // Summary Methods for Performance
        Task<List<ImportOrderDTO>> GetAllSummaryBALDTOAsync();
        Task<List<ImportOrderDTO>> GetAllSummaryBALDTOAsync(clsImportOrderFilter filter);
        Task<ImportOrderDTO> GetByIdSummaryBALDTOAsync(int importOrderID);
        
        // Business Logic Methods
        Task<bool> UpdatePaymentStatusAsync(int importOrderID, byte paymentStatus);
        Task<bool> AddPaymentAsync(int importOrderID, float paymentAmount);
        Task<float> GetRemainingAmountAsync(ImportOrderDTO importOrderID);
        Task<bool> IsFullyPaidAsync(int importOrderID);
        
        Task<bool> Save();
        
        // Import Order Item Union Methods
        Task<List<ImportOrderItemUnionDTO>> GetImportOrderItemUnionDTOs(clsImportOrderItemUnionFilter filter);
        
        // Event Raisers
        Task OnImportOrderConfirmed(clsImportOrder importOrder);
        Task OnImportOrderItemUnionAdded(ImportOrderItemUnionDTO importOrderItem);
        Task OnImportOrderItemUnionUpdated(ImportOrderItemUnionDTO oldImportOrderItem, ImportOrderItemUnionDTO newImportOrderItem);
        Task OnImportOrderItemUnionDeleted(ImportOrderItemUnionDTO importOrderItem);
        
        // Import Order Item Management
        Task<bool> AddItem(clsImportOrderItem importOrderItem);
        Task<bool> AddRawMaterialItem(clsImportRawMaterialItem rawMaterialItem);
        Task<bool> UpdateItem(clsImportOrderItem importOrderItem);
        Task<bool> UpdateRawMaterialItem(clsImportRawMaterialItem rawMaterialItem);
        Task<bool> DeleteItem(int importOrderItemID);
        Task<bool> DeleteRawMaterialItem(int rawMaterialItemID);
        Task<List<clsImportOrderItem>> GetItemsByImportOrderID(int importOrderID);
        Task<List<clsImportRawMaterialItem>> GetRawMaterialItemsByImportOrderID(int importOrderID);
        Task<clsImportRawMaterialItem> GetRawMaterialItemByIdAsync(int id);
    }
}
