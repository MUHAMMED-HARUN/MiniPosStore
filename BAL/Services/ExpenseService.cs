using BAL.Interfaces;
using DAL.IRepo;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ExpenseService : BAL.Interfaces.IExpenseService
    {
        private readonly DAL.IRepo.IExpenseService _expenseRepo;
        public clsGlobal.enSaveMode SaveMode { get; set; }

        public ExpenseService(DAL.IRepo.IExpenseService  expenseRepo)
        {
            _expenseRepo = expenseRepo;
        }

        #region ExpenseType Methods

        public async Task<bool> CreateExpenseTypeAsync(clsExpenseType expenseType)
        {
            try
            {
                await _expenseRepo.CreateExpenseTypeAsync(expenseType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateExpenseTypeAsync(clsExpenseType expenseType)
        {
            try
            {
                return await _expenseRepo.UpdateExpenseTypeAsync(expenseType);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteExpenseTypeAsync(int id)
        {
            try
            {
                return await _expenseRepo.DeleteExpenseTypeAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<clsExpenseType> GetExpenseTypeByIdAsync(int id)
        {
            try
            {
                return await _expenseRepo.GetExpenseTypeByIdAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<clsExpenseType>> GetAllExpenseTypesAsync()
        {
            try
            {
                return await _expenseRepo.GetAllExpenseTypesAsync();
            }
            catch (Exception)
            {
                return new List<clsExpenseType>();
            }
        }

        #endregion

        #region ExpenseType BALDTO Methods

        public async Task<ExpenseTypeDTO> GetExpenseTypeByIdBALDTOAsync(int id)
        {
            try
            {
                return await _expenseRepo.GetExpenseTypeByIdDTOAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ExpenseTypeDTO>> GetAllExpenseTypesBALDTOAsync()
        {
            try
            {
                return await _expenseRepo.GetAllExpenseTypesDTOAsync();
            }
            catch (Exception)
            {
                return new List<ExpenseTypeDTO>();
            }
        }

        public async Task<List<ExpenseTypeDTO>> GetAllExpenseTypesBALDTOAsync(clsExpenseTypeFilter filter)
        {
            try
            {
                return await _expenseRepo.GetAllExpenseTypesDTOAsync(filter);
            }
            catch (Exception)
            {
                return new List<ExpenseTypeDTO>();
            }
        }

        public async Task<bool> CreateExpenseTypeBALDTOAsync(ExpenseTypeDTO expenseTypeDTO)
        {
            try
            {
                var expenseType = new clsExpenseType
                {
                    Name = expenseTypeDTO.Name,
                    Description = expenseTypeDTO.Description
                };

                await _expenseRepo.CreateExpenseTypeAsync(expenseType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateExpenseTypeBALDTOAsync(ExpenseTypeDTO expenseTypeDTO)
        {
            try
            {
                var expenseType = new clsExpenseType
                {
                    ID = expenseTypeDTO.ID,
                    Name = expenseTypeDTO.Name,
                    Description = expenseTypeDTO.Description
                };

                return await _expenseRepo.UpdateExpenseTypeAsync(expenseType);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Expenses Methods

        public async Task<bool> CreateExpenseAsync(clsExpenses expense)
        {
            try
            {
                await _expenseRepo.CreateExpenseAsync(expense);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateExpenseAsync(clsExpenses expense)
        {
            try
            {
                return await _expenseRepo.UpdateExpenseAsync(expense);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            try
            {
                return await _expenseRepo.DeleteExpenseAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<clsExpenses> GetExpenseByIdAsync(int id)
        {
            try
            {
                return await _expenseRepo.GetExpenseByIdAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<clsExpenses>> GetAllExpensesAsync()
        {
            try
            {
                return await _expenseRepo.GetAllExpensesAsync();
            }
            catch (Exception)
            {
                return new List<clsExpenses>();
            }
        }

        #endregion

        #region Expenses BALDTO Methods

        public async Task<ExpensesDTO> GetExpenseByIdBALDTOAsync(int id)
        {
            try
            {
                return await _expenseRepo.GetExpenseByIdDTOAsync(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ExpensesDTO>> GetAllExpensesBALDTOAsync()
        {
            try
            {
                return await _expenseRepo.GetAllExpensesDTOAsync();
            }
            catch (Exception)
            {
                return new List<ExpensesDTO>();
            }
        }

        public async Task<List<ExpensesDTO>> GetAllExpensesBALDTOAsync(clsExpensesFilter filter)
        {
            try
            {
                return await _expenseRepo.GetAllExpensesDTOAsync(filter);
            }
            catch (Exception)
            {
                return new List<ExpensesDTO>();
            }
        }

        public async Task<bool> CreateExpenseBALDTOAsync(ExpensesDTO expensesDTO)
        {
            try
            {
                var expense = new clsExpenses
                {
                    ExpenseDate = expensesDTO.ExpenseDate,
                    ExpenseTypeID = expensesDTO.ExpenseTypeID,
                    Description = expensesDTO.Description,
                    Amount = expensesDTO.Amount,
                    ActionByUser = expensesDTO.ActionByUser,
                    ActionType = expensesDTO.ActionType,
                    ActionDate = expensesDTO.ActionDate
                };

                await _expenseRepo.CreateExpenseAsync(expense);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateExpenseBALDTOAsync(ExpensesDTO expensesDTO)
        {
            try
            {
                var expense = new clsExpenses
                {
                    ID = expensesDTO.ID,
                    ExpenseDate = expensesDTO.ExpenseDate,
                    ExpenseTypeID = expensesDTO.ExpenseTypeID,
                    Description = expensesDTO.Description,
                    Amount = expensesDTO.Amount,
                    ActionByUser = expensesDTO.ActionByUser,
                    ActionType = expensesDTO.ActionType,
                    ActionDate = expensesDTO.ActionDate
                };

                return await _expenseRepo.UpdateExpenseAsync(expense);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Search Methods

        public async Task<List<clsExpenseType>> SearchExpenseTypesAsync(string searchTerm)
        {
            try
            {
                var allExpenseTypes = await _expenseRepo.GetAllExpenseTypesDTOAsync();
                return allExpenseTypes
                    .Where(et => et.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                (et.Description != null && et.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                    .Select(et => new clsExpenseType
                    {
                        ID = et.ID,
                        Name = et.Name,
                        Description = et.Description
                    })
                    .ToList();
            }
            catch (Exception)
            {
                return new List<clsExpenseType>();
            }
        }

        public async Task<List<clsExpenses>> SearchExpensesAsync(string searchTerm)
        {
            try
            {
                var allExpenses = await _expenseRepo.GetAllExpensesDTOAsync();
                return allExpenses
                    .Where(e => e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                e.ExpenseTypeName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                e.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(e => new clsExpenses
                    {
                        ID = e.ID,
                        ExpenseDate = e.ExpenseDate,
                        ExpenseTypeID = e.ExpenseTypeID,
                        Description = e.Description,
                        Amount = e.Amount,
                        ActionByUser = e.ActionByUser,
                        ActionType = e.ActionType,
                        ActionDate = e.ActionDate
                    })
                    .ToList();
            }
            catch (Exception)
            {
                return new List<clsExpenses>();
            }
        }

        #endregion

        #region Summary Methods

        public async Task<float> GetTotalExpensesAsync()
        {
            try
            {
                var expenses = await _expenseRepo.GetAllExpensesDTOAsync();
                return expenses.Sum(e => e.Amount);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<float> GetTotalExpensesByTypeAsync(int expenseTypeId)
        {
            try
            {
                var expenses = await _expenseRepo.GetAllExpensesDTOAsync();
                return expenses.Where(e => e.ExpenseTypeID == expenseTypeId).Sum(e => e.Amount);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<float> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var expenses = await _expenseRepo.GetAllExpensesDTOAsync();
                return expenses.Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate).Sum(e => e.Amount);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        public async Task<bool> Save()
        {
            try
            {
                // Implementation for saving changes if needed
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
