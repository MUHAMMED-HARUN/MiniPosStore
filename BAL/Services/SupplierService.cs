using BAL.Interfaces;
using BAL.BALDTO;
using BAL.Mappers;
using DAL.IRepo;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepo _supplierRepo;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsSupplier supplier { get; set; }

        public SupplierService(ISupplierRepo supplierRepo)
        {
            _supplierRepo = supplierRepo;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        public async Task<bool> AddAsync(clsSupplier supplier)
        {
            return await _supplierRepo.AddAsync(supplier);
        }

        public async Task<bool> UpdateAsync(clsSupplier supplier)
        {
            return await _supplierRepo.UpdateAsync(supplier);
        }

        public async Task<bool> DeleteAsync(int supplierID)
        {
            return await _supplierRepo.DeleteAsync(supplierID);
        }

        public async Task<clsSupplier> GetByIdAsync(int supplierID)
        {
            return await _supplierRepo.GetByIdAsync(supplierID);
        }

        public async Task<List<clsSupplier>> GetAllAsync()
        {
            return await _supplierRepo.GetAllAsync();
        }

        public async Task<clsSupplier> GetByPersonIdAsync(int personID)
        {
            return await _supplierRepo.GetByPersonIdAsync(personID);
        }

        // BALDTO Methods
        public async Task<SupplierBALDTO> GetByIdBALDTOAsync(int supplierID)
        {
            var supplier = await _supplierRepo.GetByIdAsync(supplierID);
            return supplier?.ToSupplierBALDTO();
        }

        public async Task<List<SupplierBALDTO>> GetAllBALDTOAsync()
        {
            var suppliers = await _supplierRepo.GetAllAsync();
            return suppliers.ToSupplierBALDTOList();
        }

        public async Task<bool> AddBALDTOAsync(SupplierBALDTO supplierBALDTO)
        {
            var supplier = supplierBALDTO.ToSupplierModel();
            return await _supplierRepo.AddAsync(supplier);
        }

        public async Task<bool> UpdateBALDTOAsync(SupplierBALDTO supplierBALDTO)
        {
            var supplier = supplierBALDTO.ToSupplierModel();
            return await _supplierRepo.UpdateAsync(supplier);
        }

        public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await AddAsync(supplier);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(supplier);
            }
        }
    }
}
