using DAL.EF.AppDBContext;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using DAL.IRepo;
using Microsoft.Data.SqlClient;
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

        public async Task<List<CustomerDTO>> GetAllDTOAsync(clsCustomerFilter filter)
        {

            {
                string Query = @$"select * from GetCustomersFiltred ( {clsDALUtil.GetSqlPrameterString<clsCustomerFilter>()})";

                using (var connection = _context.Database.GetDbConnection().CreateCommand())
                {
                    connection.CommandText = Query;
                    connection.CommandType = System.Data.CommandType.Text;

                    if (connection.Connection.State != System.Data.ConnectionState.Open)
                        connection.Connection.Open();

                    var arr = clsDALUtil.GetSqlPrameters<clsCustomerFilter>(filter).ToArray();
                    connection.Parameters.AddRange(arr);

                    List<CustomerDTO> Customers = new List<CustomerDTO>();
                    try
                    {
                        using (var reader = connection.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var customer = new CustomerDTO();
                                clsDALUtil.MapToClass<CustomerDTO>(reader, ref customer);
                                Customers.Add(customer);
                            }
                        }
                    }
                    catch (SqlException s)
                    {
                        int sd = 5;
                    }

                    return Customers;
                }
            }
        }
    }
}
