using DAL.EF.Models;
using DAL.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface ISupplierRepo
    {
        Task<bool> AddAsync(clsSupplier supplier);
        Task<bool> UpdateAsync(clsSupplier supplier);
        Task<bool> DeleteAsync(int supplierID);
        Task<clsSupplier> GetByIdAsync(int supplierID);
        Task<List<clsSupplier>> GetAllAsync();
        Task<clsSupplier> GetByPersonIdAsync(int personID);
        
        // DTO Methods
        Task<SupplierDTO> GetByIdDTOAsync(int supplierID);
        Task<List<SupplierDTO>> GetAllDTOAsync();
    }
}
