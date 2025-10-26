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
    public interface IRecipeService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsRecipe recipe { get; set; }
        
        Task<bool> AddAsync(clsRecipe recipe);
        Task<bool> UpdateAsync(clsRecipe recipe);
        Task<bool> DeleteAsync(int recipeID);
        Task<clsRecipe> GetByIdAsync(int recipeID);
        Task<List<clsRecipe>> GetAllAsync();
        
        // BALDTO Methods
        Task<RecipeDTO> GetByIdBALDTOAsync(int recipeID);
        Task<List<RecipeDTO>> GetAllBALDTOAsync();
        Task<List<RecipeDTO>> GetAllBALDTOAsync(clsRecipeFilter filter);
        Task<bool> AddBALDTOAsync(RecipeDTO recipeDTO);
        Task<bool> UpdateBALDTOAsync(RecipeDTO recipeDTO);

        Task<bool> ProductProduction(int recipeID);

    }
}
