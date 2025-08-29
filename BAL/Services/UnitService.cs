using BAL.Interfaces;
using DAL.IRepo;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepo _unitRepo;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsUnitOfMeasure UOM { get; set; }

        public UnitService(IUnitRepo unitRepo)
        {
            _unitRepo = unitRepo;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        public async Task<bool> AddAsync(clsUnitOfMeasure unit)
        {
            return await _unitRepo.AddAsync(unit);
        }

        public async Task<bool> UpdateAsync(clsUnitOfMeasure unit)
        {
            return await _unitRepo.UpdateAsync(unit);
        }

        public async Task<bool> DeleteAsync(int unitID)
        {
            return await _unitRepo.DeleteAsync(unitID);
        }

        public async Task<clsUnitOfMeasure> GetByIdAsync(int unitID)
        {
            return await _unitRepo.GetByIdAsync(unitID);
        }

        public async Task<List<clsUnitOfMeasure>> GetAllAsync()
        {
            return await _unitRepo.GetAllAsync();
        }

        public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await AddAsync(UOM);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(UOM);
            }
        }
    }
}
