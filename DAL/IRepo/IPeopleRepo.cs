using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IPeopleRepo
    {
        Task<bool> AddAsync(clsPerson person);
        Task<bool> UpdateAsync(clsPerson person);
        Task<bool> DeleteAsync(int personID);
        Task<clsPerson> GetByIdAsync(int PersonID);
        Task<List<clsPerson>> GetAllAsync();
        Task<clsPerson> GetByPhoneNumberAsync(string phoneNumber);
    }
}
