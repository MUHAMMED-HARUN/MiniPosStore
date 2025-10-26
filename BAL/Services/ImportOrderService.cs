using SharedModels.EF.Models;
using DAL.IRepo;
using BAL.Interfaces;
using BAL.Events.ImportOrderEvents;
using BAL.Events;
using BAL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels.EF.Filters;
using SharedModels.EF.DTO;
using Microsoft.Data.SqlClient;

namespace BAL.Services
{
    public class ImportOrderService : IImportOrderService
    {
        private readonly IImportOrderRepo _importOrderRepo;
        private readonly ICurrentUserService _currentUserService;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsImportOrder importOrder { get; set; }
        
        // Events
        public event AsyncEventHandler<ImportOrderConfirmedEventArgs> ImportOrderConfirmedEvent;
        public event AsyncEventHandler<ImportOrderItemUnionAddedEventArgs> ImportOrderItemUnionAddedEvent;
        public event AsyncEventHandler<ImportOrderItemUnionUpdatedEventArgs> ImportOrderItemUnionUpdatedEvent;
        public event AsyncEventHandler<ImportOrderItemUnionDeletedEventArgs> ImportOrderItemUnionDeletedEvent;

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
            catch (SqlException e)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"ImportOrderService.AddAsync Error: {e.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {e.StackTrace}");
                
                // Get inner exception details
                string errorMessage = e.Message;
                if (e.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {e.InnerException.Message}";
                }
                
                // Re-throw the exception to see it in the application
                throw new Exception($"فشل في إضافة أمر الاستيراد: {errorMessage}", e);
            }
            catch (Exception e)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"ImportOrderService.AddAsync Error: {e.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {e.StackTrace}");
                
                // Get inner exception details
                string errorMessage = e.Message;
                if (e.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {e.InnerException.Message}";
                }
                
                // Re-throw the exception to see it in the application
                throw new Exception($"فشل في إضافة أمر الاستيراد: {errorMessage}", e);
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
            catch (Exception e)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"ImportOrderService.AddBALDTOAsync Error: {e.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {e.StackTrace}");
                
                // Get inner exception details
                string errorMessage = e.Message;
                if (e.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {e.InnerException.Message}";
                }
                
                // Re-throw the exception to see it in the application
                throw new Exception($"فشل في إضافة أمر الاستيراد: {errorMessage}", e);
            }
        }
        
        public async Task<bool> UpdateBALDTOAsync(ImportOrderDTO ImportOrderDTO)
        {
            try
            {
                var oldImportOrder = await _importOrderRepo.GetByIdAsync(ImportOrderDTO.ImportOrderID);
                byte oldStatus = oldImportOrder?.PaymentStatus ?? 0;

                if (IsPaymentCompletedDTO(ImportOrderDTO))
                    ImportOrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Completed);
                else
                    ImportOrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.PendingForPayment);

                var importOrder = ImportOrderDTO.ToImportOrderModel();
                importOrder.ActionByUser = _currentUserService.GetCurrentUserId();
                bool result = await UpdateAsync(importOrder);

                // Trigger ImportOrderConfirmed event if payment status changed from pending to completed
                if (result && oldStatus == ((byte)clsGlobal.enPaymentStatus.Pending) &&( 
                    ImportOrderDTO.PaymentStatus == (byte)clsGlobal.enPaymentStatus.Completed||ImportOrderDTO.PaymentStatus ==((byte)clsGlobal.enPaymentStatus.PendingForPayment)))
                {
                    await OnImportOrderConfirmed(importOrder);
                }

                return result;
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
                clsImportOrderFilter filter = new clsImportOrderFilter();
                filter.IOID = importOrderID;
                var importOrders = await _importOrderRepo.GetAllDTOAsync(filter);
                var importOrder = importOrders.FirstOrDefault();
                if (importOrder == null)
                    return null;

                importOrder.ImportOrderItems = await _importOrderRepo.GetImportOrderItemsByOrderIdDTOAsync(importOrder.ImportOrderID);
                importOrder.ItemsCount = importOrder.ImportOrderItems?.Count ?? 0;
                return importOrder;

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

        // Import Order Item Union Methods
        public async Task<List<ImportOrderItemUnionDTO>> GetImportOrderItemUnionDTOs(clsImportOrderItemUnionFilter filter)
        {
            return await _importOrderRepo.GetImportOrderItemUnionDTOs(filter);
        }

        // Event Raisers
               public async Task OnImportOrderConfirmed(clsImportOrder importOrder)
               {
                   // Get union items for this import order
                   var unionFilter = new clsImportOrderItemUnionFilter
                   {
                       ImportOrderID = importOrder.ID
                   };
                   var unionItems = await _importOrderRepo.GetImportOrderItemUnionDTOs(unionFilter);
                   
                   if (ImportOrderConfirmedEvent != null)
                       await ImportOrderConfirmedEvent.Invoke(this, new ImportOrderConfirmedEventArgs(importOrder, unionItems));
               }

        public async Task OnImportOrderItemUnionAdded(ImportOrderItemUnionDTO importOrderItem)
        {
            if (ImportOrderItemUnionAddedEvent != null)
                await ImportOrderItemUnionAddedEvent.Invoke(this, new ImportOrderItemUnionAddedEventArgs(importOrderItem));
        }

        public async Task OnImportOrderItemUnionUpdated(ImportOrderItemUnionDTO oldImportOrderItem, ImportOrderItemUnionDTO newImportOrderItem)
        {
            if (ImportOrderItemUnionUpdatedEvent != null)
                await ImportOrderItemUnionUpdatedEvent.Invoke(this, new ImportOrderItemUnionUpdatedEventArgs(oldImportOrderItem, newImportOrderItem));
        }

        public async Task OnImportOrderItemUnionDeleted(ImportOrderItemUnionDTO importOrderItem)
        {
            if (ImportOrderItemUnionDeletedEvent != null)
                await ImportOrderItemUnionDeletedEvent.Invoke(this, new ImportOrderItemUnionDeletedEventArgs(importOrderItem));
        }

               // Import Order Item Management
               public async Task<bool> AddItem(clsImportOrderItem importOrderItem)
               {
                   try
                   {
                       bool result = await _importOrderRepo.AddItem(importOrderItem);
                       if (result)
                       {
                           // Get the item with updated ID from database
                           var addedItem = await _importOrderRepo.GetImportOrderItemByIdDTOAsync(importOrderItem.ID);
                           if (addedItem != null)
                           {
                               // Convert to Union DTO and fire event
                               var unionItem = new ImportOrderItemUnionDTO
                               {
                                   ImportOrderItemID = addedItem.ImportOrderItemID,
                                   ImportOrderID = addedItem.ImportOrderID,
                                   ItemID = addedItem.ProductID,
                                   ItemType = 1, // Product
                                   Quantity = addedItem.Quantity,
                                   SellingPrice = addedItem.SellingPrice
                               };
                               await OnImportOrderItemUnionAdded(unionItem);
                           }
                       }
                       return result;
                   }
                   catch (Exception)
                   {
                       return false;
                   }
               }

               public async Task<bool> AddRawMaterialItem(clsImportRawMaterialItem rawMaterialItem)
               {
                   try
                   {
                       bool result = await _importOrderRepo.AddRawMaterialItem(rawMaterialItem);
                       if (result)
                       {
                           // Get the item with updated ID from database
                           var addedItem = await _importOrderRepo.GetRawMaterialItemByIdAsync(rawMaterialItem.ID);
                           if (addedItem != null)
                           {
                               // Convert to Union DTO and fire event
                               var unionItem = new ImportOrderItemUnionDTO
                               {
                                   ImportOrderItemID = addedItem.ID,
                                   ImportOrderID = addedItem.ImportOrderID,
                                   ItemID = addedItem.RawMaterialID,
                                   ItemType = 2, // Raw Material
                                   Quantity =(float) addedItem.Quantity,
                                   SellingPrice = (float)addedItem.SellingPrice
                               };
                               await OnImportOrderItemUnionAdded(unionItem);
                           }
                       }
                       return result;
                   }
                   catch (Exception)
                   {
                       return false;
                   }
               }

               public async Task<bool> UpdateRawMaterialItem(clsImportRawMaterialItem rawMaterialItem)
               {
                   try
                   {
                       // Get old item for comparison
                       var oldItem = await _importOrderRepo.GetRawMaterialItemByIdAsync(rawMaterialItem.ID);
                       bool result = await _importOrderRepo.UpdateRawMaterialItem(rawMaterialItem);
                       if (result && oldItem != null)
                       {
                           // Convert to Union DTOs and fire event
                           var oldUnionItem = new ImportOrderItemUnionDTO
                           {
                               ImportOrderItemID = oldItem.ID,
                               ImportOrderID = oldItem.ImportOrderID,
                               ItemID = oldItem.RawMaterialID,
                               ItemType = 2, // Raw Material
                               Quantity = (float)oldItem.Quantity,
                               SellingPrice = (float)oldItem.SellingPrice
                           };
                           
                           var newUnionItem = new ImportOrderItemUnionDTO
                           {
                               ImportOrderItemID = rawMaterialItem.ID,
                               ImportOrderID = rawMaterialItem.ImportOrderID,
                               ItemID = rawMaterialItem.RawMaterialID,
                               ItemType = 2, // Raw Material
                               Quantity = (float)rawMaterialItem.Quantity,
                               SellingPrice = (float)rawMaterialItem.SellingPrice
                           };
                           
                           await OnImportOrderItemUnionUpdated(oldUnionItem, newUnionItem);
                       }
                       return result;
                   }
                   catch (Exception)
                   {
                       return false;
                   }
               }

        public async Task<bool> UpdateItem(clsImportOrderItem importOrderItem)
        {
            try
            {
                var oldItem = await _importOrderRepo.GetImportOrderItemByIdDTOAsync(importOrderItem.ID);
                bool result = await _importOrderRepo.UpdateItem(importOrderItem);
                if (result && oldItem != null)
                {
                    // Convert to Union DTOs and fire event
                    var oldUnionItem = new ImportOrderItemUnionDTO
                    {
                        ImportOrderItemID = oldItem.ImportOrderItemID,
                        ImportOrderID = oldItem.ImportOrderID,
                        ItemID = oldItem.ProductID,
                        ItemType = 1, // Product
                        Quantity = oldItem.Quantity,
                        SellingPrice = oldItem.SellingPrice
                    };
                    
                    var newUnionItem = new ImportOrderItemUnionDTO
                    {
                        ImportOrderItemID = importOrderItem.ID,
                        ImportOrderID = importOrderItem.ImportOrderID,
                        ItemID = importOrderItem.ProductID,
                        ItemType = 1, // Product
                        Quantity = importOrderItem.Quantity,
                        SellingPrice = importOrderItem.SellingPrice
                    };
                    
                    await OnImportOrderItemUnionUpdated(oldUnionItem, newUnionItem);
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(int importOrderItemID)
        {
            try
            {
                var oldItem = await _importOrderRepo.GetImportOrderItemByIdDTOAsync(importOrderItemID);
                bool result = await _importOrderRepo.DeleteItem(importOrderItemID);
                if (result && oldItem != null)
                {
                    // Convert to Union DTO and fire event
                    var unionItem = new ImportOrderItemUnionDTO
                    {
                        ImportOrderItemID = oldItem.ImportOrderItemID,
                        ImportOrderID = oldItem.ImportOrderID,
                        ItemID = oldItem.ProductID,
                        ItemType = 1, // Product
                        Quantity = oldItem.Quantity,
                        SellingPrice = oldItem.SellingPrice
                    };
                    await OnImportOrderItemUnionDeleted(unionItem);
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRawMaterialItem(int rawMaterialItemID)
        {
            try
            {
                var oldItem = await _importOrderRepo.GetRawMaterialItemByIdAsync(rawMaterialItemID);
                bool result = await _importOrderRepo.DeleteRawMaterialItem(rawMaterialItemID);
                if (result && oldItem != null)
                {
                    // Convert to Union DTO and fire event
                    var unionItem = new ImportOrderItemUnionDTO
                    {
                        ImportOrderItemID = oldItem.ID,
                        ImportOrderID = oldItem.ImportOrderID,
                        ItemID = oldItem.RawMaterialID,
                        ItemType = 2, // Raw Material
                        Quantity = (float)oldItem.Quantity,
                        SellingPrice = (float)oldItem.SellingPrice
                    };
                    await OnImportOrderItemUnionDeleted(unionItem);
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

               public async Task<List<clsImportOrderItem>> GetItemsByImportOrderID(int importOrderID)
               {
                   try
                   {
                       return await _importOrderRepo.GetItemsByOrderID(importOrderID);
                   }
                   catch (Exception)
                   {
                       return new List<clsImportOrderItem>();
                   }
               }

               public async Task<List<clsImportRawMaterialItem>> GetRawMaterialItemsByImportOrderID(int importOrderID)
               {
                   try
                   {
                       return await _importOrderRepo.GetRawMaterialItemsByOrderID(importOrderID);
                   }
                   catch (Exception)
                   {
                       return new List<clsImportRawMaterialItem>();
                   }
               }

               public async Task<clsImportRawMaterialItem> GetRawMaterialItemByIdAsync(int id)
               {
                   try
                   {
                       return await _importOrderRepo.GetRawMaterialItemByIdAsync(id);
                   }
                   catch (Exception)
                   {
                       return null;
                   }
               }
    }
}
