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
    public class RecipeInfoRepo : IRecipeInfoRepo
    {
        private readonly AppDBContext _context;

        public RecipeInfoRepo(AppDBContext context)
        {
            _context = context;
        }

       
        public clsRecipeInfo recipeInfo { get; set; }

        public async Task<List<clsRecipeInfo>> GetAllAsync()
        {
            return await _context.RecipeInfos
                .Include(ri => ri.Recipe)
                .Include(ri => ri.RawMaterial)
                .Include(ri => ri.User)
                .ToListAsync();
        }

        public async Task<List<RecipeInfoDTO>> GetAllDTOAsync(clsRecipeInfoFilter filter)
        {
            return await clsDALUtil.ExecuteFilterCommands<RecipeInfoDTO, clsRecipeInfoFilter>(_context, filter, filter.FilterName);
        }

        public async Task<List<RecipeInfoDTO>> GetAllDTOAsync()
        {
            return await _context.RecipeInfos
                .Include(ri => ri.Recipe)
                .Include(ri => ri.RawMaterial)
                .Select(ri => new RecipeInfoDTO
                {
                    ID = ri.ID,
                    RecipeID = ri.RecipeID,
                    RecipeName = ri.Recipe.Name,
                    MaterialID = ri.RawMaterialID,
                    MaterialName = ri.RawMaterial.Name,

                    RequiredMaterialQuantity = ri.RequiredMaterialQuantity,
                    ActionDate = ri.ActionDate
                }).ToListAsync();
        }
       public async Task<List<clsRecipeInfo>> GetAllByRecipeIDAsync(int RecipeID)
        {
           return await _context.RecipeInfos.Where(r => r.RecipeID == RecipeID).ToListAsync();
        }
        public async Task<clsRecipeInfo> GetByIdAsync(int recipeInfoID)
        {
            return await _context.RecipeInfos
                .Include(ri => ri.Recipe)
                .Include(ri => ri.RawMaterial)
                .Include(ri => ri.User)
                .FirstOrDefaultAsync(ri => ri.ID == recipeInfoID);
        }

        public async Task<RecipeInfoDTO> GetByIdDTOAsync(int recipeInfoID)
        {
           clsRecipeInfoFilter filter = new clsRecipeInfoFilter();
            filter.ID = recipeInfoID;
               var result =  await GetAllDTOAsync(filter);
            return result.FirstOrDefault();
        }

        public async Task<bool> AddAsync(clsRecipeInfo recipeInfo)
        {
            try
            {
                _context.RecipeInfos.Add(recipeInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddDTOAsync(RecipeInfoDTO recipeInfoDTO)
        {
            try
            {
                var recipeInfo = new clsRecipeInfo
                {
                    RecipeID = recipeInfoDTO.RecipeID,
                    RawMaterialID = recipeInfoDTO.MaterialID,
                    RequiredMaterialQuantity = recipeInfoDTO.RequiredMaterialQuantity,
                    UserID = recipeInfoDTO.UserID,
                    ActionType = 1,
                    ActionDate = DateTime.Now
                };

                _context.RecipeInfos.Add(recipeInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsRecipeInfo recipeInfo)
        {
            try
            {
                _context.RecipeInfos.Update(recipeInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDTOAsync(RecipeInfoDTO recipeInfoDTO)
        {
            try
            {
                var recipeInfo = await _context.RecipeInfos.FindAsync(recipeInfoDTO.ID);
                if (recipeInfo != null)
                {
                    recipeInfo.RawMaterialID = recipeInfoDTO.MaterialID;
                    recipeInfo.RequiredMaterialQuantity = recipeInfoDTO.RequiredMaterialQuantity;
                    recipeInfo.UserID = recipeInfoDTO.UserID;
                    recipeInfo.ActionType = 2;
                    recipeInfo.ActionDate = DateTime.Now;

                    _context.RecipeInfos.Update(recipeInfo);
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

        public async Task<bool> DeleteAsync(int recipeInfoID, string UserID)
        {
            try
            {
                var recipeInfo = await _context.RecipeInfos.FindAsync(recipeInfoID);
                if (recipeInfo != null)
                {
                    recipeInfo.ActionType = 3; // Soft Delete
                    recipeInfo.UserID = UserID; 
                    _context.RecipeInfos.Update(recipeInfo);
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
