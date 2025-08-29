using DAL.EF.Models;
using DAL.IRepo;
using BAL.Interfaces;
using BAL.BALDTO;
using BAL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ImportOrderItemService : IImportOrderItemService
    {
        private readonly IImportOrderItemRepo _importOrderItemRepo;
        private readonly ICurrentUserService _currentUserService;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsImportOrderItem importOrderItem { get; set; }

        public ImportOrderItemService(IImportOrderItemRepo importOrderItemRepo, ICurrentUserService currentUserService)
        {
            _importOrderItemRepo = importOrderItemRepo;
            _currentUserService = currentUserService;
            SaveMode = clsGlobal.enSaveMode.Add;
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
        public async Task<ImportOrderItemBALDTO> GetByIdBALDTOAsync(int importOrderItemID)
        {
            try
            {
                var importOrderItem = await _importOrderItemRepo.GetByIdDTOAsync(importOrderItemID);
                return importOrderItem?.ToImportOrderItemBALDTO();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetAllBALDTOAsync()
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetAllDTOAsync();
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetByImportOrderIdBALDTOAsync(int importOrderID)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByImportOrderIdDTOAsync(importOrderID);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetByProductIdBALDTOAsync(int productID)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByProductIdDTOAsync(productID);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<bool> AddBALDTOAsync(ImportOrderItemBALDTO importOrderItemBALDTO)
        {
            try
            {
                var importOrderItem = importOrderItemBALDTO.ToImportOrderItemModel();
                return await AddAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateBALDTOAsync(ImportOrderItemBALDTO importOrderItemBALDTO)
        {
            try
            {
                var importOrderItem = importOrderItemBALDTO.ToImportOrderItemModel();
                return await UpdateAsync(importOrderItem);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Enhanced DTO Methods with Business Logic
        public async Task<List<ImportOrderItemBALDTO>> GetHighValueItemsBALDTOAsync(float minAmount = 1000)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetHighValueItemsAsync(minAmount);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetByDateRangeBALDTOAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByDateRangeAsync(startDate, endDate);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetByCurrencyTypeBALDTOAsync(string currencyType)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByCurrencyTypeAsync(currencyType);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetByUOMBALDTOAsync(string uomName)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByUOMAsync(uomName);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        // Summary Methods for Performance
        public async Task<List<ImportOrderItemBALDTO>> GetAllSummaryBALDTOAsync()
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetAllSummaryDTOAsync();
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

        public async Task<ImportOrderItemBALDTO> GetByIdSummaryBALDTOAsync(int importOrderItemID)
        {
            try
            {
                var importOrderItem = await _importOrderItemRepo.GetByIdSummaryDTOAsync(importOrderItemID);
                return importOrderItem?.ToImportOrderItemBALDTO();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ImportOrderItemBALDTO>> GetByImportOrderIdSummaryBALDTOAsync(int importOrderID)
        {
            try
            {
                var importOrderItems = await _importOrderItemRepo.GetByImportOrderIdSummaryDTOAsync(importOrderID);
                return importOrderItems.ToImportOrderItemBALDTOList();
            }
            catch (Exception)
            {
                return new List<ImportOrderItemBALDTO>();
            }
        }

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

        public async Task<bool> AddMultipleItemsBALDTOAsync(List<ImportOrderItemBALDTO> importOrderItemBALDTOs)
        {
            try
            {
                var importOrderItems = importOrderItemBALDTOs.Select(dto => dto.ToImportOrderItemModel()).ToList();
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

