using DAL.EF.DTO;
using DAL.EF.Filters;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IImportOrderRepo
    {
        Task<bool> AddAsync(clsImportOrder importOrder);
        Task<bool> UpdateAsync(clsImportOrder importOrder);
        Task<bool> DeleteAsync(int ImportOrderID,string CurentUserID);
        Task<clsImportOrder> GetByIdAsync(int ImportOrderID);
        Task<List<clsImportOrder>> GetAllAsync();
        Task<bool> SetImportOrderPaymentStatusAsync(int ImportOrderID, byte PaymentStatus);


        Task<bool> AddItem(clsImportOrderItem ImportorderItem);
        Task<bool> UpdateItem(clsImportOrderItem ImportorderItem);
        Task<bool> DeleteItem(int ImportOrderItemID);
        Task<bool> DeleteItemByRange(int[] ImportOrderItemsID);     
        Task<List<clsImportOrderItem>> GetItemsByOrderID(int ImportOrderID);

        // Additional methods for ImportOrder
        Task<clsImportOrder> GetBySupplierIdAsync(int supplierID);
        Task<List<clsImportOrder>> GetBySupplierIdDTOAsync(int supplierID);
        Task<clsImportOrder> GetByIdWithItemsAsync(int importOrderID);
        Task<List<clsImportOrder>> GetAllWithItemsAsync();
        Task<List<clsImportOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<clsImportOrder>> GetByPaymentStatusAsync(byte paymentStatus);

        // DTO Methods
        Task<ImportOrderDTO> GetByIdDTOAsync(int ImportOrderID);
        Task<List<ImportOrderDTO>> GetAllDTOAsync();
        Task<List<ImportOrderDTO>> GetAllDTOAsync(clsImportOrderFilter filter);

        // ImportOrderItems DTO Methods
        Task<ImportOrderItemDTO> GetImportOrderItemByIdDTOAsync(int ImportOrderItemID);
        Task<List<ImportOrderItemDTO>> GetImportOrderItemsByOrderIdDTOAsync(int ImportOrderID);
    }
}
