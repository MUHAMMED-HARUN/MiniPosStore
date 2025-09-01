using DAL.EF.AppDBContext;
using DAL.EF.Models;
using DAL.EF.DTO;
using DAL.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDBContext _context;

        public OrderRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(clsOrder order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsOrder order)
        {
            try
            {
                var existingOrder = await _context.Orders.FindAsync(order.ID);
                if (existingOrder == null)
                    return false;

                existingOrder.CustomerID = order.CustomerID;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.TotalAmount = order.TotalAmount;
                existingOrder.PaidAmount = order.PaidAmount;
                existingOrder.PaymentStatus = order.PaymentStatus;
                existingOrder.ActionByUser = order.ActionByUser;
                existingOrder.ActionType = order.ActionType;
                existingOrder.ActionDate = order.ActionDate;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int orderId, string CurentUserID)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    return false;

                await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_set_session_context 'UserID', {0}",
                CurentUserID
);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsOrder> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .ThenInclude(c => c.Person)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.ID == orderId);
        }

        public async Task<List<clsOrder>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .ThenInclude(c => c.Person)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }


        public async Task<int> CreateAsync(clsOrder order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order.ID; 
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> ConfirmOrderAsync(int OrderID,string CurentUserID)
        {
            try
            {
                var order = await _context.Orders.FindAsync(OrderID);
                if (order == null)
                    return false;

                // يمكنك إضافة منطق تأكيد الطلب هنا
                // مثلاً: تغيير حالة الطلب إلى مؤكد
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelOrderAsync(int OrderID,string CurentUserID)
        {
            try
            {
                var order = await _context.Orders.FindAsync(OrderID);
                if (order == null)
                    return false;

                // يمكنك إضافة منطق إلغاء الطلب هنا
                // مثلاً: تغيير حالة الطلب إلى ملغي
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> AddItem(clsOrderItem orderItem)
        {
            try
            {
                await _context.OrderItems.AddAsync(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateItem(clsOrderItem orderItem)
        {
            try
            {
                _context.OrderItems.Update(orderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(int OrderItemID)
        {
            try
            {
                var item = await _context.OrderItems.FindAsync(OrderItemID);
                if (item == null)
                    return false;
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemByRange(int[] OrderItemsID)
        {
            try
            {
                var items = await _context.OrderItems
                    .Where(oi => OrderItemsID.Contains(oi.ID))
                    .ToListAsync();
                if (items.Count == 0)
                    return false;
                _context.OrderItems.RemoveRange(items);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<clsOrderItem>> GetItemsByOrderID(int OrderID)
        {
            return await _context.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderID == OrderID)
                .ToListAsync();
        }

        public async Task<List<clsOrder>> SearchOrdersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Orders
                .Include(o => o.Customer)
                .ThenInclude(c => c.Person)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => 
                    o.ID.ToString().Contains(searchTerm) ||
                    o.Customer.Person.FirstName.Contains(searchTerm) ||
                    o.Customer.Person.LastName.Contains(searchTerm) ||
                    o.Customer.Person.PhoneNumber.Contains(searchTerm) ||
                    o.OrderDate.ToString().Contains(searchTerm) ||
                    o.TotalAmount.ToString().Contains(searchTerm)
                )
                .ToListAsync();
        }
        

        public async Task<OrderDTO> GetByIdDTOAsync(int OrderID)
        {
            return await _context.Orders.AsNoTracking()
                .Where(o => o.ID == OrderID)
                .Select(o => new OrderDTO
                {
                    CustomerID = o.CustomerID,
                    PersonID = o.Customer.PersonID,
                    FirstName = o.Customer.Person.FirstName,
                    LastName = o.Customer.Person.LastName,
                    PhoneNumber = o.Customer.Person.PhoneNumber,
                    ID = o.ID,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    PaymentStatus = o.PaymentStatus,
                    ActionByUser = o.ActionByUser,
                    ActionType = o.ActionType,
                    ActionDate = o.ActionDate,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderDTO>> GetAllDTOAsync()
        {
            return await _context.Orders.AsNoTracking()
                .Select(o => new OrderDTO
                {
                    CustomerID = o.CustomerID,
                    PersonID = o.Customer.PersonID,
                    FirstName = o.Customer.Person.FirstName,
                    LastName = o.Customer.Person.LastName,
                    PhoneNumber = o.Customer.Person.PhoneNumber,
                    ID = o.ID,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    PaidAmount = o.PaidAmount,
                    PaymentStatus = o.PaymentStatus,
                    ActionByUser = o.ActionByUser,
                    ActionType = o.ActionType,
                    ActionDate = o.ActionDate,
                })
                .ToListAsync();
        }

        public async Task<OrderItemsDTO> GetOrderItemByIdDTOAsync(int OrderItemID)
        {
            return await _context.OrderItems.AsNoTracking()
                .Include(oi => oi.Product)
                .Where(oi => oi.ID == OrderItemID)
                .Select(oi => new OrderItemsDTO
                {
                    OrderID = oi.OrderID,
                    ID = oi.ID,
                    ProductID = oi.ProductID,
                    ProductName = oi.Product.Name,
                    ProductSaleAmount = oi.Quantity * oi.SellingPrice,
                    Quantity = oi.Quantity,
                    SellingPrice = oi.SellingPrice,
                    AvailableQuantity = oi.Product.AvailableQuantity
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderItemsDTO>> GetOrderItemsByOrderIdDTOAsync(int OrderID)
        {
            return await _context.OrderItems.AsNoTracking()
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderID == OrderID)
                .Select(oi => new OrderItemsDTO
                {
                    OrderID = oi.OrderID,
                    ID = oi.ID,
                    ProductID = oi.ProductID,
                    ProductName = oi.Product.Name,
                    ProductSaleAmount = oi.Quantity * oi.SellingPrice,
                    Quantity = oi.Quantity,
                    SellingPrice = oi.SellingPrice,
                    AvailableQuantity = oi.Product.AvailableQuantity
                })
                .ToListAsync();
        }
    }
}