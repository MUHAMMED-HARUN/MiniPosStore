using BAL;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IUserService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsUser User { get; set; }
        
        Task<bool> AddAsync(clsUser user);
        Task<bool> UpdateAsync(clsUser user);
        Task<bool> DeleteAsync(string userID);
        Task<clsUser> GetByIdAsync(string userID);
        Task<List<clsUser>> GetAllAsync();
        
        Task<bool> Save();
    }
}
