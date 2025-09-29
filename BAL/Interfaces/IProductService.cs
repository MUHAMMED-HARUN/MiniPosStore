using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using SharedModels.EF.SP;
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
        Task<List<ProductDTO>> GetAllProductsAsync(clsProductFilter Filter);
        Task<clsProduct> GetProductByIdAsync(int id);
        Task<clsProduct> CreateProductAsync(clsProduct product, string currentUserId, string uploadPath);
        Task<clsProduct> UpdateProductAsync(clsProduct product, string currentUserId, string uploadPath);
        Task<bool> DeleteProductAsync(int id, string uploadPath);
        Task<List<clsProduct>> SearchProductsAsync(string searchTerm);
         Task<bool> IncreaseProductQuantityAsync( int[] ImportOrderItmesID, string ActionByUser);
        Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser);

        Task<bool> HasAvailableQuantity(int ProductID, float Quantity);
        Task<List<clsUnitOfMeasure>> GetAllUOMAsync();
        Task<clsUnitOfMeasure> GetUOMByIdAsync(int id);
        
        // BALDTO Methods
        Task<List<ProductDTO>> GetAllProductsBALDTOAsync();
        Task<ProductDTO> GetProductByIdBALDTOAsync(int id);
        Task<ProductDTO> GetByIdBALDTOAsync(int id);
        Task<bool> CreateProductDTOAsync(ProductDTO ProductDTO, string currentUserId, string uploadPath);
        Task<bool> UpdateProductDTOAsync(ProductDTO ProductDTO, string currentUserId, string uploadPath);
        Task<List<ProductDTO>> SearchProductsBALDTOAsync(string searchTerm);
        Task<ProductDTO> SearchProductByNameBALDTOAsync(string searchTerm);
        Task<double> GetNetProfit(clsNetProfit_SP profit_SP);
        Task<double> GetTotalStockValueAsync(clsTotalStockValue_SP StockVal);
    }
}
