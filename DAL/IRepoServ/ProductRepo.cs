using DAL.EF.AppDBContext;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using DAL.IRepo;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDBContext _context;

        public ProductRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<clsProduct>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync(clsProductFilter filter)
        {
            string Query = @$"select * from GetProductsFiltered ( {clsDALUtil.GetSqlPrameterString<clsProductFilter>()})";

            using (var connection = _context.Database.GetDbConnection().CreateCommand())
            {
                connection.CommandText = Query;
                connection.CommandType = System.Data.CommandType.Text;

                if (connection.Connection.State != System.Data.ConnectionState.Open)
                    connection.Connection.Open();

                var arr = clsDALUtil.GetSqlPrameters<clsProductFilter>(filter).ToArray();
                connection.Parameters.AddRange(arr);

                List<ProductDTO> products = new List<ProductDTO>();
                try
                {

                using (var reader = connection.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new ProductDTO();
                        clsDALUtil.MapToClass<ProductDTO>(reader, ref product);
                        products.Add(product);
                    }
                }
                }
                catch (SqlException e)
                {

                }
                return products;
            }
        }


        public async Task<clsProduct> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ID == id);

        }

        public async Task<clsProduct> CreateProductAsync(clsProduct product)
        {
            product.ActionDate = DateTime.Now;
            product.ActionType = 1; // Create
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            return await GetProductByIdAsync(product.ID);
        }

        public async Task<clsProduct> UpdateProductAsync(clsProduct product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ID);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.RetailPrice = product.RetailPrice;
            existingProduct.WholesalePrice = product.WholesalePrice;
            existingProduct.AvailableQuantity = product.AvailableQuantity;
            existingProduct.ImagePath = product.ImagePath;
            existingProduct.CurrencyType = product.CurrencyType;
            existingProduct.UOMID = product.UOMID;
            existingProduct.ActionByUser = product.ActionByUser;
            existingProduct.ActionDate = DateTime.Now;
            existingProduct.ActionType = 2; // Update

            await _context.SaveChangesAsync();
            
            return await GetProductByIdAsync(product.ID);
        }

        public async Task<bool> DeleteProductAsync(clsProduct Product, string CurentUserID)
        {
            if (Product == null)
                return false;
          
            _context.Products.Update(Product);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<List<clsProduct>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.User)
                .Where(p => p.Name.Contains(searchTerm) || 
                           p.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> IncreaseProductQuantityAsync(int[] importOrderItemsID, string actionByUser)
        {
            if (importOrderItemsID == null || importOrderItemsID.Length == 0)
                return false;

            // 1?? ��� ����� ��������� ��������
            var importOrderItems = await _context.ImportOrderItems
                .Where(ioi => importOrderItemsID.Contains(ioi.ID))
                .ToListAsync();

            if (!importOrderItems.Any())
                return false;

            // 2?? ������� ������ �������� ��������
            var productIds = importOrderItems.Select(ioi => ioi.ProductID).Distinct().ToList();

            // 3?? ��� �������� �� ����� ��������
            var products = await _context.Products
                .Where(p => productIds.Contains(p.ID))
                .ToListAsync();

            // 4?? ����� �������� ��� Dictionary ����� ������
            var productDict = products.ToDictionary(p => p.ID);

            // 5?? ����� ������� ��� ����
            foreach (var item in importOrderItems)
            {
                if (productDict.TryGetValue(item.ProductID, out var product))
                {
                    product.AvailableQuantity += item.Quantity;
                    product.ActionDate = DateTime.Now;
                    product.ActionByUser = actionByUser;
                    product.ActionType = 2 ; // Quantity Change or UpdateMode
                }
            }

            // 6?? ��� ��������� �� ����� ��������
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DecreaseProductQuantityAsync(int[] orderItemIds, string actionByUser)
        {
            if (orderItemIds == null || orderItemIds.Length == 0)
                return false;

            // 1. اجلب OrderItems مع المنتجات المرتبطة
            List<clsOrderItem> orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => orderItemIds.Contains(oi.ID))
                .ToListAsync();

            if (!orderItems.Any())
                return false;

            // 2. جهز Dictionary للمنتجات لمزيد من السرعة
            List<int> productIds = orderItems.Select(oi => oi.ProductID).Distinct().ToList();
            Dictionary<int,clsProduct>? products = await _context.Products
                .Where(p => productIds.Contains(p.ID))
                .ToDictionaryAsync(p => p.ID);

         
            Dictionary<int,float> requiredQuantities = orderItems
                .GroupBy(oi => oi.ProductID)
                .ToDictionary(g => g.Key, g => g.Sum(oi => oi.Quantity));

           
            bool isAvailable = requiredQuantities.All(rq =>
                products.ContainsKey(rq.Key) && products[rq.Key].AvailableQuantity >= rq.Value);

            if (!isAvailable)
                return false; 

            
            foreach (var rq in requiredQuantities)
            {
                products[rq.Key].AvailableQuantity -= rq.Value;
                products[rq.Key].ActionByUser = actionByUser;
                products[rq.Key].ActionDate = DateTime.Now;
                _context.Products.Update(products[rq.Key]);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<clsProduct> SearchProductByNameBALDTOAsync(string searchTerm)
        {
            return await _context.Products.Where(p => p.Name==searchTerm).Include(p => p.UnitOfMeasure).FirstOrDefaultAsync();
        }
        public async Task<List<clsUnitOfMeasure>> GetAllUOMAsync()
        {
            return await _context.UnitOfMeasures.ToListAsync();
        }

        public async Task<clsUnitOfMeasure> GetUOMByIdAsync(int id)
        {
            return await _context.UnitOfMeasures.FindAsync(id);
        }
    }
}