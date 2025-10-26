using BAL.Interfaces;
using BAL.Mappers;
using DAL.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepo _recipeRepo;
        private readonly ICurrentUserService _currentUserServ;
        private readonly DAL.IRepo.IRawMaterialService _materialServ;
        private readonly IRecipeInfoRepo _recipeInfoRepo;
        private readonly IProductService _ProductService;
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public clsRecipe recipe { get; set; }

        public RecipeService(IRecipeRepo recipeRepo, ICurrentUserService currentUser, DAL.IRepo.IRawMaterialService materialRepo , IRecipeInfoRepo recipeInfo,IProductService productService)
        {
            _recipeRepo = recipeRepo;
            _currentUserServ = currentUser;
            _materialServ = materialRepo;
            _recipeInfoRepo = recipeInfo;
            _ProductService = productService;
        }

        public async Task<bool> AddAsync(clsRecipe recipe)
        {
            recipe.UserID = _currentUserServ.GetCurrentUserId();
            recipe.ActionDate = DateTime.Now;
            recipe.ActionType = 1; // Add

            return await _recipeRepo.AddAsync(recipe);
        }

        public async Task<bool> UpdateAsync(clsRecipe recipe)
        {
            recipe.UserID = _currentUserServ.GetCurrentUserId();
            recipe.ActionDate = DateTime.Now;
            recipe.ActionType = 2; // Update

            return await _recipeRepo.UpdateAsync(recipe);
        }

        public async Task<bool> DeleteAsync(int recipeID)
        {
            return await _recipeRepo.DeleteAsync(recipeID, _currentUserServ.GetCurrentUserId());
        }

        public async Task<clsRecipe> GetByIdAsync(int recipeID)
        {
            return await _recipeRepo.GetByIdAsync(recipeID);
        }

        public async Task<List<clsRecipe>> GetAllAsync()
        {
            return await _recipeRepo.GetAllAsync();
        }

        // BALDTO Methods
        public async Task<RecipeDTO> GetByIdBALDTOAsync(int recipeID)
        {
            return await _recipeRepo.GetByIdDTOAsync(recipeID);
        }

        public async Task<List<RecipeDTO>> GetAllBALDTOAsync()
        {
            return await _recipeRepo.GetAllDTOAsync();
        }

        public async Task<List<RecipeDTO>> GetAllBALDTOAsync(clsRecipeFilter filter)
        {
            return await _recipeRepo.GetAllDTOAsync(filter);
        }

        public async Task<bool> AddBALDTOAsync(RecipeDTO recipeDTO)
        {
            var recipe = BALMappers.ToRecipeModel(recipeDTO);
            recipe.UserID = _currentUserServ.GetCurrentUserId();
            recipe.ActionDate = DateTime.Now;
            recipe.ActionType = 1; // Add

            return await _recipeRepo.AddDTOAsync(recipeDTO);
        }

        public async Task<bool> UpdateBALDTOAsync(RecipeDTO recipeDTO)
        {
            var recipe = BALMappers.ToRecipeModel(recipeDTO);
            recipe.UserID = _currentUserServ.GetCurrentUserId();
            recipe.ActionDate = DateTime.Now;
            recipe.ActionType = 2; // Update

            return await _recipeRepo.UpdateDTOAsync(recipeDTO);
        }
        public async Task<bool> ProductProduction(int recipeID)
        {
            try
            {
                clsRecipe recipe = await GetByIdAsync(recipeID);
                if (recipe == null)
                    return false;
                    
                List<clsRecipeInfo> RecipesInfo = await _recipeInfoRepo.GetAllByRecipeIDAsync(recipeID);
                if (RecipesInfo == null || !RecipesInfo.Any())
                    return false;
                    
                Dictionary<int, float> MateriaLInfos = RecipesInfo.ToDictionary(ri => ri.RawMaterialID, ri => ri.RequiredMaterialQuantity);
                
                // تحقق من توفر المواد الخام أولاً
           
                    
                // زيادة كمية المنتج
                bool Result =   await  _materialServ.DecreaseByRange(MateriaLInfos);
                if (Result)
                {
                    return await _ProductService.IncreaseProductQuantityAsync(recipe.ProductID, recipe.YieldQuantity, _currentUserServ.GetCurrentUserId());
                }
                return false;
            }
            catch (SqlException E)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
