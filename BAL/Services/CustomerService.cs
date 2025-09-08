using BAL.BALDTO;
using BAL.BALFilters;
using BAL.Interfaces;
using BAL.Mappers;
using DAL.EF.DTO;
using DAL.EF.Filters;
using DAL.EF.Models;
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
        public async Task<CustomerBALDTO> GetByIdBALDTOAsync(int customerID)
        {
            var customer = await _customerRepo.GetByIdAsync(customerID);
            return customer?.ToCustomerBALDTO();
        }

        public async Task<List<CustomerBALDTO>> GetAllBALDTOAsync()
        {
            var customers = await _customerRepo.GetAllAsync();
            return customers.ToCustomerBALDTOList();
        }
      public  async Task<List<CustomerBALDTO>> GetAllBALDTOAsync(clsCustomerFilterBAL filter)
        {
                var Customer =await _customerRepo.GetAllDTOAsync(filter);
            return Customer.Select(c => new CustomerBALDTO
            {
                CustomerID = c.CustomerID,
                PersonID = c.PersonID,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                RemainingAmount = c.RemainingAmount,
            }).ToList();
   
        }
        public async Task<bool> AddBALDTOAsync(CustomerBALDTO customerBALDTO)
        {
            var customer = customerBALDTO.ToCustomerModel();
            return await _customerRepo.AddAsync(customer);
        }

        public async Task<bool> UpdateBALDTOAsync(CustomerBALDTO customerBALDTO)
        {
            var customer = customerBALDTO.ToCustomerModel();
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
