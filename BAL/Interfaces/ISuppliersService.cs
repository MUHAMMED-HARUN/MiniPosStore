using BAL;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface ISuppliersService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsSupplier Supplier { get; set; }
        
        Task<bool> AddAsync(clsSupplier supplier);
        Task<bool> UpdateAsync(clsSupplier supplier);
        Task<bool> DeleteAsync(int supplierID);
        Task<clsSupplier> GetByIdAsync(int supplierID);
        Task<List<clsSupplier>> GetAllAsync();
        
        Task<bool> Save();
    }
}
