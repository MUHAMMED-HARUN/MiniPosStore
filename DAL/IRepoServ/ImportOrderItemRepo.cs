using DAL.EF.AppDBContext;
using SharedModels.EF.DTO;
using SharedModels.EF.Models;
using DAL.IRepo;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Mapper;

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



        public async Task<List<ImportOrderItemDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var importOrderItems = await _context.ImportOrderItems
                    .Include(ioi => ioi.Product)
                    .Include(ioi => ioi.ImportOrder)
                    .AsNoTracking()
                    .Where(ioi => ioi.ImportOrder.ImportDate >= startDate && ioi.ImportOrder.ImportDate <= endDate)
                    .Select(ioi=> ioi.ToImportOrderItemDTO())
                    .ToListAsync();
                return importOrderItems;
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


    }
}

