using BAL;
using SharedModels.EF.DTO;
using SharedModels.EF.Models;
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

     
        Task<bool> Save();
    }
}
