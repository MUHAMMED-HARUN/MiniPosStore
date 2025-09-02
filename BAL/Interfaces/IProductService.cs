using DAL.EF.Models;
using BAL.BALDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IProductService
    {
        Task<List<clsProduct>> GetAllProductsAsync();
        Task<clsProduct> GetProductByIdAsync(int id);
        Task<clsProduct> CreateProductAsync(clsProduct product, string currentUserId, string uploadPath);
        Task<clsProduct> UpdateProductAsync(clsProduct product, string currentUserId, string uploadPath);
        Task<bool> DeleteProductAsync(int id, string uploadPath);
        Task<List<clsProduct>> SearchProductsAsync(string searchTerm);
         Task<bool> IncreaseProductQuantityAsync( int[] ImportOrderItmesID, string ActionByUser);
        Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser);


        Task<List<clsUnitOfMeasure>> GetAllUOMAsync();
        Task<clsUnitOfMeasure> GetUOMByIdAsync(int id);
        
        // BALDTO Methods
        Task<List<ProductBALDTO>> GetAllProductsBALDTOAsync();
        Task<ProductBALDTO> GetProductByIdBALDTOAsync(int id);
        Task<ProductBALDTO> GetByIdBALDTOAsync(int id);
        Task<bool> CreateProductBALDTOAsync(ProductBALDTO productBALDTO, string currentUserId, string uploadPath);
        Task<bool> UpdateProductBALDTOAsync(ProductBALDTO productBALDTO, string currentUserId, string uploadPath);
        Task<List<ProductBALDTO>> SearchProductsBALDTOAsync(string searchTerm);
        Task<ProductBALDTO> SearchProductByNameBALDTOAsync(string searchTerm);
    }
}
