using DAL.EF.AppDBContext;
using DAL.EF.Models;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class PeopleRepo : IPeopleRepo
    {
        private readonly AppDBContext _context;

        public PeopleRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(clsPerson person)
        {
            try
            {
                await _context.People.AddAsync(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsPerson person)
        {
            try
            {
                _context.People.Update(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int personId)
        {
            try
            {
                var person = await _context.People.FindAsync(personId);
                if (person == null)
                    return false;
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsPerson> GetByIdAsync(int personId)
        {
            return await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.ID == personId);
        }

        public async Task<List<clsPerson>> GetAllAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<clsPerson> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.People.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }
    }
}