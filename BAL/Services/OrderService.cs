using BAL.Interfaces;
using BAL.BALDTO;
using BAL.Mappers;
using DAL.IRepo;
using DAL.EF.Models;
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
		
		public clsGlobal.enSaveMode SaveMode { get; set; }
		public virtual clsOrder Order { get; set; }

		public OrderService(IOrderRepo orderRepo)
		{
			_orderRepo = orderRepo;
			SaveMode = clsGlobal.enSaveMode.Add;
		}

		public async Task<bool> CreateAsync(clsOrder order)
		{
			return await _orderRepo.CreateAsync(order)>0;
		}

		public async Task<bool> UpdateAsync(clsOrder order)
		{
			return await _orderRepo.UpdateAsync(order);
		}

		public async Task<bool> DeleteAsync(int OrderID, string CurentUserID)
		{
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
			return await _orderRepo.ConfirmOrderAsync(OrderID);
		}

		public async Task<bool> CancelOrderAsync(int OrderID)
		{
			return await _orderRepo.CancelOrderAsync(OrderID);
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

		public async Task<bool> CreateBALDTOAsync(OrderBALDTO orderBALDTO)
		{
			var order = orderBALDTO.ToOrderModel();
			orderBALDTO.ID=	await _orderRepo.CreateAsync(order);
			return orderBALDTO.ID > 0;
        }

		public async Task<bool> UpdateBALDTOAsync(OrderBALDTO orderBALDTO)
		{
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
	}
}
