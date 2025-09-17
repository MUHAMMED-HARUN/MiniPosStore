
using SharedModels.EF.Filters;
using BAL.Interfaces;
using BAL.Mappers;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using DAL.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ICurrentUserRepo _currentUserRepo;
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsCustomer customer { get; set; }

        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        public async Task<bool> AddAsync(clsCustomer customer)
        {
            
            return await _customerRepo.AddAsync(customer);
        }

        public async Task<bool> UpdateAsync(clsCustomer customer)
        {
            return await _customerRepo.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(int customerID)
        {
            return await _customerRepo.DeleteAsync(customerID);
        }

        public async Task<clsCustomer> GetByIdAsync(int customerID)
        {
            return await _customerRepo.GetByIdAsync(customerID);
        }

        public async Task<List<clsCustomer>> GetAllAsync()
        {
            return await _customerRepo.GetAllAsync();
        }

        public async Task<clsCustomer> GetByPersonIdAsync(int personID)
        {
            return await _customerRepo.GetByPersonIdAsync(personID);
        }

        // BALDTO Methods
        public async Task<CustomerDTO> GetByIdBALDTOAsync(int customerID)
        {
            var customer = await _customerRepo.GetByIdAsync(customerID);
            return customer?.ToCustomerDTO();
        }

        public async Task<List<CustomerDTO>> GetAllBALDTOAsync()
        {
            clsCustomerFilter filter = new clsCustomerFilter();

            var customers = await _customerRepo.GetAllDTOAsync(filter);
            return customers;
        }
      public  async Task<List<CustomerDTO>> GetAllBALDTOAsync(clsCustomerFilter filter)
        {
                var Customer =await _customerRepo.GetAllDTOAsync(filter);
            return Customer;
   
        }
        public async Task<bool> AddBALDTOAsync(CustomerDTO CustomerDTO)
        {
            var customer = CustomerDTO.ToCustomerModel();
            return await _customerRepo.AddAsync(customer);
        }

        public async Task<bool> UpdateBALDTOAsync(CustomerDTO CustomerDTO)
        {
            var customer = CustomerDTO.ToCustomerModel();
            return await _customerRepo.UpdateAsync(customer);
        }

        public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await AddAsync(customer);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(customer);
            }
        }
    }
}
