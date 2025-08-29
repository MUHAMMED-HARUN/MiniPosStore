using BAL.Interfaces;
using BAL.BALDTO;
using BAL.Mappers;
using DAL.EF.Models;
using DAL.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<List<clsProduct>> GetAllProductsAsync()
        {
            return await _productRepo.GetAllProductsAsync();
        }

        public async Task<clsProduct> GetProductByIdAsync(int id)
        {
            return await _productRepo.GetProductByIdAsync(id);
        }

        public async Task<clsProduct> CreateProductAsync(clsProduct product, string currentUserId, string uploadPath)
        {
            product.ActionByUser = currentUserId;
            product.ActionDate = DateTime.Now;
            product.ActionType = 1; // Add
            
            var createdProduct = await _productRepo.CreateProductAsync(product);
            return createdProduct;
        }

        public async Task<clsProduct> UpdateProductAsync(clsProduct product, string currentUserId, string uploadPath)
        {
            product.ActionByUser = currentUserId;
            product.ActionDate = DateTime.Now;
            product.ActionType = 2; // Update
            
            var updatedProduct = await _productRepo.UpdateProductAsync(product);
            return updatedProduct;
        }

        public async Task<bool> DeleteProductAsync(int id, string uploadPath)
        {
            // حذف الصورة المرتبطة بالمنتج
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product != null && !string.IsNullOrEmpty(product.ImagePath))
            {
                clsUtil.DeleteImage(product.ImagePath, uploadPath);
            }

            return await _productRepo.DeleteProductAsync(id);
        }

        public async Task<List<clsProduct>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepo.SearchProductsAsync(searchTerm);
        }
        public async Task<bool> IncreaseProductQuantityAsync( int[] ImportOrderItmesID, string ActionByUser)
        {
            return await _productRepo.IncreaseProductQuantityAsync( ImportOrderItmesID, ActionByUser);
        }

        public async Task<List<clsUnitOfMeasure>> GetAllUOMAsync()
        {
            return await _productRepo.GetAllUOMAsync();
        }

        public async Task<clsUnitOfMeasure> GetUOMByIdAsync(int id)
        {
            return await _productRepo.GetUOMByIdAsync(id);
        }

        // BALDTO Methods
        public async Task<List<ProductBALDTO>> GetAllProductsBALDTOAsync()
        {
            var products = await _productRepo.GetAllProductsAsync();
            return products.ToProductBALDTOList();
        }

        public async Task<ProductBALDTO> GetProductByIdBALDTOAsync(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            return product?.ToProductBALDTO();
        }

        public async Task<ProductBALDTO> GetByIdBALDTOAsync(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            return product?.ToProductBALDTO();
        }

        public async Task<bool> CreateProductBALDTOAsync(ProductBALDTO productBALDTO, string currentUserId, string uploadPath)
        {
            var product = productBALDTO.ToProductModel();
            product.ActionByUser = currentUserId;
            product.ActionDate = DateTime.Now;
            product.ActionType = 1; // Add
            
            var createdProduct = await _productRepo.CreateProductAsync(product);
            return createdProduct != null;
        }

        public async Task<bool> UpdateProductBALDTOAsync(ProductBALDTO productBALDTO, string currentUserId, string uploadPath)
        {
            var product = productBALDTO.ToProductModel();
            product.ActionByUser = currentUserId;
            product.ActionDate = DateTime.Now;
            product.ActionType = 2; // Update
            
            var updatedProduct = await _productRepo.UpdateProductAsync(product);
            return updatedProduct != null;
        }

        public async Task<List<ProductBALDTO>> SearchProductsBALDTOAsync(string searchTerm)
        {
            var products = await _productRepo.SearchProductsAsync(searchTerm);
            return products.ToProductBALDTOList();
        }

        // دالة مساعدة لمعالجة رفع الصور
        public string HandleImageUpload(IFormFile imageFile, string currentImagePath, string uploadPath)
        {
            // إذا تم رفع صورة جديدة
            if (imageFile != null && imageFile.Length > 0)
            {
                // حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(currentImagePath))
                {
                    clsUtil.DeleteImage(currentImagePath, uploadPath);
                }
                
                // حفظ الصورة الجديدة
                return clsUtil.SaveImage(imageFile, uploadPath);
            }
            
            // إذا لم يتم رفع صورة جديدة، احتفظ بالصورة الحالية
            return currentImagePath;
        }
    }
}
