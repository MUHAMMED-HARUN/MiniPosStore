using BAL;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IOrderService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsOrder Order { get; set; }
        
        Task<bool> CreateAsync(clsOrder order);
        Task<bool> UpdateAsync(clsOrder order);
        Task<bool> DeleteAsync(int OrderID, string CurentUserID);
        Task<clsOrder> GetByIdAsync(int OrderID);
        Task<List<clsOrder>> GetAllAsync();
        Task<bool> ConfirmOrderAsync(int OrderID);
        Task<bool> CancelOrderAsync(int OrderID);
        
        Task<bool> AddItem(clsOrderItem orderItem);
        Task<bool> UpdateItem(clsOrderItem orderItem);
        Task<bool> DeleteItem(int OrderItemID);
        Task<bool> DeleteItemByRange(int[] OrderItemsID);
        Task<List<clsOrderItem>> GetItemsByOrderID(int OrderID);
        
        // BALDTO Methods
        Task<OrderDTO> GetByIdBALDTOAsync(int OrderID);
        Task<List<OrderDTO>> GetAllBALDTOAsync();
        Task<List<OrderDTO>> GetAllOrdersDTOAsync(clsOrderFilter Filter);
        Task<bool> CreateBALDTOAsync(OrderDTO OrderDTO);
        Task<bool> UpdateBALDTOAsync(OrderDTO OrderDTO);
        
        // OrderItems BALDTO Methods
        Task<OrderItemsDTO> GetOrderItemByIdBALDTOAsync(int OrderItemID);
        Task<List<OrderItemsDTO>> GetOrderItemsByOrderIdBALDTOAsync(int OrderID);
        Task<bool> AddItemBALDTOAsync(OrderItemsDTO orderItemBALDTO);
        Task<bool> UpdateItemBALDTOAsync(OrderItemsDTO orderItemBALDTO);
        
        // Search Methods
        Task<List<clsOrder>> SearchOrdersAsync(string searchTerm);
        
        // Payment Methods
        Task<bool> AddPaymentAsync(int orderID, float paymentAmount);
        Task<float> GetRemainingAmountAsync(int orderID);
        Task<bool> IsFullyPaidAsync(int orderID);
        
        Task<bool> Save();
    }
}
