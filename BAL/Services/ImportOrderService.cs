using SharedModels.EF.Models;
using DAL.IRepo;
using BAL.Interfaces;

using BAL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels.EF.Filters;
using SharedModels.EF.DTO;

namespace BAL.Services
{
    public class ImportOrderService : IImportOrderService
    {
        private readonly IImportOrderRepo _importOrderRepo;
        private readonly ICurrentUserService _currentUserService;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsImportOrder importOrder { get; set; }

        public ImportOrderService(IImportOrderRepo importOrderRepo, ICurrentUserService currentUserService)
        {
            _importOrderRepo = importOrderRepo;
            _currentUserService = currentUserService;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        // Basic CRUD Operations
        public async Task<bool> AddAsync(clsImportOrder importOrder)
        {
            try
            {
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                importOrder.ActionType = 0; // Add
                importOrder.ActionDate = DateTime.Now;
                importOrder.ImportDate = DateTime.Now;
                
                return await _importOrderRepo.AddAsync(importOrder);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsImportOrder importOrder)
        {
            try
            {
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                importOrder.ActionType = 1; // Update
                importOrder.ActionDate = DateTime.Now;
                
                return await _importOrderRepo.UpdateAsync(importOrder);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int importOrderID)
        {
            try
            {
                string currentUserId = _currentUserService.GetCurrentUserId();
                return await _importOrderRepo.DeleteAsync(importOrderID, currentUserId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<clsImportOrder> GetByIdAsync(int importOrderID)
        {
            try
            {
                return await _importOrderRepo.GetByIdAsync(importOrderID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<clsImportOrder>> GetAllAsync()
        {
            try
            {
                return await _importOrderRepo.GetAllAsync();
            }
            catch (Exception)
            {
                return new List<clsImportOrder>();
            }
        }

        public async Task<clsImportOrder> GetBySupplierIdAsync(int supplierID)
        {
            try
            {
                return await _importOrderRepo.GetBySupplierIdAsync(supplierID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // BALDTO Methods
        public async Task<ImportOrderDTO> GetByIdBALDTOAsync(int importOrderID)
        {
            try
            {
                var importOrder = await _importOrderRepo.GetByIdDTOAsync(importOrderID);
                return importOrder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ImportOrderDTO>> GetAllBALDTOAsync()
        {
            try
            {
                var importOrders = await _importOrderRepo.GetAllDTOAsync();
                return importOrders;
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<List<ImportOrderDTO>> GetBySupplierIdBALDTOAsync(int supplierID)
        {
            try
            {
                var importOrders = await _importOrderRepo.GetBySupplierIdDTOAsync(supplierID);
                return importOrders.ToImportOrderDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<bool> AddBALDTOAsync(ImportOrderDTO ImportOrderDTO)
        {
            try
            {
                var importOrder = ImportOrderDTO.ToImportOrderModel();
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                return await AddAsync(importOrder);
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<bool> UpdateBALDTOAsync(ImportOrderDTO ImportOrderDTO)
        {
            try
            {
                if (IsPaymentCompletedDTO(ImportOrderDTO))
                    ImportOrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Completed);
                else
                    ImportOrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.PendingForPayment);

                var importOrder = ImportOrderDTO.ToImportOrderModel();
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                return await UpdateAsync(importOrder);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Enhanced DTO Methods with Business Logic
        public bool IsPaymentCompletedDTO(ImportOrderDTO order)
        {
            return order.PaidAmount == order.TotalAmount;
        }
        public bool IsPaymentCompleted(clsImportOrder order)
        {
            return order.PaidAmount == order.TotalAmount;
        }
        public async Task<ImportOrderDTO> GetByIdWithItemsBALDTOAsync(int importOrderID)
        {
            try
            {
                // Get the import order
                clsImportOrderFilter filter = new clsImportOrderFilter();
                filter.IOID = importOrderID;
                var importOrder = await _importOrderRepo.GetAllDTOAsync(filter);
                var order = importOrder.FirstOrDefault();
                
                if (order != null)
                {
                    // Get the import order items
                    var items = await _importOrderRepo.GetImportOrderItemsByOrderIdDTOAsync(importOrderID);
                    order.ImportOrderItems = items ?? new List<ImportOrderItemDTO>();
                }
                
                return order;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ImportOrderDTO>> GetAllWithItemsBALDTOAsync()
        {
            try
            {
                var importOrders = await _importOrderRepo.GetAllWithItemsAsync();
                return importOrders.ToImportOrderDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<List<ImportOrderDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var importOrders = await _importOrderRepo.GetByDateRangeAsync(startDate, endDate);
                return importOrders.ToImportOrderDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<List<ImportOrderDTO>> GetByPaymentStatusBALDTOAsync(byte paymentStatus)
        {
            try
            {
                var importOrders = await _importOrderRepo.GetByPaymentStatusAsync(paymentStatus);
                return importOrders.ToImportOrderDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<List<ImportOrderDTO>> GetUnpaidOrdersBALDTOAsync()
        {
            return await GetByPaymentStatusBALDTOAsync(0);
        }

        public async Task<List<ImportOrderDTO>> GetPartiallyPaidOrdersBALDTOAsync()
        {
            return await GetByPaymentStatusBALDTOAsync(1);
        }

        public async Task<List<ImportOrderDTO>> GetFullyPaidOrdersBALDTOAsync()
        {
            return await GetByPaymentStatusBALDTOAsync(2);
        }

        // Summary Methods for Performance
        public async Task<List<ImportOrderDTO>> GetAllSummaryBALDTOAsync()
        {
            try
            {
                var importOrders = await _importOrderRepo.GetAllDTOAsync();
                return importOrders.ToImportOrderDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<List<ImportOrderDTO>> GetAllSummaryBALDTOAsync(clsImportOrderFilter filter)
        {
            try
            {
                var importOrders = await _importOrderRepo.GetAllDTOAsync(filter);
                return importOrders.ToImportOrderDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderDTO>();
            }
        }

        public async Task<ImportOrderDTO> GetByIdSummaryBALDTOAsync(int importOrderID)
        {
            try
            {
                var importOrder = await _importOrderRepo.GetByIdAsync(importOrderID);
                return importOrder?.ToImportOrderSummaryBALDTO();
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Business Logic Methods
        public async Task<bool> UpdatePaymentStatusAsync(int importOrderID, byte paymentStatus)
        {
            try
            {
                var importOrder = await _importOrderRepo.GetByIdAsync(importOrderID);
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                if (importOrder == null) return false;

                importOrder.PaymentStatus = paymentStatus;
                return await UpdateAsync(importOrder);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddPaymentAsync(int importOrderID, float paymentAmount)
        {
            try
            {
                var importOrder = await _importOrderRepo.GetByIdAsync(importOrderID);
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                if (importOrder == null) return false;

                importOrder.PaidAmount += paymentAmount;
                
                // Update payment status based on paid amount
                if (IsPaymentCompleted(importOrder))
                    importOrder.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Completed); // Fully paid
                else
                    importOrder.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.PendingForPayment); // Not paid

                return await UpdateAsync(importOrder);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<float> GetRemainingAmountAsync(ImportOrderDTO importOrderID)
        {
            try
            {
                if (importOrder == null) return 0;
                return importOrder.TotalAmount - importOrder.PaidAmount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> IsFullyPaidAsync(int importOrderID)
        {
            try
            {
                var importOrder = await _importOrderRepo.GetByIdAsync(importOrderID);
                if (importOrder == null) return false;
                return importOrder.PaidAmount >= importOrder.TotalAmount;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Save()
        {
            try
            {
                switch (SaveMode)
                {
                    case clsGlobal.enSaveMode.Add:
                        return await AddAsync(importOrder);
                    case clsGlobal.enSaveMode.Update:
                        return await UpdateAsync(importOrder);
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
