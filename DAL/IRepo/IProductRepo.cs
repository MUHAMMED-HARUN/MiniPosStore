using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using SharedModels.EF.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IProductRepo
    {
        Task<List<clsProduct>> GetAllProductsAsync();
        Task<List<ProductDTO>> GetAllProductsAsync(clsProductFilter filter);
        Task<clsProduct> GetProductByIdAsync(int id);
        Task<clsProduct> CreateProductAsync(clsProduct product);
        Task<clsProduct> UpdateProductAsync(clsProduct product);
        Task<bool> DeleteProductAsync(clsProduct Product,string CurentUserID);
        Task<List<clsProduct>> SearchProductsAsync(string searchTerm);
          Task<bool> IncreaseProductQuantityAsync( int[] ImportOrderItmesID, string ActionByUser);
          Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser);
        Task<clsProduct> SearchProductByNameBALDTOAsync(string searchTerm);
        Task<List<clsUnitOfMeasure>> GetAllUOMAsync();
        Task<clsUnitOfMeasure> GetUOMByIdAsync(int id);
        Task<double> GetNetProfitAsync(clsNetProfit_SP profit_SP);
    }
}
