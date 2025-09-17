using BAL;
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
    }
}
