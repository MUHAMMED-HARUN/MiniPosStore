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
    public interface IRecipeInfoRepo
    {
   
        clsRecipeInfo recipeInfo { get; set; }
        
        Task<bool> AddAsync(clsRecipeInfo recipeInfo);
        Task<bool> UpdateAsync(clsRecipeInfo recipeInfo);
        Task<bool> DeleteAsync(int recipeInfoID,string UserID);
        Task<clsRecipeInfo> GetByIdAsync(int recipeInfoID);
        Task<List<clsRecipeInfo>> GetAllAsync();
        Task<List<clsRecipeInfo>> GetAllByRecipeIDAsync(int RecipeID);

        // DTO Methods
        Task<RecipeInfoDTO> GetByIdDTOAsync(int recipeInfoID);
        Task<List<RecipeInfoDTO>> GetAllDTOAsync();
        Task<List<RecipeInfoDTO>> GetAllDTOAsync(clsRecipeInfoFilter filter);
        Task<bool> AddDTOAsync(RecipeInfoDTO recipeInfoDTO);
        Task<bool> UpdateDTOAsync(RecipeInfoDTO recipeInfoDTO);
        
    }
}
