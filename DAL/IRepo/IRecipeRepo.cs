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
    public interface IRecipeRepo
    {
     
        clsRecipe recipe { get; set; }
        
        Task<bool> AddAsync(clsRecipe recipe);
        Task<bool> UpdateAsync(clsRecipe recipe);
        Task<bool> DeleteAsync(int recipeID, string UserID);
        Task<clsRecipe> GetByIdAsync(int recipeID);
        Task<List<clsRecipe>> GetAllAsync();
        
        // DTO Methods
        Task<RecipeDTO> GetByIdDTOAsync(int recipeID);
        Task<List<RecipeDTO>> GetAllDTOAsync();
        Task<List<RecipeDTO>> GetAllDTOAsync(clsRecipeFilter filter);
        Task<bool> AddDTOAsync(RecipeDTO recipeDTO);
        Task<bool> UpdateDTOAsync(RecipeDTO recipeDTO);

    }
}
