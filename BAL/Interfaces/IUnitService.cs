using BAL;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.EF.Models;
namespace BAL.Interfaces
{
    public interface IUnitService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsUnitOfMeasure UOM { get; set; }
        
        Task<bool> AddAsync(clsUnitOfMeasure unit);
        Task<bool> UpdateAsync(clsUnitOfMeasure unit);
        Task<bool> DeleteAsync(int unitID);
        Task<clsUnitOfMeasure> GetByIdAsync(int unitID);
        Task<List<clsUnitOfMeasure>> GetAllAsync();
        
        Task<bool> Save();
    }
}
