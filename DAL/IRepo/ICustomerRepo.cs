using DAL.EF.Models;
using DAL.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface ICustomerRepo
    {
        Task<bool> AddAsync(clsCustomer customer);
        Task<bool> UpdateAsync(clsCustomer customer);
        Task<bool> DeleteAsync(int customerID);
        Task<clsCustomer> GetByIdAsync(int customerID);
        Task<List<clsCustomer>> GetAllAsync();
        Task<clsCustomer> GetByPersonIdAsync(int personID);
        
        // DTO Methods
        Task<CustomerDTO> GetByIdDTOAsync(int customerID);
        Task<List<CustomerDTO>> GetAllDTOAsync();
    }
}
