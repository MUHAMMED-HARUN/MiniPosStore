using DAL.EF.AppDBContext;
using DAL.EF.Models;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class UserRepo : IUserRepo
    {
       AppDBContext _context;
        public UserRepo(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<bool> AddAsync(clsUser user)
        {

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AssignPermissionsAsync(string UserID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string UserID)
        {
            try
            {
                clsUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UserID);
                if(user == null)
                {
                    return false; // User not found
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<clsUser>> GetAllAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<clsUser> GetByIdAsync(string UserID)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Id==UserID);
        }

        public async Task<bool> UpdateAsync(clsUser user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
