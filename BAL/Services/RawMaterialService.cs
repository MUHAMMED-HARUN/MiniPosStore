using BAL.Events.OrderEvents;
using BAL.Events.ImportOrderEvents;
using BAL.Interfaces;
using BAL.Mappers;
using DAL.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Configuration;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class RawMaterialService : Interfaces.IRawMaterialService
    {
        private readonly DAL.IRepo.IRawMaterialService _rawMaterialRepo;
        private readonly ICurrentUserService _currentUserServ;
        private readonly IOrderService _orderService;
        private readonly IImportOrderService _importOrderService;
        private readonly IServiceScopeFactory _scopeFactory;
      
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public clsRawMaterial rawMaterial { get; set; }

        public RawMaterialService(DAL.IRepo.IRawMaterialService rawMaterialRepo, ICurrentUserService currentUser,IOrderService orderService, IImportOrderService importOrderService, IServiceScopeFactory scopeFactory)
        {
            _rawMaterialRepo = rawMaterialRepo;
            _currentUserServ = currentUser;
            _orderService = orderService;
            _importOrderService = importOrderService;
            _scopeFactory = scopeFactory;
            _orderService.OrderConfirmedEvent += OnOrderConfirmd;
            // Subscribe to material-specific events if fired from OrderService
            _orderService.MaterialOrderItemAddedEvent += async (s, e) =>
            {
                // reserve on add already handled inside repo add; keep as safety if needed
                await ReserveQuantity(e.OrderItem.RawMaterialID,(float) e.OrderItem.Quantity);
            };
            _orderService.MaterialOrderItemUpdatedEvent += async (s, e) =>
            {
                if (e.OldOrderItem.Quantity > e.NewOrderItem.Quantity)
                {
                     await DeReserveQuantity(e.NewOrderItem.RawMaterialID, (float)(e.OldOrderItem.Quantity - e.NewOrderItem.Quantity));
                }
                else
                {
                     await ReserveQuantity(e.NewOrderItem.RawMaterialID, (float)(e.NewOrderItem.Quantity - e.OldOrderItem.Quantity));
                }
            };
            _orderService.MaterialOrderItemDeletedEvent += async (s, e) =>
            {
                await DeReserveQuantity(e.OrderItem.RawMaterialID, (float)e.OrderItem.Quantity);
            };

            // Removed duplicate subscription that re-queries via OrderService and could cause DbContext concurrent access
            
            // Import Order Events
            _importOrderService.ImportOrderConfirmedEvent += OnImportOrderConfirmed;
            //_importOrderService.ImportOrderItemUnionAddedEvent += OnImportOrderItemUnionAdded;
            //_importOrderService.ImportOrderItemUnionUpdatedEvent += OnImportOrderItemUnionUpdated;
            //_importOrderService.ImportOrderItemUnionDeletedEvent += OnImportOrderItemUnionDeleted;
        }

        public async Task<bool> AddAsync(clsRawMaterial rawMaterial)
        {
            rawMaterial.UserID = _currentUserServ.GetCurrentUserId();
            rawMaterial.ActionDate = DateTime.Now;
            rawMaterial.ActionType = 1; // Add

            bool result = await IsExistByName(rawMaterial.Name);
            if (!result)
                return await _rawMaterialRepo.AddAsync(rawMaterial);
            else
                return false;
        }

        public async Task<bool> UpdateAsync(clsRawMaterial rawMaterial)
        {
            rawMaterial.UserID = _currentUserServ.GetCurrentUserId();
            rawMaterial.ActionDate = DateTime.Now;
            rawMaterial.ActionType = 2; // Update

            bool result = await IsExistByName(rawMaterial.Name,rawMaterial.ID);
            if (!result)
                return await _rawMaterialRepo.UpdateAsync(rawMaterial);
            else
                return false;
            
        }

        public async Task<bool> DeleteAsync(int rawMaterialID)
        {
            return await _rawMaterialRepo.DeleteAsync(rawMaterialID, _currentUserServ.GetCurrentUserId());
        }

        public async Task<clsRawMaterial> GetByIdAsync(int rawMaterialID)
        {
            return await _rawMaterialRepo.GetByIdAsync(rawMaterialID);
        }

        public async Task<List<clsRawMaterial>> GetAllAsync()
        {
            return await _rawMaterialRepo.GetAllAsync();
        }

        // BALDTO Methods
        public async Task<RawMaterialDTO> GetByIdBALDTOAsync(int rawMaterialID)
        {
            return await _rawMaterialRepo.GetByIdDTOAsync(rawMaterialID);
        }

        public async Task<List<RawMaterialDTO>> GetAllBALDTOAsync()
        {
            return await _rawMaterialRepo.GetAllDTOAsync();
        }

        public async Task<List<RawMaterialDTO>> GetAllBALDTOAsync(clsRawMaterialFilter filter)
        {
            return await _rawMaterialRepo.GetAllDTOAsync(filter);
        }

        public async Task<bool> AddBALDTOAsync(RawMaterialDTO rawMaterialDTO)
        {
            var rawMaterial = BALMappers.ToRawMaterialModel(rawMaterialDTO);
            rawMaterial.UserID = _currentUserServ.GetCurrentUserId();
            rawMaterial.ActionDate = DateTime.Now;
            rawMaterial.ActionType = 1; // Add


            bool result = await IsExistByName(rawMaterial.Name);
            if (!result)
                return await _rawMaterialRepo.AddDTOAsync(rawMaterialDTO);
            else
                return false;
  
        }

        public async Task<bool> UpdateBALDTOAsync(RawMaterialDTO rawMaterialDTO)
        {
            var rawMaterial = BALMappers.ToRawMaterialModel(rawMaterialDTO);
            rawMaterial.UserID = _currentUserServ.GetCurrentUserId();
            rawMaterial.ActionDate = DateTime.Now;
            rawMaterial.ActionType = 2; // Update

            bool result = await IsExistByName(rawMaterial.Name,rawMaterial.ID);
            if (!result)
                return await _rawMaterialRepo.AddDTOAsync(rawMaterialDTO);
            else
                return false;

            return await _rawMaterialRepo.UpdateDTOAsync(rawMaterialDTO);
        }

        public async Task<bool> Save()
        {
            return await _rawMaterialRepo.Save();
        }


        public async Task< bool> IsAvalableQuantity(int MaterialID,float Quantity)
        {

            var material =await _rawMaterialRepo.GetByIdAsync(MaterialID);
            if (material != null)
            {
                return material.AvailableQuantity >= Quantity;
            }
            return false;
        }

        public async Task<bool> AddOrderItem(clsRawMaterialOrderItem materialOrderItem)
        {
         bool available=   await IsAvalableQuantity(materialOrderItem.RawMaterialID, (float)materialOrderItem.Quantity);
            if ( !available)
             return false;
            


            bool Result =  await _rawMaterialRepo.AddOrderItem(materialOrderItem);
            //if (Result)
            //{
            //    bool ReservResult = await ReserveQuantity(materialOrderItem.RawMaterialID, (float)materialOrderItem.Quantity);
            //    return ReservResult;
            //}
            return Result;
        }

        public async Task<bool> UpdateOrderItem(clsRawMaterialOrderItem materialOrderItem)
        {
            clsRawMaterialOrderItem oldItem = await GetMaterialItem(materialOrderItem.ID);
            bool Result =     await _rawMaterialRepo.UpdateOrderItem(materialOrderItem);
          
            return Result;
        }

        public async Task<bool> DeleteOrderItem(clsRawMaterialOrderItem materialOrderItem)
        {
               bool Result =  await _rawMaterialRepo.DeleteOrderItem(materialOrderItem);
            if (Result)
            {
              return await  DeReserveQuantity(materialOrderItem.RawMaterialID, (float)materialOrderItem.Quantity);
            }
            return false;
        }

        public async Task<List<clsRawMaterialOrderItem>> GetAllOrderItem()
        {
            return await _rawMaterialRepo.GetAllOrderItem();
        }

        public async Task<List<clsRawMaterialOrderItem>> GetAllOrderItem(int OrderID)
        {
            return await _rawMaterialRepo.GetAllOrderItem(OrderID);
        }

        public async Task<bool> AddImportOrderItem(clsImportRawMaterialItem importRawMaterialItem)
        {
            return await _rawMaterialRepo.AddImportOrderItem(importRawMaterialItem);
        }

        public async Task<bool> UpdateImportOrderItem(clsImportRawMaterialItem importRawMaterialItem)
        {
            return await _rawMaterialRepo.UpdateImportOrderItem(importRawMaterialItem);
        }

        public async Task<bool> DeleteImportOrderItem(clsImportRawMaterialItem importRawMaterialItem)
        {
            return await _rawMaterialRepo.DeleteImportOrderItem(importRawMaterialItem);
        }

        public async Task<List<clsImportRawMaterialItem>> GetAllImportOrderItem()
        {
            return await _rawMaterialRepo.GetAllImportOrderItem();
        }
     public   async Task<List<clsRawMaterialOrderItem>> GetMaterialItemByOrderID(int OrderID)
        {
            return await _rawMaterialRepo.GetMaterialItemByOrderID(OrderID);
        }
        public async Task<List<clsImportRawMaterialItem>> GetAllImportOrderItem(int ImportOrderID)
        {
            return await _rawMaterialRepo.GetAllImportOrderItem(ImportOrderID);
        }

        public async Task<bool> IncreaseByRange(Dictionary<int, float> rawMaterialInfo)
        {
            return await _rawMaterialRepo.IncreaseByRange(rawMaterialInfo);
        }

        public async Task<bool> IncreaseAsync(int rawMaterialID, float Quantity)
        {
            return await _rawMaterialRepo.IncreaseAsync(rawMaterialID, Quantity);
        }

        public async Task<bool> DecreaseAsync(int rawMaterialID, float Quantity)
        {
            return await _rawMaterialRepo.DecreaseAsync(rawMaterialID, Quantity);
        }

        public async Task<bool> ReserveQuantity(int MaterialID, float ReserveedQuantity)
        {
            return await _rawMaterialRepo.ReserveQuantity(MaterialID, ReserveedQuantity);
        }

        public async Task<bool> DeReserveQuantity(int MaterialID, float deReserveedQuantity)
        {
            return await _rawMaterialRepo.DeReserveQuantity(MaterialID, deReserveedQuantity);
        }
        public async Task<bool> DeReserveQuantityByOrderID(int OrderID)
        {
            using var scope = _scopeFactory.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            clsOrderItemUnionFilter unionFilter = new clsOrderItemUnionFilter();
            unionFilter.OrderID = OrderID;
            unionFilter.ItemType = ((int)clsGlobal.enOrderItemType.Material);
            var unionItems = await orderService.GetOrderItemUnionDTOs(unionFilter);

            foreach (var item in unionItems)
            {
                await DeReserveQuantity(item.ItemID, item.Quantity);
                await DecreaseAsync(item.ItemID, item.Quantity);
            }
            return true;
        }

        public Task<clsRawMaterialOrderItem> GetMaterialItem(int OIMaterial)
        {

            return _rawMaterialRepo.GetMaterialItem(OIMaterial);
        }

        public Task<clsImportRawMaterialItem> GetMaterialImportItem(int IOIMaterial)
        {
            return _rawMaterialRepo.GetMaterialImportItem(IOIMaterial);
        }
        async Task DecreaseAsync(List<clsRawMaterialOrderItem> orderItems)
        {
            foreach(clsRawMaterialOrderItem item in orderItems)
            {
              await   DecreaseAsync(item.RawMaterialID, (float)item.Quantity);
            }
        }
        async Task DeReserveQuantity(List<clsRawMaterialOrderItem> orderItems)
        {
            foreach (clsRawMaterialOrderItem item in orderItems)
            {
                await DeReserveQuantity(item.RawMaterialID, (float)item.Quantity);
            }
        }
        protected async Task OnOrderConfirmd(object sender, OrderConfirmedEventArgs e)
        {
            // Use a new scope to avoid concurrent operations on the same DbContext
            using var scope = _scopeFactory.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            clsOrderItemUnionFilter unionFilter = new clsOrderItemUnionFilter();
            unionFilter.OrderID = e.Order.ID;
            unionFilter.ItemType = ((int)clsGlobal.enOrderItemType.Material);
            var unionItems = await orderService.GetOrderItemUnionDTOs(unionFilter);
            foreach (var item in unionItems)
            {
                await DeReserveQuantity(item.ItemID, item.Quantity);
                await DecreaseAsync(item.ItemID, item.Quantity);
            }
        }

        // Method to handle import order confirmed - called externally
        public async Task HandleImportOrderConfirmed(int importOrderID, List<SharedModels.EF.DTO.ImportOrderItemUnionDTO> rawMaterialItems)
        {
            try
            {
                // Increase raw material quantities for imported materials
                foreach (var item in rawMaterialItems)
                {
                    await IncreaseAsync(item.ItemID, item.Quantity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RawMaterialService.HandleImportOrderConfirmed Error: {ex.Message}");
            }
        }

        // Import Order Event Handlers
        protected async Task OnImportOrderConfirmed(object sender, ImportOrderConfirmedEventArgs e)
        {
            try
            {
                // Increase raw material quantities for imported materials
                var rawMaterialItems = e.Items.Where(item => item.ItemType == 2).ToList();
                foreach (var item in rawMaterialItems)
                {
                    await IncreaseAsync(item.ItemID, item.Quantity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RawMaterialService.OnImportOrderConfirmed Error: {ex.Message}");
            }
        }

        // Import Order Union Event Handlers
        protected async Task OnImportOrderItemUnionAdded(object sender, ImportOrderItemUnionAddedEventArgs e)
        {
            try
            {
                // Only handle raw material items (ItemType = 2)
                if (e.ImportOrderItem.ItemType == 2)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<DAL.IRepo.IRawMaterialService>();
                    
                    // Reserve quantity for imported raw material
                    await repo.IncreaseAsync(e.ImportOrderItem.ItemID, e.ImportOrderItem.Quantity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RawMaterialService.OnImportOrderItemUnionAdded Error: {ex.Message}");
            }
        }

        protected async Task OnImportOrderItemUnionUpdated(object sender, ImportOrderItemUnionUpdatedEventArgs e)
        {
            try
            {
                // Only handle raw material items (ItemType = 2)
                if (e.OldImportOrderItem.ItemType == 2 && e.NewImportOrderItem.ItemType == 2)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<DAL.IRepo.IRawMaterialService>();
                    
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
                System.Diagnostics.Debug.WriteLine($"RawMaterialService.OnImportOrderItemUnionUpdated Error: {ex.Message}");
            }
        }

        protected async Task OnImportOrderItemUnionDeleted(object sender, ImportOrderItemUnionDeletedEventArgs e)
        {
            try
            {
                // Only handle raw material items (ItemType = 2)
                if (e.ImportOrderItem.ItemType == 2)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<DAL.IRepo.IRawMaterialService>();
                    
                    await repo.DeReserveQuantity(e.ImportOrderItem.ItemID, e.ImportOrderItem.Quantity);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RawMaterialService.OnImportOrderItemUnionDeleted Error: {ex.Message}");
            }
        }
        public async Task<bool> IsExistByName(string Name, int ID=0)
        {
            return await _rawMaterialRepo.IsExistByName(Name, ID);
        }
    }
}
