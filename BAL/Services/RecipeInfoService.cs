using BAL.Interfaces;
using BAL.Mappers;
using DAL.IRepo;
using Microsoft.AspNetCore.Http;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class RecipeInfoService : IRecipeInfoService
    {
        private readonly IRecipeInfoRepo _recipeInfoRepo;
        private readonly ICurrentUserService _currentUserServ;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public clsRecipeInfo recipeInfo { get; set; }

        public RecipeInfoService(IRecipeInfoRepo recipeInfoRepo, ICurrentUserService currentUser)
        {
            _recipeInfoRepo = recipeInfoRepo;
            _currentUserServ = currentUser;
        }

        public async Task<bool> AddAsync(clsRecipeInfo recipeInfo)
        {
            recipeInfo.UserID = _currentUserServ.GetCurrentUserId();
            recipeInfo.ActionDate = DateTime.Now;
            recipeInfo.ActionType = 1; // Add
            
            return await _recipeInfoRepo.AddAsync(recipeInfo);
        }

        public async Task<bool> UpdateAsync(clsRecipeInfo recipeInfo)
        {
            recipeInfo.UserID = _currentUserServ.GetCurrentUserId();
            recipeInfo.ActionDate = DateTime.Now;
            recipeInfo.ActionType = 2; // Update
            
            return await _recipeInfoRepo.UpdateAsync(recipeInfo);
        }

        public async Task<bool> DeleteAsync(int recipeInfoID)
        {
            return await _recipeInfoRepo.DeleteAsync(recipeInfoID,_currentUserServ.GetCurrentUserId());
        }

        public async Task<clsRecipeInfo> GetByIdAsync(int recipeInfoID)
        {
            return await _recipeInfoRepo.GetByIdAsync(recipeInfoID);
        }

        public async Task<List<clsRecipeInfo>> GetAllAsync()
        {
            return await _recipeInfoRepo.GetAllAsync();
        }

        // BALDTO Methods
        public async Task<RecipeInfoDTO> GetByIdBALDTOAsync(int recipeInfoID)
        {
            return await _recipeInfoRepo.GetByIdDTOAsync(recipeInfoID);
        }

        public async Task<List<RecipeInfoDTO>> GetAllBALDTOAsync()
        {
            return await _recipeInfoRepo.GetAllDTOAsync();
        }

        public async Task<List<RecipeInfoDTO>> GetAllBALDTOAsync(clsRecipeInfoFilter filter)
        {
            return await _recipeInfoRepo.GetAllDTOAsync(filter);
        }

        public async Task<bool> AddBALDTOAsync(RecipeInfoDTO recipeInfoDTO)
        {
            var recipeInfo = BALMappers.ToRecipeInfoModel(recipeInfoDTO);
            recipeInfo.UserID = _currentUserServ.GetCurrentUserId();
            recipeInfo.ActionDate = DateTime.Now;
            recipeInfo.ActionType = 1; // Add
            
            return await _recipeInfoRepo.AddDTOAsync(recipeInfoDTO);
        }

        public async Task<bool> UpdateBALDTOAsync(RecipeInfoDTO recipeInfoDTO)
        {
            var recipeInfo = BALMappers.ToRecipeInfoModel(recipeInfoDTO);
            recipeInfo.UserID = _currentUserServ.GetCurrentUserId();
            recipeInfo.ActionDate = DateTime.Now;
            recipeInfo.ActionType = 2; // Update
            
            return await _recipeInfoRepo.UpdateDTOAsync(recipeInfoDTO);
        }

 
    }
}
