using BAL;
using BAL.BALDTO;
using DAL.EF.Models;
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
        Task<ImportOrderBALDTO> GetByIdBALDTOAsync(int importOrderID);
        Task<List<ImportOrderBALDTO>> GetAllBALDTOAsync();
        Task<List<ImportOrderBALDTO>> GetBySupplierIdBALDTOAsync(int supplierID);
        Task<bool> AddBALDTOAsync(ImportOrderBALDTO importOrderBALDTO);
        Task<bool> UpdateBALDTOAsync(ImportOrderBALDTO importOrderBALDTO);
        
        // Enhanced DTO Methods with Business Logic
        Task<ImportOrderBALDTO> GetByIdWithItemsBALDTOAsync(int importOrderID);
        Task<List<ImportOrderBALDTO>> GetAllWithItemsBALDTOAsync();
        Task<List<ImportOrderBALDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate);
        Task<List<ImportOrderBALDTO>> GetByPaymentStatusBALDTOAsync(byte paymentStatus);
        Task<List<ImportOrderBALDTO>> GetUnpaidOrdersBALDTOAsync();
        Task<List<ImportOrderBALDTO>> GetPartiallyPaidOrdersBALDTOAsync();
        Task<List<ImportOrderBALDTO>> GetFullyPaidOrdersBALDTOAsync();
        
        // Summary Methods for Performance
        Task<List<ImportOrderBALDTO>> GetAllSummaryBALDTOAsync();
        Task<ImportOrderBALDTO> GetByIdSummaryBALDTOAsync(int importOrderID);
        
        // Business Logic Methods
        Task<bool> UpdatePaymentStatusAsync(int importOrderID, byte paymentStatus);
        Task<bool> AddPaymentAsync(int importOrderID, float paymentAmount);
        Task<float> GetRemainingAmountAsync(int importOrderID);
        Task<bool> IsFullyPaidAsync(int importOrderID);
        
        Task<bool> Save();
    }
}
