using BAL;
using BAL.BALDTO;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface ICustomerService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsCustomer customer { get; set; }
        
        Task<bool> AddAsync(clsCustomer customer);
        Task<bool> UpdateAsync(clsCustomer customer);
        Task<bool> DeleteAsync(int customerID);
        Task<clsCustomer> GetByIdAsync(int customerID);
        Task<List<clsCustomer>> GetAllAsync();
        Task<clsCustomer> GetByPersonIdAsync(int personID);
        
        // BALDTO Methods
        Task<CustomerBALDTO> GetByIdBALDTOAsync(int customerID);
        Task<List<CustomerBALDTO>> GetAllBALDTOAsync();
        Task<bool> AddBALDTOAsync(CustomerBALDTO customerBALDTO);
        Task<bool> UpdateBALDTOAsync(CustomerBALDTO customerBALDTO);
        
        Task<bool> Save();
    }
}
