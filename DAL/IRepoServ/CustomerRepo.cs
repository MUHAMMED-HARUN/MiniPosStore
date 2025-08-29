using DAL.EF.AppDBContext;
using DAL.EF.Models;
using DAL.EF.DTO;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDBContext _context;

        public CustomerRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(clsCustomer customer)
        {
            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsCustomer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerId);
                if (customer == null)
                    return false;
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsCustomer> GetByIdAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == customerId);
        }

        public async Task<List<clsCustomer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.Person)
                .ToListAsync();
        }

        public async Task<clsCustomer> GetByPersonIdAsync(int personId)
        {
            return await _context.Customers
                .Include(c => c.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PersonID == personId);
        }

        public async Task<CustomerDTO> GetByIdDTOAsync(int customerID)
        {
            return await _context.Customers.AsNoTracking()
                .Where(c => c.ID == customerID)
                .Select(c => new CustomerDTO
                {
                    CustomerID = c.ID,
                    PersonID = c.PersonID,
                    FirstName = c.Person.FirstName,
                    LastName = c.Person.LastName,
                    PhoneNumber = c.Person.PhoneNumber,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CustomerDTO>> GetAllDTOAsync()
        {
            return await _context.Customers.AsNoTracking()
                .Select(c => new CustomerDTO
                {
                    CustomerID = c.ID,
                    PersonID = c.PersonID,
                    FirstName = c.Person.FirstName,
                    LastName = c.Person.LastName,
                    PhoneNumber = c.Person.PhoneNumber,
                })
                .ToListAsync();
        }
    }
}
