using DAL.EF.AppDBContext;
using DAL.EF.Models;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class SuppliersRepo : ISuppliersRepo
    {
        private readonly AppDBContext _context;

        public SuppliersRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(clsSupplier supplier)
        {
            try
            {
                await _context.Suppliers.AddAsync(supplier);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsSupplier supplier)
        {
            try
            {
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int supplierId)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(supplierId);
                if (supplier == null)
                    return false;
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsSupplier> GetByIdAsync(int supplierId)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierId);
        }

        public async Task<List<clsSupplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}