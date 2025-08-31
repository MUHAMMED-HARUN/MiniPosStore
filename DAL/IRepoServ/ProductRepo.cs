using DAL.EF.AppDBContext;
using DAL.EF.Models;
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

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
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

            // 1?? ÌáÈ ÚäÇÕÑ ÇáÇÓÊíÑÇÏ ÇáãÑÊÈØÉ
            var importOrderItems = await _context.ImportOrderItems
                .Where(ioi => importOrderItemsID.Contains(ioi.ID))
                .ToListAsync();

            if (!importOrderItems.Any())
                return false;

            // 2?? ÇÓÊÎÑÇÌ ãÚÑÝÇÊ ÇáãäÊÌÇÊ ÇáãÑÊÈØÉ
            var productIds = importOrderItems.Select(ioi => ioi.ProductID).Distinct().ToList();

            // 3?? ÌáÈ ÇáãäÊÌÇÊ ãä ÞÇÚÏÉ ÇáÈíÇäÇÊ
            var products = await _context.Products
                .Where(p => productIds.Contains(p.ID))
                .ToListAsync();

            // 4?? ÊÍæíá ÇáãäÊÌÇÊ Åáì Dictionary ááÈÍË ÇáÓÑíÚ
            var productDict = products.ToDictionary(p => p.ID);

            // 5?? ÊÍÏíË ÇáßãíÇÊ áßá ãäÊÌ
            foreach (var item in importOrderItems)
            {
                if (productDict.TryGetValue(item.ProductID, out var product))
                {
                    product.AvailableQuantity += item.Quantity;
                    product.ActionDate = DateTime.Now;
                    product.ActionByUser = actionByUser;
                    product.ActionType = 3; // Quantity Change
                }
            }

            // 6?? ÍÝÙ ÇáÊÛííÑÇÊ Ýí ÞÇÚÏÉ ÇáÈíÇäÇÊ
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser)
        {
            if (OrderItemsID == null || OrderItemsID.Length == 0)
                return false;

            // 1?? ÌáÈ ÚäÇÕÑ ÇáÇÓÊíÑÇÏ ÇáãÑÊÈØÉ
            var OrderItems =  _context.OrderItems
                .Where(oi => OrderItemsID.Contains(oi.ID))
                .ToList();

            if (!OrderItems.Any())
                return false;

            // 2?? ÇÓÊÎÑÇÌ ãÚÑÝÇÊ ÇáãäÊÌÇÊ ÇáãÑÊÈØÉ
            var productIds = OrderItems.Select(ioi => ioi.ProductID).Distinct().ToList();
            var products = new List<clsProduct>();
            try
            {
                products =  _context.Products
                    .Where(p => productIds.Contains(p.ID))
                    .ToList();
            }
            catch (SqlException e)
            {
                return false; // Ãæ ÇáÊÚÇãá ãÚ ÇáÎØÃ ÍÓÈ ÇáÍÇÌÉ
            }
            catch (Exception e)
            {
                return false;
            }


            // 4?? ÊÍæíá ÇáãäÊÌÇÊ Åáì Dictionary ááÈÍË ÇáÓÑíÚ
            var productDict = products.ToDictionary(p => p.ID);

            // 5?? ÊÍÏíË ÇáßãíÇÊ áßá ãäÊÌ
            foreach (var item in OrderItems)
            {
                if (productDict.TryGetValue(item.ProductID, out var product))
                {
                    product.AvailableQuantity -= item.Quantity;
                    product.ActionDate = DateTime.Now;
                    product.ActionByUser = actionByUser;
                    product.ActionType = 3; // Quantity Change
                }
            }

            // 6?? ÍÝÙ ÇáÊÛííÑÇÊ Ýí ÞÇÚÏÉ ÇáÈíÇäÇÊ
            return await _context.SaveChangesAsync() > 0;
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