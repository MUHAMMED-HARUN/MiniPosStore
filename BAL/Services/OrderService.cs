
using BAL.Events.OrderEvents;
using BAL.Events;
using BAL.Interfaces;
using BAL.Mappers;
using DAL.IRepo;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
	public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICurrentUserService _currentUserServ;
        private readonly IServiceProvider _serviceProvider;
        public clsGlobal.enSaveMode SaveMode { get; set; }
		public virtual clsOrder Order { get; set; }
        public event AsyncEventHandler<OrderConfirmedEventArgs> OrderConfirmedEvent;
        public event AsyncEventHandler<OrderItemAddedEventArgs> OrderItemAddedEvent;
        public event AsyncEventHandler<OrderItemUpdatedEventArgs> OrderItemUpdatedEvent;
        public event AsyncEventHandler<OrderDeletedEventArgs> OrderDeletedEvent;
        public event AsyncEventHandler<OrderItemDeletedEventArgs> OrderItemDeletedEvent;
        public event AsyncEventHandler<MaterialOrderItemAddedEventArgs> MaterialOrderItemAddedEvent;
        public event AsyncEventHandler<MaterialOrderItemUpdatedEventArgs> MaterialOrderItemUpdatedEvent;
        public event AsyncEventHandler<MaterialOrderItemDeletedEventArgs> MaterialOrderItemDeletedEvent;
        public async Task OnOrderConfirmed(clsOrder Order)
        {
            // include order items to avoid consumers needing back-references
            var items = await _orderRepo.GetItemsByOrderID(Order.ID);
            if (OrderConfirmedEvent != null)
                await OrderConfirmedEvent.Invoke(this, new OrderConfirmedEventArgs(Order, items));
        }
        public async Task OnOrderItemAdded(clsOrderItem orderItem)
        {
            if (OrderItemAddedEvent != null)
                await OrderItemAddedEvent.Invoke(this, new OrderItemAddedEventArgs(orderItem));
        }
        public async Task OnOrderItemUpdated(clsOrderItem oldOrderItem, clsOrderItem newOrderItem)
        {
            if (OrderItemUpdatedEvent != null)
                await OrderItemUpdatedEvent.Invoke(this, new OrderItemUpdatedEventArgs(oldOrderItem, newOrderItem));
        }
        public async Task OnOrderDeleted(OrderDTO order)
        {
            var items = await _orderRepo.GetItemsByOrderID(order.ID);
            if (OrderDeletedEvent != null)
                await OrderDeletedEvent.Invoke(this, new OrderDeletedEventArgs(order, items));
        }
        public async Task OnOrderItemDeleted(clsOrderItem orderItem)
        {
            if (OrderItemDeletedEvent != null)
                await OrderItemDeletedEvent.Invoke(this, new OrderItemDeletedEventArgs(orderItem));
        }
        public async Task OnMaterialOrderItemAdded(clsRawMaterialOrderItem orderItem)
        {
            if (MaterialOrderItemAddedEvent != null)
                await MaterialOrderItemAddedEvent.Invoke(this, new MaterialOrderItemAddedEventArgs(orderItem));
        }
        public async Task OnMaterialOrderItemUpdated(clsRawMaterialOrderItem oldOrderItem, clsRawMaterialOrderItem newOrderItem)
        {
            if (MaterialOrderItemUpdatedEvent != null)
                await MaterialOrderItemUpdatedEvent.Invoke(this, new MaterialOrderItemUpdatedEventArgs(oldOrderItem, newOrderItem));
        }
        public async Task OnMaterialOrderItemDeleted(clsRawMaterialOrderItem orderItem)
        {
            if (MaterialOrderItemDeletedEvent != null)
                await MaterialOrderItemDeletedEvent.Invoke(this, new MaterialOrderItemDeletedEventArgs(orderItem));
        }
        public OrderService(IOrderRepo orderRepo,ICurrentUserService currentUser,IServiceProvider provider)
		{
			_orderRepo = orderRepo;
			SaveMode = clsGlobal.enSaveMode.Add;
			_currentUserServ = currentUser;
            _serviceProvider = provider;
            // Ensure ProductService is instantiated so it subscribes to order events
            //_ = _serviceProvider.GetService(typeof(IProductService));
		}

		public async Task<bool> CreateAsync(clsOrder order)
		{
			order.ActionByUser = _currentUserServ.GetCurrentUserId();
            return await _orderRepo.CreateAsync(order)>0;
		}
		
		public async Task<bool> UpdateAsync(clsOrder order)
		{
            order.ActionByUser = _currentUserServ.GetCurrentUserId();
            return await _orderRepo.UpdateAsync(order);
		}

        public async Task<bool> DeleteAsync(int OrderID, string CurentUserID)
		{
			CurentUserID = _currentUserServ.GetCurrentUserId();
            var order = await _orderRepo.GetByIdDTOAsync(OrderID);
            bool Result = await _orderRepo.DeleteAsync(OrderID, CurentUserID);
            if (Result && order != null)
                await OnOrderDeleted(order);
            return Result;
		}

		public async Task<clsOrder> GetByIdAsync(int OrderID)
		{
			return await _orderRepo.GetByIdAsync(OrderID);
		}

		public async Task<List<clsOrder>> GetAllAsync()
		{
			return await _orderRepo.GetAllAsync();
		}

		public async Task<bool> ConfirmOrderAsync(int OrderID)
		{
			return await _orderRepo.ConfirmOrderAsync(OrderID, _currentUserServ.GetCurrentUserId());
		}

		public async Task<bool> CancelOrderAsync(int OrderID)
		{
			return await _orderRepo.CancelOrderAsync(OrderID,_currentUserServ.GetCurrentUserId());
		}
		
        public async Task<bool> AddItem(clsOrderItem orderItem)
		{
            if (IsValidPriceAdjustment(orderItem))
			{
                bool AddResult = await _orderRepo.AddItem(orderItem);
                if (AddResult)
                    await OnOrderItemAdded(orderItem);
                return AddResult;
			}
			else
				return false;
		}
		
        public async Task<bool> UpdateItem(clsOrderItem orderItem)
		{
            if (IsValidPriceAdjustment(orderItem))
			{
                var OldOI = await _orderRepo.GetOrderItemByIdDTOAsync(orderItem.ID);
                bool UpdateResult = await _orderRepo.UpdateItem(orderItem);
                if (UpdateResult && OldOI != null)
                    await OnOrderItemUpdated(OldOI.ToOrderItemModel(), orderItem);
                return UpdateResult;
			}
            
            return false;
        }

        public async Task<bool> DeleteItem(int OrderItemID)
		{
            var OldOrderItemdto =await GetOrderItemByIdBALDTOAsync(OrderItemID);
            if(OldOrderItemdto!=null)
            {
                var OldOrderItem = OldOrderItemdto.ToOrderItemModel();

                  bool Result =   await _orderRepo.DeleteItem(OrderItemID);
                if(Result)
                    await OnOrderItemDeleted(OldOrderItem);
                return Result;
            }
            return false;
        }

		public async Task<bool> DeleteItemByRange(int[] OrderItemsID)
		{
			return await _orderRepo.DeleteItemByRange(OrderItemsID);
		}

		public async Task<List<clsOrderItem>> GetItemsByOrderID(int OrderID)
		{
			return await _orderRepo.GetItemsByOrderID(OrderID);
		}

		// BALDTO Methods
		public async Task<OrderDTO> GetByIdBALDTOAsync(int OrderID)
		{
			clsOrderFilter filter = new clsOrderFilter();
			filter.OrderID = OrderID;
			var order = await _orderRepo.GetAllDTOAsync(filter);

			if (order.FirstOrDefault() != null)
				order.FirstOrDefault().ID = order.FirstOrDefault().OrderID;

			return order.FirstOrDefault();
		}

		public async Task<List<OrderDTO>> GetAllBALDTOAsync()
		{
			var orders = await _orderRepo.GetAllAsync();
			return orders.ToOrderDTOList();
		}
      	public async   Task<List<OrderDTO>> GetAllOrdersDTOAsync(clsOrderFilter Filter)
		{
			List<OrderDTO> dalDTO =await _orderRepo.GetAllDTOAsync(Filter);
			List<OrderDTO> BalOrderDTO = dalDTO.Select(o => new OrderDTO
			{
				ID=o.OrderID,
				CustomerID=o.CustomerID,
				PersonID=o.PersonID,
				FirstName=o.FirstName,
				LastName=o.LastName,
				PhoneNumber=o.PhoneNumber,
				OrderDate=o.OrderDate,
				TotalAmount=o.TotalAmount,
				PaidAmount=o.PaidAmount,
				PaymentStatus=o.PaymentStatus,
				ActionByUser=o.ActionByUser,

			}).ToList();
			return BalOrderDTO;
		}

        public async Task<bool> CreateBALDTOAsync(OrderDTO OrderDTO)
		{
			OrderDTO.ActionByUser=_currentUserServ.GetCurrentUserId();
            var order = OrderDTO.ToOrderModel();
			
			OrderDTO.ID=	await _orderRepo.CreateAsync(order);
            order.ID = 3;
			return OrderDTO.ID > 0;
        }

        // removed product operations; now handled by event subscribers
        public async Task<bool> UpdateBALDTOAsync(OrderDTO OrderDTO)
		{
            byte OldStatus = _orderRepo.GetByIdAsync(OrderDTO.ID).Result.PaymentStatus;
			

			if (IsPaymentCompletedDTO(OrderDTO))
				OrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Completed);
			else
				OrderDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.PendingForPayment);
			

			OrderDTO.ActionByUser = _currentUserServ.GetCurrentUserId();
            var order = OrderDTO.ToOrderModel();

			bool Result = 	await _orderRepo.UpdateAsync(order);

            if ((OldStatus == ((byte)clsGlobal.enPaymentStatus.Pending))&&Result)
            {
                await OnOrderConfirmed(order);
            }
			return Result;
		}

		// OrderItems BALDTO Methods
		public async Task<OrderItemsDTO> GetOrderItemByIdBALDTOAsync(int OrderItemID)
		{
			var orderItem = await _orderRepo.GetOrderItemByIdDTOAsync(OrderItemID);

			return BAL.Mappers.BALMappers.FromDALToOrderItemsDTO( orderItem);
		}

		public async Task<List<OrderItemsDTO>> GetOrderItemsByOrderIdBALDTOAsync(int OrderID)
		{
			var orderItems = await _orderRepo.GetItemsByOrderID(OrderID);
			return orderItems.ToOrderItemsDTOList();
		}
		public bool IsValidPriceAdjustment(OrderItemsDTO orderItems)
		{
			return orderItems.PriceAdjustment>=0&&(orderItems.PriceAdjustment <= orderItems.Quantity * orderItems.SellingPrice);
        }
        public bool IsValidPriceAdjustment(clsOrderItem orderItems)
		{
			return orderItems.PriceAdjustment>=0&&(orderItems.PriceAdjustment <= orderItems.Quantity * orderItems.SellingPrice);
        }

        public async Task<bool> AddItemBALDTOAsync(OrderItemsDTO orderItemBALDTO)
		{

           var _productServ =(IProductService) _serviceProvider.GetService(typeof(IProductService));
            if (_productServ == null)
                return false;

            bool result = await _productServ.HasAvailableQuantity(orderItemBALDTO.ProductID, orderItemBALDTO.Quantity);


            if (IsValidPriceAdjustment(orderItemBALDTO)&&result)
            {

                var orderItem = orderItemBALDTO.ToOrderItemModel();

                bool AddResult = await _orderRepo.AddItem(orderItem);
                if (AddResult)
                {
                    bool ReserveQuantity = await _productServ.ReserveQuantity(orderItem.ProductID, orderItem.Quantity);
                    return ReserveQuantity;
                }
                return false;

            }
            else
                return false;

  

		}

		public async Task<bool> UpdateItemBALDTOAsync(OrderItemsDTO orderItemBALDTO)
		{

            var _productServ = (IProductService)_serviceProvider.GetService(typeof(IProductService));
            if (_productServ == null)
                return false;
            
            bool result = await _productServ.HasAvailableQuantity(orderItemBALDTO.ProductID, orderItemBALDTO.Quantity);

            if (IsValidPriceAdjustment(orderItemBALDTO)&&result)
            {
                var OldOI = await _orderRepo.GetOrderItemByIdDTOAsync(orderItemBALDTO.ID);

                var orderItem = orderItemBALDTO.ToOrderItemModel();
           
                bool UpdateResult = await _orderRepo.UpdateItem(orderItem);
                if (UpdateResult)
                {
                    if (OldOI.Quantity != orderItem.Quantity)
                    {
                        // new 6   old 4
                        // old 6   new 4
                        if (orderItem.Quantity > OldOI.Quantity)
                            return await _productServ.ReserveQuantity(orderItem.ProductID, orderItem.Quantity - OldOI.Quantity);
                        else
                            return await _productServ.DeReserveQuantity(orderItem.ProductID, OldOI.Quantity - orderItem.Quantity);
                    }
                }
            }

            return false;


		}

		public async Task<List<clsOrder>> SearchOrdersAsync(string searchTerm)
		{
			return await _orderRepo.SearchOrdersAsync(searchTerm);
		}

		public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await CreateAsync(Order);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(Order);
            }
        }

        public bool IsPaymentCompletedDTO(OrderDTO order)
        {
            return order.PaidAmount == order.TotalAmount;
        }
        public bool IsPaymentCompleted(clsOrder order)
        {
            return order.PaidAmount == order.TotalAmount;

        }
        public async Task<bool> AddPaymentAsync(int orderID, float paymentAmount)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(orderID);
                if (order == null) return false;

                order.PaidAmount += paymentAmount;
                order.ActionDate = DateTime.Now;
                order.ActionType = ((int)clsGlobal.enActionType.Update); // Update

				if (IsPaymentCompleted(order))
					order.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Completed);
				else
					order.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.PendingForPayment);

                
                return await UpdateAsync(order);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<float> GetRemainingAmountAsync(int orderID)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(orderID);
                if (order == null) return 0;
                return order.TotalAmount - order.PaidAmount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> IsFullyPaidAsync(int orderID)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(orderID);
                if (order == null) return false;
                return order.PaidAmount >= order.TotalAmount;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<OrderItemUnionDTO>> GetOrderItemUnionDTOs(clsOrderItemUnionFilter filter)
        {
            return await _orderRepo.GetOrderItemUnionDTOs(filter);
        }

        // Material order item operations moved under OrderService and surfaced via events
        public async Task<bool> AddMaterialOrderItem(clsRawMaterialOrderItem item)
        {
            // Persist via order repo material methods if exist; otherwise reuse RawMaterialRepo through service provider
            var rawServ = (BAL.Interfaces.IRawMaterialService)_serviceProvider.GetService(typeof(BAL.Interfaces.IRawMaterialService));
            if (rawServ == null) return false;
            bool available = await rawServ.IsAvalableQuantity(item.RawMaterialID,(float) item.Quantity);
            if (!available) return false;
            // reuse repo in raw service to add
            bool ok = await rawServ.AddOrderItem(item);
            if (ok) OnMaterialOrderItemAdded(item);
            return ok;
        }

        public async Task<bool> UpdateMaterialOrderItem(clsRawMaterialOrderItem item)
        {
            var rawServ = (BAL.Interfaces.IRawMaterialService)_serviceProvider.GetService(typeof(BAL.Interfaces.IRawMaterialService));
            if (rawServ == null) return false;

            clsOrderItemUnionFilter unionFilter = new clsOrderItemUnionFilter();
          unionFilter.OrderItemID = item.ID;
            unionFilter.ItemType=((int)clsGlobal.enOrderItemType.Material);

         var materialunion = (await GetOrderItemUnionDTOs(unionFilter)).FirstOrDefault();
            if(materialunion == null) return false;

            var oldItem = materialunion.ToRawMaterialOrderItemModel();
            bool ok = await rawServ.UpdateOrderItem(item);

            if (ok && oldItem != null) OnMaterialOrderItemUpdated(oldItem, item);
            return ok;
        }

        public async Task<bool> DeleteMaterialOrderItem(int orderMaterialItemId)
        {
            var rawServ = (BAL.Interfaces.IRawMaterialService)_serviceProvider.GetService(typeof(BAL.Interfaces.IRawMaterialService));

            if (rawServ == null) return false;
            var oldItem = await rawServ.GetMaterialItem(orderMaterialItemId);
            if (oldItem == null) return false;
            bool ok = await rawServ.DeleteOrderItem(oldItem);
            if (ok) OnMaterialOrderItemDeleted(oldItem);
            return ok;
        }

    }
}
