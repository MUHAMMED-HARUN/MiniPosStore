using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IExpenseService
    {
        // ExpenseType Methods
        Task<int> CreateExpenseTypeAsync(clsExpenseType expenseType);
        Task<bool> UpdateExpenseTypeAsync(clsExpenseType expenseType);
        Task<bool> DeleteExpenseTypeAsync(int id);
        Task<clsExpenseType> GetExpenseTypeByIdAsync(int id);
        Task<List<clsExpenseType>> GetAllExpenseTypesAsync();
        
        // ExpenseType DTO Methods
        Task<ExpenseTypeDTO> GetExpenseTypeByIdDTOAsync(int id);
        Task<List<ExpenseTypeDTO>> GetAllExpenseTypesDTOAsync();
        Task<List<ExpenseTypeDTO>> GetAllExpenseTypesDTOAsync(clsExpenseTypeFilter filter);

        // Expenses Methods
        Task<int> CreateExpenseAsync(clsExpenses expense);
        Task<bool> UpdateExpenseAsync(clsExpenses expense);
        Task<bool> DeleteExpenseAsync(int id);
        Task<clsExpenses> GetExpenseByIdAsync(int id);
        Task<List<clsExpenses>> GetAllExpensesAsync();
        
        // Expenses DTO Methods
        Task<ExpensesDTO> GetExpenseByIdDTOAsync(int id);
        Task<List<ExpensesDTO>> GetAllExpensesDTOAsync();
        Task<List<ExpensesDTO>> GetAllExpensesDTOAsync(clsExpensesFilter filter);
    }
}
