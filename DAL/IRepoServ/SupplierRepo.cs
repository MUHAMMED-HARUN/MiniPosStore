using DAL.EF.AppDBContext;
using SharedModels.EF.Models;
using SharedModels.EF.DTO;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DAL.IRepoServ
{
    public class SupplierRepo : ISupplierRepo
    {
        private readonly AppDBContext _context;

        public SupplierRepo(AppDBContext context)
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
            catch(SqlException e)
            {
                return false;
            }
        }

        public async Task<clsSupplier> GetByIdAsync(int supplierId)
        {
            return await _context.Suppliers
                .Include(s => s.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ID == supplierId);
        }

        public async Task<List<clsSupplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .Include(s => s.Person)
                .ToListAsync();
        }

        public async Task<clsSupplier> GetByPersonIdAsync(int personId)
        {
            return await _context.Suppliers
                .Include(s => s.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.PersonID == personId);
        }

        public async Task<SupplierDTO> GetByIdDTOAsync(int supplierID)
        {
            return await _context.Suppliers.AsNoTracking()
                .Where(s => s.ID == supplierID)
                .Select(s => new SupplierDTO
                {
                    SupplierID = s.ID,
                    PersonID = s.PersonID,
                    ShopName = s.StoreName,
                    Address = s.StoreAddress,
                    FirstName = s.Person.FirstName,
                    LastName = s.Person.LastName,
                    PhoneNumber = s.Person.PhoneNumber,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<SupplierDTO>> GetAllDTOAsync()
        {
            return await _context.Suppliers.AsNoTracking()
                .Select(s => new SupplierDTO
                {
                    SupplierID = s.ID,
                    PersonID = s.PersonID,
                    ShopName = s.StoreName,
                    Address = s.StoreAddress,
                    FirstName = s.Person.FirstName,
                    LastName = s.Person.LastName,
                    PhoneNumber = s.Person.PhoneNumber,
                })
                .ToListAsync();
        }
    }
}
