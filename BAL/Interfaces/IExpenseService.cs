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
    public interface IExpenseService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        
        // ExpenseType Methods
        Task<bool> CreateExpenseTypeAsync(clsExpenseType expenseType);
        Task<bool> UpdateExpenseTypeAsync(clsExpenseType expenseType);
        Task<bool> DeleteExpenseTypeAsync(int id);
        Task<clsExpenseType> GetExpenseTypeByIdAsync(int id);
        Task<List<clsExpenseType>> GetAllExpenseTypesAsync();
        
        // ExpenseType BALDTO Methods
        Task<ExpenseTypeDTO> GetExpenseTypeByIdBALDTOAsync(int id);
        Task<List<ExpenseTypeDTO>> GetAllExpenseTypesBALDTOAsync();
        Task<List<ExpenseTypeDTO>> GetAllExpenseTypesBALDTOAsync(clsExpenseTypeFilter filter);
        Task<bool> CreateExpenseTypeBALDTOAsync(ExpenseTypeDTO expenseTypeDTO);
        Task<bool> UpdateExpenseTypeBALDTOAsync(ExpenseTypeDTO expenseTypeDTO);

        // Expenses Methods
        Task<bool> CreateExpenseAsync(clsExpenses expense);
        Task<bool> UpdateExpenseAsync(clsExpenses expense);
        Task<bool> DeleteExpenseAsync(int id);
        Task<clsExpenses> GetExpenseByIdAsync(int id);
        Task<List<clsExpenses>> GetAllExpensesAsync();
        
        // Expenses BALDTO Methods
        Task<ExpensesDTO> GetExpenseByIdBALDTOAsync(int id);
        Task<List<ExpensesDTO>> GetAllExpensesBALDTOAsync();
        Task<List<ExpensesDTO>> GetAllExpensesBALDTOAsync(clsExpensesFilter filter);
        Task<bool> CreateExpenseBALDTOAsync(ExpensesDTO expensesDTO);
        Task<bool> UpdateExpenseBALDTOAsync(ExpensesDTO expensesDTO);
        
        // Search Methods
        Task<List<clsExpenseType>> SearchExpenseTypesAsync(string searchTerm);
        Task<List<clsExpenses>> SearchExpensesAsync(string searchTerm);
        
        // Summary Methods
        Task<float> GetTotalExpensesAsync();
        Task<float> GetTotalExpensesByTypeAsync(int expenseTypeId);
        Task<float> GetTotalExpensesByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        Task<bool> Save();
    }
}
