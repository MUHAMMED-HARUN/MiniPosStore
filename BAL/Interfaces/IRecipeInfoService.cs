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
    public interface IRecipeInfoService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsRecipeInfo recipeInfo { get; set; }
        
        Task<bool> AddAsync(clsRecipeInfo recipeInfo);
        Task<bool> UpdateAsync(clsRecipeInfo recipeInfo);
        Task<bool> DeleteAsync(int recipeInfoID);
        Task<clsRecipeInfo> GetByIdAsync(int recipeInfoID);
        Task<List<clsRecipeInfo>> GetAllAsync();
        
        // BALDTO Methods
        Task<RecipeInfoDTO> GetByIdBALDTOAsync(int recipeInfoID);
        Task<List<RecipeInfoDTO>> GetAllBALDTOAsync();
        Task<List<RecipeInfoDTO>> GetAllBALDTOAsync(clsRecipeInfoFilter filter);
        Task<bool> AddBALDTOAsync(RecipeInfoDTO recipeInfoDTO);
        Task<bool> UpdateBALDTOAsync(RecipeInfoDTO recipeInfoDTO);
        
    }
}
