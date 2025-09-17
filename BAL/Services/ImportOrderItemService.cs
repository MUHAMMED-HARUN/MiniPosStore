using SharedModels.EF.Models;
using DAL.IRepo;
using BAL.Interfaces;

using BAL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels.EF.DTO;

namespace BAL.Services
{
    public class ImportOrderItemService : IImportOrderItemService
    {
        private readonly IImportOrderItemRepo _importOrderItemRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImportOrderRepo _importOrderRepo;


        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsImportOrderItem importOrderItem { get; set; }

        public ImportOrderItemService(IImportOrderItemRepo importOrderItemRepo, ICurrentUserService currentUserService,IImportOrderRepo importOrder)
        {
            _importOrderItemRepo = importOrderItemRepo;
            _currentUserService = currentUserService;
            SaveMode = clsGlobal.enSaveMode.Add;
            _importOrderRepo = importOrder; 
        }

        // Basic CRUD Operations
        public async Task<bool> AddAsync(clsImportOrderItem importOrderItem)
        {
            try
            {
                return await _importOrderItemRepo.AddAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsImportOrderItem importOrderItem)
        {
            try
            {
                return await _importOrderItemRepo.UpdateAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int importOrderItemID)
        {
            try
            {
                return await _importOrderItemRepo.DeleteAsync(importOrderItemID);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<clsImportOrderItem> GetByIdAsync(int importOrderItemID)
        {
            try
            {
                return await _importOrderItemRepo.GetByIdAsync(importOrderItemID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<clsImportOrderItem>> GetAllAsync()
        {
            try
            {
                return await _importOrderItemRepo.GetAllAsync();
            }
            catch (Exception)
            {
                return new List<clsImportOrderItem>();
            }
        }

        public async Task<List<clsImportOrderItem>> GetByImportOrderIdAsync(int importOrderID)
        {
            try
            {
                return await _importOrderItemRepo.GetByImportOrderIdAsync(importOrderID);
            }
            catch (Exception)
            {
                return new List<clsImportOrderItem>();
            }
        }

        public async Task<List<clsImportOrderItem>> GetByProductIdAsync(int productID)
        {
            try
            {
                return await _importOrderItemRepo.GetByProductIdAsync(productID);
            }
            catch (Exception)
            {
                return new List<clsImportOrderItem>();
            }
        }

        // BALDTO Methods
        public async Task<ImportOrderItemDTO> GetByIdBALDTOAsync(int importOrderItemID)
        {
            try
            {
                
                var importOrderItem = await _importOrderRepo.GetImportOrderItemByIdDTOAsync(importOrderItemID); ;
                return importOrderItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetAllBALDTOAsync()
        {
            try
            {
                //var importOrderItems = await _importOrderItemRepo.GetAllAsync();
                return null;
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByImportOrderIdBALDTOAsync(int importOrderID)
        {
            try
            {
                var importOrderItems = await _importOrderRepo.GetImportOrderItemsByOrderIdDTOAsync(importOrderID); ;
                return importOrderItems;
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByProductIdBALDTOAsync(int productID)
        {
            try
            {
                
                var importOrderItems = await _importOrderItemRepo.GetByProductIdAsync(productID);
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<bool> AddBALDTOAsync(ImportOrderItemDTO ImportOrderItemDTO)
        {
            try
            {
                var importOrderItem = ImportOrderItemDTO.ToImportOrderItemModel();
                return await AddAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateBALDTOAsync(ImportOrderItemDTO ImportOrderItemDTO)
        {
            try
            {
                var importOrderItem = ImportOrderItemDTO.ToImportOrderItemModel();
                return await UpdateAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateBALDTOAsync(clsImportOrderItem ImportOrderItem)
        {
            try
            {
              
                return await UpdateAsync(ImportOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Enhanced DTO Methods with Business Logic
        public async Task<List<ImportOrderItemDTO>> GetHighValueItemsBALDTOAsync(float minAmount = 1000)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetHighValueItemsAsync(minAmount);
                return importOrderItems;
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByDateRangeAsync(startDate, endDate);
                return importOrderItems;
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByCurrencyTypeBALDTOAsync(string currencyType)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByCurrencyTypeAsync(currencyType);
                return importOrderItems;
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByUOMBALDTOAsync(string uomName)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByUOMAsync(uomName);
                return importOrderItems;
            }
            catch (Exception)
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        // Summary Methods for Performance
  

    

        

        // Business Logic Methods
        public async Task<float> GetTotalAmountByImportOrderAsync(int importOrderID)
        {
            try
            {
                var items = await GetByImportOrderIdBALDTOAsync(importOrderID);
                return items.Sum(item => item.TotalItemAmount);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<float> GetTotalAmountByProductAsync(int productID)
        {
            try
            {
                var items = await GetByProductIdBALDTOAsync(productID);
                return items.Sum(item => item.TotalItemAmount);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> GetItemsCountByImportOrderAsync(int importOrderID)
        {
            try
            {
                var items = await GetByImportOrderIdBALDTOAsync(importOrderID);
                return items.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> HasProductInImportOrderAsync(int importOrderID, int productID)
        {
            try
            {
                var items = await GetByImportOrderIdBALDTOAsync(importOrderID);
                return items.Any(item => item.ProductID == productID);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateQuantityAsync(int importOrderItemID, float newQuantity)
        {
            try
            {
                var importOrderItem = await _importOrderItemRepo.GetByIdAsync(importOrderItemID);
                if (importOrderItem == null) return false;

                importOrderItem.Quantity = newQuantity;
                return await UpdateAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSellingPriceAsync(int importOrderItemID, float newSellingPrice)
        {
            try
            {
                var importOrderItem = await _importOrderItemRepo.GetByIdAsync(importOrderItemID);
                if (importOrderItem == null) return false;

                importOrderItem.SellingPrice = newSellingPrice;
                return await UpdateAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Bulk Operations
        public async Task<bool> AddMultipleItemsAsync(List<clsImportOrderItem> importOrderItems)
        {
            try
            {
                bool allSuccess = true;
                foreach (var item in importOrderItems)
                {
                    if (!await AddAsync(item))
                        allSuccess = false;
                }
                return allSuccess;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddMultipleItemsBALDTOAsync(List<ImportOrderItemDTO> ImportOrderItemDTOs)
        {
            try
            {
                var importOrderItems = ImportOrderItemDTOs.Select(dto => dto.ToImportOrderItemModel()).ToList();
                return await AddMultipleItemsAsync(importOrderItems);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteMultipleItemsAsync(int[] importOrderItemIDs)
        {
            try
            {
                bool allSuccess = true;
                foreach (var id in importOrderItemIDs)
                {
                    if (!await DeleteAsync(id))
                        allSuccess = false;
                }
                return allSuccess;
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
                        return await AddAsync(importOrderItem);
                    case clsGlobal.enSaveMode.Update:
                        return await UpdateAsync(importOrderItem);
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

