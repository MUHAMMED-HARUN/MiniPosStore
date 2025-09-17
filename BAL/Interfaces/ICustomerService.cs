using BAL;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
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
        Task<CustomerDTO> GetByIdBALDTOAsync(int customerID);
        Task<List<CustomerDTO>> GetAllBALDTOAsync();
        Task<List<CustomerDTO>> GetAllBALDTOAsync(clsCustomerFilter filter);
        Task<bool> AddBALDTOAsync(CustomerDTO CustomerDTO);
        Task<bool> UpdateBALDTOAsync(CustomerDTO CustomerDTO);
        
        Task<bool> Save();
    }
}
