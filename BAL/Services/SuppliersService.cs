using BAL.Interfaces;
using DAL.IRepo;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly ISuppliersRepo _suppliersRepo;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsSupplier Supplier { get; set; }

        public SuppliersService(ISuppliersRepo suppliersRepo)
        {
            _suppliersRepo = suppliersRepo;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        public async Task<bool> AddAsync(clsSupplier supplier)
        {
            return await _suppliersRepo.AddAsync(supplier);
        }

        public async Task<bool> UpdateAsync(clsSupplier supplier)
        {
            return await _suppliersRepo.UpdateAsync(supplier);
        }

        public async Task<bool> DeleteAsync(int supplierID)
        {
            return await _suppliersRepo.DeleteAsync(supplierID);
        }

        public async Task<clsSupplier> GetByIdAsync(int supplierID)
        {
            return await _suppliersRepo.GetByIdAsync(supplierID);
        }

        public async Task<List<clsSupplier>> GetAllAsync()
        {
            return await _suppliersRepo.GetAllAsync();
        }

        public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await AddAsync(Supplier);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(Supplier);
            }
        }
    }
}
