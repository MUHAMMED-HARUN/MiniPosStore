using DAL.EF.Models;
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
        Task<clsProduct> GetProductByIdAsync(int id);
        Task<clsProduct> CreateProductAsync(clsProduct product);
        Task<clsProduct> UpdateProductAsync(clsProduct product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<clsProduct>> SearchProductsAsync(string searchTerm);
          Task<bool> IncreaseProductQuantityAsync( int[] ImportOrderItmesID, string ActionByUser);
          Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser);

        Task<List<clsUnitOfMeasure>> GetAllUOMAsync();
        Task<clsUnitOfMeasure> GetUOMByIdAsync(int id);
    }
}
