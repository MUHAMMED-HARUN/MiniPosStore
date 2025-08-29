using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface ISuppliersRepo
    {
        Task<bool> AddAsync(clsSupplier supplier);
        Task<bool> UpdateAsync(clsSupplier supplier);
        Task<bool> DeleteAsync(int SupplierID);
        Task<clsSupplier> GetByIdAsync(int SupplierID);
        Task<List<clsSupplier>> GetAllAsync();

    }
}
