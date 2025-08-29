using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IUserRepo
    {
        Task<bool> AddAsync(clsUser user);
        Task<bool> UpdateAsync(clsUser user);
        Task<bool> DeleteAsync(string UserID);
        Task<clsUser> GetByIdAsync(string UserID);
        Task<List<clsUser>> GetAllAsync();
        bool AssignPermissionsAsync(string UserID);

    }
}
