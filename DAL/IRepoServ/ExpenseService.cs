using DAL.IRepo;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.EF.AppDBContext;

namespace DAL.IRepoServ
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppDBContext _context;

        public ExpenseService(AppDBContext context)
        {
            _context = context;
        }

        #region ExpenseType Methods

        public async Task<int> CreateExpenseTypeAsync(clsExpenseType expenseType)
        {
            _context.ExpenseTypes.Add(expenseType);
            await _context.SaveChangesAsync();
            return expenseType.ID;
        }

        public async Task<bool> UpdateExpenseTypeAsync(clsExpenseType expenseType)
        {
            var existingExpenseType = await _context.ExpenseTypes.FindAsync(expenseType.ID);
            if (existingExpenseType == null)
                return false;

            existingExpenseType.Name = expenseType.Name;
            existingExpenseType.Description = expenseType.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteExpenseTypeAsync(int id)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(id);
            if (expenseType == null)
                return false;

            _context.ExpenseTypes.Remove(expenseType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<clsExpenseType> GetExpenseTypeByIdAsync(int id)
        {
            return await _context.ExpenseTypes.FindAsync(id);
        }

        public async Task<List<clsExpenseType>> GetAllExpenseTypesAsync()
        {
            return await _context.ExpenseTypes.ToListAsync();
        }

        #endregion

        #region ExpenseType DTO Methods

        public async Task<ExpenseTypeDTO> GetExpenseTypeByIdDTOAsync(int id)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(id);
            if (expenseType == null)
                return null;

            return new ExpenseTypeDTO
            {
                ID = expenseType.ID,
                Name = expenseType.Name,
                Description = expenseType.Description
            };
        }

        public async Task<List<ExpenseTypeDTO>> GetAllExpenseTypesDTOAsync()
        {
            return await _context.ExpenseTypes
                .Select(et => new ExpenseTypeDTO
                {
                    ID = et.ID,
                    Name = et.Name,
                    Description = et.Description
                })
                .ToListAsync();
        }

        public async Task<List<ExpenseTypeDTO>> GetAllExpenseTypesDTOAsync(clsExpenseTypeFilter filter)
        {
            var query = _context.ExpenseTypes.AsQueryable();

            if (filter.ID.HasValue)
                query = query.Where(et => et.ID == filter.ID.Value);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(et => et.Name.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(et => et.Description.Contains(filter.Description));

            return await query
                .Select(et => new ExpenseTypeDTO
                {
                    ID = et.ID,
                    Name = et.Name,
                    Description = et.Description
                })
                .ToListAsync();
        }

        #endregion

        #region Expenses Methods

        public async Task<int> CreateExpenseAsync(clsExpenses expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense.ID;
        }

        public async Task<bool> UpdateExpenseAsync(clsExpenses expense)
        {
            var existingExpense = await _context.Expenses.FindAsync(expense.ID);
            if (existingExpense == null)
                return false;

            existingExpense.ExpenseDate = expense.ExpenseDate;
            existingExpense.ExpenseTypeID = expense.ExpenseTypeID;
            existingExpense.Description = expense.Description;
            existingExpense.Amount = expense.Amount;
            existingExpense.ActionByUser = expense.ActionByUser;
            existingExpense.ActionType = expense.ActionType;
            existingExpense.ActionDate = expense.ActionDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<clsExpenses> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<List<clsExpenses>> GetAllExpensesAsync()
        {
            return await _context.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .ToListAsync();
        }

        #endregion

        #region Expenses DTO Methods

        public async Task<ExpensesDTO> GetExpenseByIdDTOAsync(int id)
        {
            var expense = await _context.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (expense == null)
                return null;

            return new ExpensesDTO
            {
                ID = expense.ID,
                ExpenseDate = expense.ExpenseDate,
                ExpenseTypeID = expense.ExpenseTypeID,
                ExpenseTypeName = expense.ExpenseType?.Name ?? "",
                Description = expense.Description,
                Amount = expense.Amount,
                ActionByUser = expense.ActionByUser,
                UserName = expense.User?.UserName ?? "",
                ActionType = expense.ActionType,
                ActionDate = expense.ActionDate
            };
        }

        public async Task<List<ExpensesDTO>> GetAllExpensesDTOAsync()
        {
            return await _context.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .Select(e => new ExpensesDTO
                {
                    ID = e.ID,
                    ExpenseDate = e.ExpenseDate,
                    ExpenseTypeID = e.ExpenseTypeID,
                    ExpenseTypeName = e.ExpenseType.Name,
                    Description = e.Description,
                    Amount = e.Amount,
                    ActionByUser = e.ActionByUser,
                    UserName = e.User.UserName,
                    ActionType = e.ActionType,
                    ActionDate = e.ActionDate
                })
                .ToListAsync();
        }

        public async Task<List<ExpensesDTO>> GetAllExpensesDTOAsync(clsExpensesFilter filter)
        {
            var query = _context.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .AsQueryable();

            if (filter.ID.HasValue)
                query = query.Where(e => e.ID == filter.ID.Value);

            if (filter.ExpenseDateFrom.HasValue)
                query = query.Where(e => e.ExpenseDate >= filter.ExpenseDateFrom.Value);

            if (filter.ExpenseDateTo.HasValue)
                query = query.Where(e => e.ExpenseDate <= filter.ExpenseDateTo.Value);

            if (filter.ExpenseTypeID.HasValue)
                query = query.Where(e => e.ExpenseTypeID == filter.ExpenseTypeID.Value);

            if (!string.IsNullOrEmpty(filter.ExpenseTypeName))
                query = query.Where(e => e.ExpenseType.Name.Contains(filter.ExpenseTypeName));

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(e => e.Description.Contains(filter.Description));

            if (filter.AmountFrom.HasValue)
                query = query.Where(e => e.Amount >= filter.AmountFrom.Value);

            if (filter.AmountTo.HasValue)
                query = query.Where(e => e.Amount <= filter.AmountTo.Value);

            if (!string.IsNullOrEmpty(filter.ActionByUser))
                query = query.Where(e => e.ActionByUser.Contains(filter.ActionByUser));

            if (!string.IsNullOrEmpty(filter.UserName))
                query = query.Where(e => e.User.UserName.Contains(filter.UserName));

            if (filter.ActionType.HasValue)
                query = query.Where(e => e.ActionType == filter.ActionType.Value);

            if (filter.ActionDateFrom.HasValue)
                query = query.Where(e => e.ActionDate >= filter.ActionDateFrom.Value);

            if (filter.ActionDateTo.HasValue)
                query = query.Where(e => e.ActionDate <= filter.ActionDateTo.Value);

            return await query
                .Select(e => new ExpensesDTO
                {
                    ID = e.ID,
                    ExpenseDate = e.ExpenseDate,
                    ExpenseTypeID = e.ExpenseTypeID,
                    ExpenseTypeName = e.ExpenseType.Name,
                    Description = e.Description,
                    Amount = e.Amount,
                    ActionByUser = e.ActionByUser,
                    UserName = e.User.UserName,
                    ActionType = e.ActionType,
                    ActionDate = e.ActionDate
                })
                .ToListAsync();
        }

        #endregion
    }
}
