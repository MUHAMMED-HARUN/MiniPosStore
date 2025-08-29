using BAL;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IPeopleService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsPerson People { get; set; }
        
        Task<bool> AddAsync(clsPerson person);
        Task<bool> UpdateAsync(clsPerson person);
        Task<bool> DeleteAsync(int personID);
        Task<clsPerson> GetByIdAsync(int personID);
        Task<List<clsPerson>> GetAllAsync();
        Task<clsPerson> GetByPhoneNumberAsync(string phoneNumber);
        
        Task<bool> Save();
    }
}
