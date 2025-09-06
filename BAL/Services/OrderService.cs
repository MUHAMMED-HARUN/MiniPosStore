using BAL.BALDTO;
using BAL.Interfaces;
using BAL.Mappers;
using DAL.EF.DTO;
using DAL.EF.Filters;
using DAL.EF.Models;
using DAL.IRepo;
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
        public clsGlobal.enSaveMode SaveMode { get; set; }
		public virtual clsOrder Order { get; set; }

		public OrderService(IOrderRepo orderRepo,ICurrentUserService currentUser)
		{
			_orderRepo = orderRepo;
			SaveMode = clsGlobal.enSaveMode.Add;
			_currentUserServ = currentUser;
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
            return await _orderRepo.DeleteAsync(OrderID, CurentUserID);
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
		
			return await _orderRepo.AddItem(orderItem);
		}

		public async Task<bool> UpdateItem(clsOrderItem orderItem)
		{
			return await _orderRepo.UpdateItem(orderItem);
		}

		public async Task<bool> DeleteItem(int OrderItemID)
		{
			return await _orderRepo.DeleteItem(OrderItemID);
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
		public async Task<OrderBALDTO> GetByIdBALDTOAsync(int OrderID)
		{
			var order = await _orderRepo.GetByIdAsync(OrderID);
			return order?.ToOrderBALDTO();
		}

		public async Task<List<OrderBALDTO>> GetAllBALDTOAsync()
		{
			var orders = await _orderRepo.GetAllAsync();
			return orders.ToOrderBALDTOList();
		}
      	public async   Task<List<OrderBALDTO>> GetAllOrdersDTOAsync(clsOrderFilter Filter)
		{
			List<OrderDTO> dalDTO =await _orderRepo.GetAllDTOAsync(Filter);
			List<OrderBALDTO> BalOrderDTO = dalDTO.Select(o => new OrderBALDTO
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

        public async Task<bool> CreateBALDTOAsync(OrderBALDTO orderBALDTO)
		{
			orderBALDTO.ActionByUser=_currentUserServ.GetCurrentUserId();
            var order = orderBALDTO.ToOrderModel();
			
			orderBALDTO.ID=	await _orderRepo.CreateAsync(order);
			return orderBALDTO.ID > 0;
        }


        public async Task<bool> UpdateBALDTOAsync(OrderBALDTO orderBALDTO)
		{
			if (IsPaymentCompletedDTO(orderBALDTO))
				orderBALDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.Completed);
			else
				orderBALDTO.PaymentStatus = ((byte)clsGlobal.enPaymentStatus.PendingForPayment);


			orderBALDTO.ActionByUser = _currentUserServ.GetCurrentUserId();
            var order = orderBALDTO.ToOrderModel();
			return await _orderRepo.UpdateAsync(order);
		}

		// OrderItems BALDTO Methods
		public async Task<OrderItemsBALDTO> GetOrderItemByIdBALDTOAsync(int OrderItemID)
		{
			var orderItem = await _orderRepo.GetOrderItemByIdDTOAsync(OrderItemID);

			return BAL.Mappers.BALMappers.FromDALToOrderItemsBALDTO( orderItem);
		}

		public async Task<List<OrderItemsBALDTO>> GetOrderItemsByOrderIdBALDTOAsync(int OrderID)
		{
			var orderItems = await _orderRepo.GetItemsByOrderID(OrderID);
			return orderItems.ToOrderItemsBALDTOList();
		}

		public async Task<bool> AddItemBALDTOAsync(OrderItemsBALDTO orderItemBALDTO)
		{
			var orderItem = orderItemBALDTO.ToOrderItemModel();
			return await _orderRepo.AddItem(orderItem);
		}

		public async Task<bool> UpdateItemBALDTOAsync(OrderItemsBALDTO orderItemBALDTO)
		{
			var orderItem = orderItemBALDTO.ToOrderItemModel();
			return await _orderRepo.UpdateItem(orderItem);
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

        public bool IsPaymentCompletedDTO(OrderBALDTO order)
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
    }
}
