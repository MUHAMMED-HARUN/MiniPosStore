using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IUnitRepo
    {
        Task<bool> AddAsync(clsUnitOfMeasure Unit);
        Task<bool> UpdateAsync(clsUnitOfMeasure Unit);
        Task<bool> DeleteAsync(int UnitID);
        Task<clsUnitOfMeasure> GetByIdAsync(int UnitID);
        Task<List<clsUnitOfMeasure>> GetAllAsync();

    }
}
