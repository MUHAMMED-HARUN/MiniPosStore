
using BAL.Events.OrderEvents;
using BAL.Events.ImportOrderEvents;
using BAL.Interfaces;
using BAL.Mappers;
using DAL.IRepo;
using Microsoft.AspNetCore.Http;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using SharedModels.EF.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly ICurrentUserService _currentUserServ;
        private readonly IServiceScopeFactory _scopeFactory;
        public ProductService(IProductRepo productRepo,ICurrentUserService currentUser,IOrderService orderService, IImportOrderService importOrderService, IServiceScopeFactory scopeFactory)
        {
            _productRepo = productRepo;
            _currentUserServ = currentUser;
            _scopeFactory = scopeFactory;
            
            // Order Events
            orderService.OrderConfirmedEvent += OnOrderConfirmd;
            orderService.OrderItemAddedEvent += OnOrderItemAdded;
            orderService.OrderItemUpdatedEvent += OnOrderItemUpdated;
            orderService.OrderDeletedEvent += OnOrderDeleted;
            orderService.OrderItemDeletedEvent += OnOrderItemDeleted;
            
            // Import Order Events
            importOrderService.ImportOrderConfirmedEvent += OnImportOrderConfirmed;
            importOrderService.ImportOrderItemUnionAddedEvent += OnImportOrderItemUnionAdded;
            importOrderService.ImportOrderItemUnionUpdatedEvent += OnImportOrderItemUnionUpdated;
            importOrderService.ImportOrderItemUnionDeletedEvent += OnImportOrderItemUnionDeleted;
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

            bool result =await IsExistProductByName(product.Name);
            if (!result) 
            {
                var createdProduct = await _productRepo.CreateProductAsync(product);
                return createdProduct;
            }
            return null;
        }

        public async Task<clsProduct> UpdateProductAsync(clsProduct product, string currentUserId, string uploadPath)
        {
            product.ActionByUser = _currentUserServ.GetCurrentUserId(); 
            product.ActionDate = DateTime.Now;
            product.ActionType = 2; // Update

            bool result = await IsExistProductByName(product.Name, product.ID);
            if (!result)
            {
                var updatedProduct = await _productRepo.UpdateProductAsync(product);
                return updatedProduct;
            }
            return null;
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
      public async  Task<bool> IncreaseProductQuantityAsync(int ProductID, float Quantity, string ActionByUser)
        {
           return await _productRepo.IncreaseProductQuantityAsync(ProductID, Quantity, ActionByUser);
        }
        public async Task<bool> DecreaseProductQuantityAsync(int[] OrderItemsID, string actionByUser)
        {
            return await _productRepo.DecreaseProductQuantityAsync(OrderItemsID, _currentUserServ.GetCurrentUserId());
        }
       public async  Task<bool> HasAvailableQuantity(int ProductID, float Quantity)
        {
            return await _productRepo.HasAvailableQuantity(ProductID, Quantity);
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

            bool result = await IsExistProductByName(product.Name);
            if (!result)
            {
                var createdProduct = await _productRepo.CreateProductAsync(product);
            return createdProduct != null;
            }
            return false;
        }

        public async Task<bool> UpdateProductDTOAsync(ProductDTO ProductDTO, string currentUserId, string uploadPath)
        {
            var product = ProductDTO.ToProductModel();
            product.ActionByUser = _currentUserServ.GetCurrentUserId(); ;
            product.ActionDate = DateTime.Now;
            product.ActionType = 2; // Update

            bool result = await IsExistProductByName(product.Name, product.ID);
            if (!result)
            {

                var updatedProduct = await _productRepo.UpdateProductAsync(product);
                return updatedProduct != null;
            }
            return false;
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

        public async Task<double> GetNetProfit(clsNetProfit_SP profit_SP)
        {
         return  await _productRepo.GetNetProfitAsync(profit_SP);
        }
        public async Task<double> GetTotalStockValueAsync(clsTotalStockValue_SP StockVal)
        {
            return await _productRepo.GetTotalStockValueAsync(StockVal);
        }
        public async Task<bool> ReserveQuantity(int ProductID, float ReserveedQuantity)
        {
            return await _productRepo.ReserveQuantity(ProductID, ReserveedQuantity);
        }
       public async  Task<bool> DeReserveQuantity(int ProductID, float deReserveedQuantity)
        {
            return await _productRepo.DeReserveQuantity(ProductID, deReserveedQuantity);
        }


        public async Task<bool> DeReserveQuantity(List<clsOrderItem> OrderItems)
        {
            
        
            foreach (var Item in OrderItems)
            {
              await  DeReserveQuantity(Item.ProductID, Item.Quantity);
            }
            return true;
        }

        protected async Task OnOrderConfirmd(object sender ,OrderConfirmedEventArgs e)
        {
            

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
            // de-reserve each item
            foreach (var item in e.OrderItems)
            {
                await repo.DeReserveQuantity(item.ProductID, item.Quantity);
            }
            var oiArr = e.OrderItems.Select(oi => oi.ID).ToArray();
            if(oiArr.Length>0)
            await repo.DecreaseProductQuantityAsync(oiArr, _currentUserServ.GetCurrentUserId());
        }

        // Method to handle import order confirmed - called externally
        public async Task HandleImportOrderConfirmed(int importOrderID, List<SharedModels.EF.DTO.ImportOrderItemUnionDTO> productItems)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                
                // Increase product quantities for imported products
                if (productItems.Any())
                {
                    var productIds = productItems.Select(item => item.ItemID).ToArray();
                    
                    // Increase product quantities
                    await repo.IncreaseProductQuantityAsync(productIds, _currentUserServ.GetCurrentUserId());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductService.HandleImportOrderConfirmed Error: {ex.Message}");
            }
        }

        protected async Task OnOrderItemAdded(object sender, OrderItemAddedEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
            await repo.ReserveQuantity(e.OrderItem.ProductID, e.OrderItem.Quantity);
        }

        protected async Task OnOrderItemUpdated(object sender, OrderItemUpdatedEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
            if (e.NewOrderItem.Quantity > e.OldOrderItem.Quantity)
            {
                await repo.ReserveQuantity(e.NewOrderItem.ProductID, e.NewOrderItem.Quantity - e.OldOrderItem.Quantity);
            }
            else if (e.NewOrderItem.Quantity < e.OldOrderItem.Quantity)
            {
                await repo.DeReserveQuantity(e.NewOrderItem.ProductID, e.OldOrderItem.Quantity - e.NewOrderItem.Quantity);
            }
        }

        protected async Task OnOrderDeleted(object sender, OrderDeletedEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
            foreach (var item in e.OrderItems)
            {
                await repo.DeReserveQuantity(item.ProductID, item.Quantity);
            }
        }

        protected async Task OnOrderItemDeleted(object sender, OrderItemDeletedEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
            await repo.DeReserveQuantity(e.OrderItem.ProductID, e.OrderItem.Quantity);
        }

        // Import Order Event Handlers
        protected async Task OnImportOrderConfirmed(object sender, ImportOrderConfirmedEventArgs e)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                
                // Increase product quantities for imported products
                var productItems = e.Items.Where(item => item.ItemType == 1).ToList();
                if (productItems.Any())
                {
                    var productIds = productItems.Select(item => item.ItemID).ToArray();
                    var quantities = productItems.ToDictionary(item => item.ItemID, item => item.Quantity);
                    
                    // Increase product quantities
                    foreach (var productId in productIds)
                    {
                        await repo.IncreaseProductQuantityAsync(productId, quantities[productId], _currentUserServ.GetCurrentUserId());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductService.OnImportOrderConfirmed Error: {ex.Message}");
            }
        }

        // Import Order Union Event Handlers
        protected async Task OnImportOrderItemUnionAdded(object sender, ImportOrderItemUnionAddedEventArgs e)
        {
            try
            {
                // Only handle product items (ItemType = 1)
                if (e.ImportOrderItem.ItemType == 1)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                    
                    // Reserve quantity for imported product
                    await repo.ReserveQuantity(e.ImportOrderItem.ItemID, e.ImportOrderItem.Quantity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductService.OnImportOrderItemUnionAdded Error: {ex.Message}");
            }
        }

        protected async Task OnImportOrderItemUnionUpdated(object sender, ImportOrderItemUnionUpdatedEventArgs e)
        {
            try
            {
                // Only handle product items (ItemType = 1)
                if (e.OldImportOrderItem.ItemType == 1 && e.NewImportOrderItem.ItemType == 1)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                    
                    if (e.NewImportOrderItem.Quantity > e.OldImportOrderItem.Quantity)
                    {
                        await repo.ReserveQuantity(e.NewImportOrderItem.ItemID, e.NewImportOrderItem.Quantity - e.OldImportOrderItem.Quantity);
                    }
                    else if (e.NewImportOrderItem.Quantity < e.OldImportOrderItem.Quantity)
                    {
                        await repo.DeReserveQuantity(e.NewImportOrderItem.ItemID, e.OldImportOrderItem.Quantity - e.NewImportOrderItem.Quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductService.OnImportOrderItemUnionUpdated Error: {ex.Message}");
            }
        }

        protected async Task OnImportOrderItemUnionDeleted(object sender, ImportOrderItemUnionDeletedEventArgs e)
        {
            try
            {
                // Only handle product items (ItemType = 1)
                if (e.ImportOrderItem.ItemType == 1)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                    
                    await repo.DeReserveQuantity(e.ImportOrderItem.ItemID, e.ImportOrderItem.Quantity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductService.OnImportOrderItemUnionDeleted Error: {ex.Message}");
            }
        }
        public async Task<bool> IsExistProductByName(string ProductName, int ProductID = 0)
        {
            return await _productRepo.IsExistProductByName(ProductName, ProductID);
        }
    }
}
