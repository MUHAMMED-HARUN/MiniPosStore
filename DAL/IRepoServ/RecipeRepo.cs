using DAL.EF.AppDBContext;
using DAL.IRepo;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class RecipeRepo : IRecipeRepo
    {
        private readonly AppDBContext _context;

        public RecipeRepo(AppDBContext context)
        {
            _context = context;
        }

       
        public clsRecipe recipe { get; set; }

        public async Task<List<clsRecipe>> GetAllAsync()
        {
            return await _context.Recipes
                .Include(r => r.Product)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<List<RecipeDTO>> GetAllDTOAsync(clsRecipeFilter filter)
        {
          return await  clsDALUtil.ExecuteFilterCommands<RecipeDTO, clsRecipeFilter>(_context, filter, filter.FilterName);
        }

        public async Task<List<RecipeDTO>> GetAllDTOAsync()
        {
            return await _context.Recipes
                .Include(r => r.Product)
                .Select(r => new RecipeDTO
                {
                    ID = r.ID,
                    Name = r.Name,
                    Description = r.Description,
                    ProductID = r.ProductID,
                    ProductName = r.Product.Name,
                    YieldQuantity = r.YieldQuantity,
                    ActionDate = r.ActionDate
                }).ToListAsync();
        }

        public async Task<clsRecipe> GetByIdAsync(int recipeID)
        {
            try
            {
             var obj=       await _context.Recipes
    .Include(r => r.Product)
    .Include(r => r.User)
    .FirstOrDefaultAsync(r => r.ID == recipeID);
                return obj;
            }
            catch(SqlException E)
            {
                return null;
            }

        }

        public async Task<RecipeDTO> GetByIdDTOAsync(int recipeID)
        {
            clsRecipeFilter filter = new clsRecipeFilter();
            filter.id = recipeID;
            var result=await GetAllDTOAsync(filter);
            return result.FirstOrDefault();
        }

        public async Task<bool> AddAsync(clsRecipe recipe)
        {
            try
            {
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddDTOAsync(RecipeDTO recipeDTO)
        {
            try
            {
                var recipe = new clsRecipe
                {
                    Name = recipeDTO.Name,
                    Description = recipeDTO.Description,
                    ProductID = recipeDTO.ProductID,
                    YieldQuantity = recipeDTO.YieldQuantity,
                    UserID = recipeDTO.UserID,
               
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsRecipe recipe)
        {
            try
            {
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDTOAsync(RecipeDTO recipeDTO)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(recipeDTO.ID);
                if (recipe != null)
                {
                    recipe.Name = recipeDTO.Name;
                    recipe.Description = recipeDTO.Description;
                    recipe.ProductID = recipeDTO.ProductID;
                    recipe.YieldQuantity = recipeDTO.YieldQuantity;
                    recipe.ActionDate = DateTime.Now;
                    recipe.UserID = recipeDTO.UserID;

                    _context.Recipes.Update(recipe);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int recipeID,string UserID)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(recipeID);
                if (recipe != null)
                {
                    
                    recipe.ActionType = 3; // Soft Delete
                    _context.Recipes.Update(recipe);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


    }
}
