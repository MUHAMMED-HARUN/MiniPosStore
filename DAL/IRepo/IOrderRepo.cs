using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepo
{
    public interface IOrderRepo
    {
        Task<int> CreateAsync(clsOrder order);
        Task<bool> UpdateAsync(clsOrder order);
        Task<bool> DeleteAsync(int OrderID, string CurentUserID);
        Task<clsOrder> GetByIdAsync(int OrderID);
        Task<List<clsOrder>> GetAllAsync();

        Task<bool> ConfirmOrderAsync(int OrderID, string CurentUserID);
        Task<bool> CancelOrderAsync(int OrderID,string CurentUserID);


        Task<bool> AddItem(clsOrderItem orderItem);
        Task<bool> UpdateItem(clsOrderItem orderItem);
        Task<bool> DeleteItem(int OrderItemID);
        Task<bool> DeleteItemByRange(int[] OrderItemsID);
        Task<List< clsOrderItem>> GetItemsByOrderID(int OrderID);

        // DTO Methods
        Task<OrderDTO> GetByIdDTOAsync(int OrderID);
        Task<List<OrderDTO>> GetAllDTOAsync();
        Task<List<OrderDTO>> GetAllDTOAsync(clsOrderFilter filter);

        // OrderItems DTO Methods
        Task<OrderItemsDTO> GetOrderItemByIdDTOAsync(int OrderItemID);
        Task<List<OrderItemsDTO>> GetOrderItemsByOrderIdDTOAsync(int OrderID);
        
        // Search Methods
        Task<List<clsOrder>> SearchOrdersAsync(string searchTerm);

        Task<List<OrderItemUnionDTO>> GetOrderItemUnionDTOs(clsOrderItemUnionFilter filter);
    }
}
