using DAL.EF.AppDBContext;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using DAL.IRepo;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class ImportOrderRepo : IImportOrderRepo
    {
        private readonly AppDBContext _context;

        public ImportOrderRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(clsImportOrder importOrder)
        {
            try
            {
                await _context.ImportOrders.AddAsync(importOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsImportOrder importOrder)
        {
            try
            {
                _context.ImportOrders.Update(importOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int importOrderId, string CurentUserID)
        {
            try
            {
                var importOrder = await _context.ImportOrders.FindAsync(importOrderId);
                if (importOrder == null)
                    return false;

                await _context.Database.ExecuteSqlRawAsync(
    "EXEC sp_set_session_context 'UserID', {0}",
    CurentUserID
);
                _context.ImportOrders.Remove(importOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<clsImportOrder> GetByIdAsync(int importOrderId)
        {
            return await _context.ImportOrders.AsNoTracking().FirstOrDefaultAsync(io => io.ID == importOrderId);
        }

        public async Task<List<clsImportOrder>> GetAllAsync()
        {
            return await _context.ImportOrders.ToListAsync();
        }

        // Additional methods for ImportOrder
        public async Task<clsImportOrder> GetBySupplierIdAsync(int supplierID)
        {
            return await _context.ImportOrders.AsNoTracking()
                .FirstOrDefaultAsync(io => io.SupplierID == supplierID);
        }

        public async Task<List<clsImportOrder>> GetBySupplierIdDTOAsync(int supplierID)
        {
            return await _context.ImportOrders.AsNoTracking()
                .Where(io => io.SupplierID == supplierID)
                .ToListAsync();
        }

        public async Task<clsImportOrder> GetByIdWithItemsAsync(int importOrderID)
        {
            
            return await _context.ImportOrders.AsNoTracking()
                .Include(io => io.ImportOrderItems)
                .ThenInclude(item => item.Product)
                .Include(s=>s.Supplier)
                .ThenInclude(p=>p.Person)
                .FirstOrDefaultAsync(io => io.ID == importOrderID);
        }

        public async Task<List<clsImportOrder>> GetAllWithItemsAsync()
        {
            return await _context.ImportOrders.AsNoTracking()
                .Include(io => io.ImportOrderItems)
                .ThenInclude(item => item.Product)
                .ToListAsync();
        }

        public async Task<List<clsImportOrder>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ImportOrders.AsNoTracking()
                .Where(io => io.ImportDate >= startDate && io.ImportDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<clsImportOrder>> GetByPaymentStatusAsync(byte paymentStatus)
        {
            return await _context.ImportOrders.AsNoTracking()
                .Where(io => io.PaymentStatus == paymentStatus)
                .ToListAsync();
        }



        public async Task<bool> SetImportOrderPaymentStatusAsync(int ImportOrderID,byte PaymentStatus)
        {
            try
            {
               clsImportOrder importOrder=await _context.ImportOrders.AsNoTracking().FirstOrDefaultAsync(io => io.ID == ImportOrderID);
                importOrder.PaymentStatus = PaymentStatus;
                _context.ImportOrders.Update(importOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddItem(clsImportOrderItem ImportorderItem)
        {
            try
            {
                _context.ImportOrderItems.Add(ImportorderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateItem(clsImportOrderItem ImportorderItem)
        {
            try
            {
                _context.ImportOrderItems.Update(ImportorderItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItem(int ImportOrderItemID)
        {
            try
            {
                clsImportOrderItem item =await _context.ImportOrderItems.FirstOrDefaultAsync(IOI=>IOI.ID==ImportOrderItemID);
                if (item == null)
                    return false;
                _context.ImportOrderItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemByRange(int[] ImportOrderItemsID)
        {
            try
            {
                var items=await _context.ImportOrderItems.Where(IOI => ImportOrderItemsID.Contains(IOI.ID)).ToListAsync();
               
                if (items.Count == 0)
                    return false;

                _context.ImportOrderItems.RemoveRange(items);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<clsImportOrderItem>> GetItemsByOrderID(int ImportOrderID)
        {
            try
            {
                return await _context.ImportOrderItems.Where(IOI => IOI.ImportOrderID == ImportOrderID).ToListAsync();
            }
            catch
            {
                return new List<clsImportOrderItem>();
            }

        }

        public async Task<ImportOrderDTO> GetByIdDTOAsync(int ImportOrderID)
        {
            return await _context.ImportOrders.AsNoTracking()
                .Where(io => io.ID == ImportOrderID)
                .Select(io => new ImportOrderDTO
                {
                    ImportOrderID = io.ID,
                    SupplierID = io.SupplierID,
                    SupplierName = io.Supplier.Person.FirstName+" "+io.Supplier.Person.LastName,
                    TotalAmount = io.TotalAmount,
                    PaidAmount = io.PaidAmount,
                    ImportDate = io.ImportDate,
                    PaymentStatus = io.PaymentStatus,
                    ActionByUser = io.ActionByUser,
                    UserName = io.User.UserName,
                    ActionType = io.ActionType,
                    ActionDate = io.ActionDate,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ImportOrderDTO>> GetAllDTOAsync()
        {
            return await _context.ImportOrders.AsNoTracking()
                .Select(io => new ImportOrderDTO
                {
                    ImportOrderID = io.ID,
                    SupplierID = io.SupplierID,
                    SupplierName = io.Supplier.Person.FirstName + " " + io.Supplier.Person.LastName,
                    TotalAmount = io.TotalAmount,
                    PaidAmount = io.PaidAmount,
                    ImportDate = io.ImportDate,
                    PaymentStatus = io.PaymentStatus,
                    ActionByUser = io.ActionByUser,
                    UserName = io.User.UserName,
                    ActionType = io.ActionType,
                    ActionDate = io.ActionDate
                     ,ItemsCount = io.ImportOrderItems.Count
                })
                .ToListAsync();
        }
        public async Task<List<ImportOrderDTO>> GetAllDTOAsync(clsImportOrderFilter filter)
        {
            string Query = @$"select * from GetImportOrdersFiltered ( {clsDALUtil.GetSqlPrameterString<clsImportOrderFilter>()})";

            using (var connection = _context.Database.GetDbConnection().CreateCommand())
            {
                connection.CommandText = Query;
                connection.CommandType = System.Data.CommandType.Text;

                if (connection.Connection.State != System.Data.ConnectionState.Open)
                    connection.Connection.Open();

                var arr = clsDALUtil.GetSqlPrameters<clsImportOrderFilter>(filter).ToArray();
                connection.Parameters.AddRange(arr);

                List<ImportOrderDTO> Orders = new List<ImportOrderDTO>();
                try
                {
                    using (var reader = connection.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new ImportOrderDTO();
                            clsDALUtil.MapToClass<ImportOrderDTO>(reader, ref order);
                            Orders.Add(order);
                        }
                    }
                }
                catch (SqlException s)
                {

                }

                return Orders;
            }
        }
        public async Task<ImportOrderItemDTO> GetImportOrderItemByIdDTOAsync(int ImportOrderItemID)
        {
            return await _context.ImportOrderItems.AsNoTracking()
                .Where(ioi => ioi.ID == ImportOrderItemID)
                .Select(ioi => new ImportOrderItemDTO
                {
                    ImportOrderID = ioi.ImportOrderID,
                    ImportOrderItemID = ioi.ID,
                    ProductID = ioi.ProductID,
                    ProductName = ioi.Product.Name,
                    Quantity = ioi.Quantity,
                    SellingPrice = ioi.SellingPrice,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ImportOrderItemDTO>> GetImportOrderItemsByOrderIdDTOAsync(int ImportOrderID)
        {
            return await _context.ImportOrderItems.AsNoTracking()
                .Where(ioi => ioi.ImportOrderID == ImportOrderID)
                .Select(ioi => new ImportOrderItemDTO
                {
                    ImportOrderID = ioi.ImportOrderID,
                    ImportOrderItemID = ioi.ID,
                    ProductID = ioi.ProductID,
                    ProductName = ioi.Product.Name,
                    Quantity = ioi.Quantity,
                    SellingPrice = ioi.SellingPrice,
                    TotalItemAmount = ioi.Quantity * ioi.SellingPrice,
              
                })
                .ToListAsync();
        }
    }
}