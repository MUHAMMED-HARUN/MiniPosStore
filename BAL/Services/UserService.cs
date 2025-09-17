using BAL.Interfaces;
using DAL.IRepo;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsUser User { get; set; }

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        public async Task<bool> AddAsync(clsUser user)
        {
            return await _userRepo.AddAsync(user);
        }

        public async Task<bool> UpdateAsync(clsUser user)
        {
            return await _userRepo.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(string userID)
        {
            return await _userRepo.DeleteAsync(userID);
        }

        public async Task<clsUser> GetByIdAsync(string userID)
        {
            return await _userRepo.GetByIdAsync(userID);
        }

        public async Task<List<clsUser>> GetAllAsync()
        {
            return await _userRepo.GetAllAsync();
        }

        public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await AddAsync(User);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(User);
            }
        }
    }
}
