using DAL.EF.AppDBContext;
using DAL.EF.DTO;
using DAL.EF.Models;
using DAL.IRepo;
using DAL.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class ImportOrderItemRepo : IImportOrderItemRepo
    {
        private readonly AppDBContext _context;

        public ImportOrderItemRepo(AppDBContext context)
        {
            _context = context;
        }

        // Basic CRUD Operations
        public async Task<bool> AddAsync(clsImportOrderItem importOrderItem)
        {
            try
            {
                await _context.ImportOrderItems.AddAsync(importOrderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsImportOrderItem importOrderItem)
        {
            try
            {
                _context.ImportOrderItems.Update(importOrderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int importOrderItemID)
        {
            try
            {
                var importOrderItem = await _context.ImportOrderItems.FindAsync(importOrderItemID);
                if (importOrderItem != null)
                {
                    _context.ImportOrderItems.Remove(importOrderItem);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsImportOrderItem> GetByIdAsync(int importOrderItemID)
        {
            try
            {
                return await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ioi => ioi.ImportOrderID == importOrderItemID);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<clsImportOrderItem>> GetAllAsync()
        {
            try
            {
                return await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch
            {
                return new List<clsImportOrderItem>();
            }
        }

        public async Task<List<clsImportOrderItem>> GetByImportOrderIdAsync(int importOrderID)
        {
            try
            {
                return await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => ioi.ImportOrderID == importOrderID)
                    .ToListAsync();
            }
            catch
            {
                return new List<clsImportOrderItem>();
            }
        }

        public async Task<List<clsImportOrderItem>> GetByProductIdAsync(int productID)
        {
            try
            {
                return await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => ioi.ProductID == productID)
                    .ToListAsync();
            }
            catch
            {
                return new List<clsImportOrderItem>();
            }
        }

        // DTO Methods
        public async Task<ImportOrderItemDTO> GetByIdDTOAsync(int importOrderItemID)
        {
            try
            {
                var importOrderItem = await GetByIdAsync(importOrderItemID);
                return importOrderItem?.ToImportOrderItemDTO();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetAllDTOAsync()
        {
            try
            {
                var importOrderItems = await GetAllAsync();
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByImportOrderIdDTOAsync(int importOrderID)
        {
            try
            {
                var importOrderItems = await GetByImportOrderIdAsync(importOrderID);
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByProductIdDTOAsync(int productID)
        {
            try
            {
                var importOrderItems = await GetByProductIdAsync(productID);
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var importOrderItems = await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => ioi.ImportOrder.ImportDate >= startDate && ioi.ImportOrder.ImportDate <= endDate)
                    .ToListAsync();
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByCurrencyTypeAsync(string currencyType)
        {
            try
            {
                var importOrderItems = await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => ioi.Product.CurrencyType == currencyType)
                    .ToListAsync();
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByUOMAsync(string uomName)
        {
            try
            {
                var importOrderItems = await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => ioi.Product.UnitOfMeasure.Name == uomName)
                    .ToListAsync();
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetHighValueItemsAsync(float minAmount = 1000)
        {
            try
            {
                var importOrderItems = await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => (ioi.Quantity * ioi.SellingPrice) >= minAmount)
                    .ToListAsync();
                return importOrderItems.ToImportOrderItemDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        // Summary DTO Methods
        public async Task<ImportOrderItemDTO> GetByIdSummaryDTOAsync(int importOrderItemID)
        {
            try
            {
                var importOrderItem = await GetByIdAsync(importOrderItemID);
                return importOrderItem?.ToImportOrderItemSummaryDTO();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetAllSummaryDTOAsync()
        {
            try
            {
                var importOrderItems = await GetAllAsync();
                return importOrderItems.ToImportOrderItemSummaryDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }

        public async Task<List<ImportOrderItemDTO>> GetByImportOrderIdSummaryDTOAsync(int importOrderID)
        {
            try
            {
                var importOrderItems = await GetByImportOrderIdAsync(importOrderID);
                return importOrderItems.ToImportOrderItemSummaryDTOList();
            }
            catch
            {
                return new List<ImportOrderItemDTO>();
            }
        }
    }
}

