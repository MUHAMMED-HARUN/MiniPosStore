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
using DAL;

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
                // Create a clean ImportOrder object without navigation properties
                var cleanImportOrder = new clsImportOrder
                {
                    SupplierID = importOrder.SupplierID,
                    TotalAmount = importOrder.TotalAmount,
                    PaidAmount = importOrder.PaidAmount,
                    ImportDate = importOrder.ImportDate,
                    PaymentStatus = importOrder.PaymentStatus,
                    ActionByUser = importOrder.ActionByUser,
                    ActionType = importOrder.ActionType,
                    ActionDate = importOrder.ActionDate
                };

                await _context.ImportOrders.AddAsync(cleanImportOrder);
                await _context.SaveChangesAsync();
                
                // Update the original object with the generated ID
                importOrder.ID = cleanImportOrder.ID;
                
                return true;
            }
            catch(Exception e)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"ImportOrderRepo.AddAsync Error: {e.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {e.StackTrace}");
                
                // Get inner exception details
                string errorMessage = e.Message;
                if (e.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {e.InnerException.Message}";
                }
                
                // Re-throw the exception to see it in the application
                throw new Exception($"فشل في إضافة أمر الاستيراد: {errorMessage}", e);
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

               public async Task<List<ImportOrderItemUnionDTO>> GetImportOrderItemUnionDTOs(clsImportOrderItemUnionFilter filter)
               {
                   return await clsDALUtil.ExecuteFilterCommands<ImportOrderItemUnionDTO, clsImportOrderItemUnionFilter>(_context, filter, filter.FilterName);
               }

               public async Task<bool> AddRawMaterialItem(clsImportRawMaterialItem rawMaterialItem)
               {
                   try
                   {
                       await _context.ImportRawMaterialItems.AddAsync(rawMaterialItem);
                       await _context.SaveChangesAsync();
                       return true;
                   }
                   catch (Exception)
                   {
                       return false;
                   }
               }

               public async Task<bool> UpdateRawMaterialItem(clsImportRawMaterialItem rawMaterialItem)
               {
                   try
                   {
                       _context.ImportRawMaterialItems.Update(rawMaterialItem);
                       await _context.SaveChangesAsync();
                       return true;
                   }
                   catch (Exception)
                   {
                       return false;
                   }
               }

               public async Task<List<clsImportRawMaterialItem>> GetRawMaterialItemsByOrderID(int importOrderID)
               {
                   try
                   {
                       return await _context.ImportRawMaterialItems
                           .Where(item => item.ImportOrderID == importOrderID)
                           .ToListAsync();
                   }
                   catch (Exception)
                   {
                       return new List<clsImportRawMaterialItem>();
                   }
               }

        public async Task<clsImportRawMaterialItem> GetRawMaterialItemByIdAsync(int id)
        {
            try
            {
                return await _context.ImportRawMaterialItems
                    .FirstOrDefaultAsync(item => item.ID == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteRawMaterialItem(int rawMaterialItemID)
        {
            try
            {
                var item = await _context.ImportRawMaterialItems
                    .FirstOrDefaultAsync(i => i.ID == rawMaterialItemID);
                
                if (item != null)
                {
                    _context.ImportRawMaterialItems.Remove(item);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}