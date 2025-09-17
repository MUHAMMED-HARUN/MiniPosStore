using DAL.EF.AppDBContext;
using SharedModels.EF.Models;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class UnitRepo : IUnitRepo
    {
        private readonly AppDBContext _context;

        public UnitRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(clsUnitOfMeasure unit)
        {
            try
            {
                await _context.UnitOfMeasures.AddAsync(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsUnitOfMeasure unit)
        {
            try
            {
                _context.UnitOfMeasures.Update(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int unitId)
        {
            try
            {
                var unit = await _context.UnitOfMeasures.FindAsync(unitId);
                if (unit == null)
                    return false;
                _context.UnitOfMeasures.Remove(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsUnitOfMeasure> GetByIdAsync(int unitId)
        {
            return await _context.UnitOfMeasures.FirstOrDefaultAsync(u => u.ID == unitId);
        }

        public async Task<List<clsUnitOfMeasure>> GetAllAsync()
        {
            return await _context.UnitOfMeasures.ToListAsync();
        }
    }
}