
using BAL.Interfaces;
using BAL.Mappers;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using DAL.IRepo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly ICurrentUserService _currentUserServ;
        public ProductService(IProductRepo productRepo,ICurrentUserService currentUser)
        {
            _productRepo = productRepo;
            _currentUserServ = currentUser;
        }

        public async Task<List<clsProduct>> GetAllProductsAsync()
        {
            return await _productRepo.GetAllProductsAsync();
        }
        public async Task<List<ProductDTO>> GetAllProductsAsync(clsProductFilter Filter)
        {
            List<ProductDTO> productsDAL = await _productRepo.GetAllProductsAsync(Filter);
         
          return productsDAL;
        }
        public async Task<clsProduct> GetProductByIdAsync(int id)
        {
            return await _productRepo.GetProductByIdAsync(id);
        }

        public async Task<clsProduct> CreateProductAsync(clsProduct product, string currentUserId, string uploadPath)
        {
            product.ActionByUser = _currentUserServ.GetCurrentUserId();
            product.ActionDate = DateTime.Now;
            product.ActionType = 1; // Add
            
            var createdProduct = await _productRepo.CreateProductAsync(product);
            return createdProduct;
        }

        public async Task<clsProduct> UpdateProductAsync(clsProduct product, string currentUserId, string uploadPath)
        {
            product.ActionByUser = _currentUserServ.GetCurrentUserId(); 
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
            product.ActionType = ((byte)clsGlobal.enActionType.Delete);
            return await _productRepo.DeleteProductAsync(product,_currentUserServ.GetCurrentUserId());
        }

        public async Task<List<clsProduct>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepo.SearchProductsAsync(searchTerm);
        }
        public async Task<bool> IncreaseProductQuantityAsync( int[] ImportOrderItmesID, string ActionByUser)
        {

            return await _productRepo.IncreaseProductQuantityAsync(ImportOrderItmesID, _currentUserServ.GetCurrentUserId());
        }
        public async Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser)
        {
            return await _productRepo.DecreaseProductQuantityAsync(OrderItemsID, _currentUserServ.GetCurrentUserId());
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
        public async Task<List<ProductDTO>> GetAllProductsBALDTOAsync()
        {
            clsProductFilter filter = new clsProductFilter();
            var products = await _productRepo.GetAllProductsAsync(filter);
            return products;
        }

        public async Task<ProductDTO> GetProductByIdBALDTOAsync(int id)
        {
            clsProductFilter filter = new clsProductFilter();
            filter.Id = id;

            var product = await _productRepo.GetAllProductsAsync(filter);
            return product.FirstOrDefault();
        }

        public async Task<ProductDTO> GetByIdBALDTOAsync(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            return product?.ToProductDTO();
        }

        public async Task<bool> CreateProductDTOAsync(ProductDTO ProductDTO, string currentUserId, string uploadPath)
        {
            var product = ProductDTO.ToProductModel();
            product.ActionByUser = _currentUserServ.GetCurrentUserId(); 
            product.ActionDate = DateTime.Now;
            product.ActionType = 1; // Add
            
            var createdProduct = await _productRepo.CreateProductAsync(product);
            return createdProduct != null;
        }

        public async Task<bool> UpdateProductDTOAsync(ProductDTO ProductDTO, string currentUserId, string uploadPath)
        {
            var product = ProductDTO.ToProductModel();
            product.ActionByUser = _currentUserServ.GetCurrentUserId(); ;
            product.ActionDate = DateTime.Now;
            product.ActionType = 2; // Update
            
            var updatedProduct = await _productRepo.UpdateProductAsync(product);
            return updatedProduct != null;
        }

        public async Task<List<ProductDTO>> SearchProductsBALDTOAsync(string searchTerm)
        {
            var products = await _productRepo.SearchProductsAsync(searchTerm);
            return products.ToProductDTOList();
        }
     public async   Task<ProductDTO> SearchProductByNameBALDTOAsync(string searchTerm)
        {
           var product =await _productRepo.SearchProductByNameBALDTOAsync(searchTerm);
            return product.ToProductDTO();
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
