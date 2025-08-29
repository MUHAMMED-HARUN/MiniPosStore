using BAL;
using BAL.BALDTO;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface ISupplierService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsSupplier supplier { get; set; }
        
        Task<bool> AddAsync(clsSupplier supplier);
        Task<bool> UpdateAsync(clsSupplier supplier);
        Task<bool> DeleteAsync(int supplierID);
        Task<clsSupplier> GetByIdAsync(int supplierID);
        Task<List<clsSupplier>> GetAllAsync();
        Task<clsSupplier> GetByPersonIdAsync(int personID);
        
        // BALDTO Methods
        Task<SupplierBALDTO> GetByIdBALDTOAsync(int supplierID);
        Task<List<SupplierBALDTO>> GetAllBALDTOAsync();
        Task<bool> AddBALDTOAsync(SupplierBALDTO supplierBALDTO);
        Task<bool> UpdateBALDTOAsync(SupplierBALDTO supplierBALDTO);
        
        Task<bool> Save();
    }
}
