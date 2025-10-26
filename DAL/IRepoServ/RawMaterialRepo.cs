using DAL.EF.AppDBContext;
using DAL.IRepo;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepoServ
{
    public class RawMaterialRepo : IRawMaterialService
    {
        private readonly AppDBContext _context;

        public RawMaterialRepo(AppDBContext context)
        {
            _context = context;
        }

       
        public clsRawMaterial rawMaterial { get; set; }

        public async Task<List<clsRawMaterial>> GetAllAsync()
        {
            return await _context.RawMaterials
                .Include(rm => rm.unitOfMeasure)
                .Include(rm => rm.Supplier)
                .Include(rm => rm.User)
                .ToListAsync();
        }

        public async Task<List<RawMaterialDTO>> GetAllDTOAsync(clsRawMaterialFilter filter)
        {
            return await clsDALUtil.ExecuteFilterCommands<RawMaterialDTO, clsRawMaterialFilter>(_context, filter, filter.FilterName);
        }

        public async Task<List<RawMaterialDTO>> GetAllDTOAsync()
        {
            return await _context.RawMaterials
                .Include(rm => rm.unitOfMeasure)
                .Include(rm => rm.Supplier)
                .Select(rm => new RawMaterialDTO
                {
                    ID = rm.ID,
                    Name = rm.Name,
                    Description = rm.Description,
                    PurchasePrice = rm.PurchasePrice,
                    AvailableQuantity = rm.AvailableQuantity,
                    UOMID = rm.UOMID,
                    UOMName = rm.unitOfMeasure.Name,
                    CurrencyTypeID = rm.CurrencyTypeID,
                    MaterialSupplier = rm.MaterialSupplier,
                    SupplierName = rm.Supplier.Person.FirstName + " " + rm.Supplier.Person.LastName,
                    ActionDate = rm.ActionDate
                }).ToListAsync();
        }

        public async Task<clsRawMaterial> GetByIdAsync(int rawMaterialID)
        {
            return await _context.RawMaterials
                .Include(rm => rm.unitOfMeasure)
                .Include(rm => rm.Supplier)
                .Include(rm => rm.User)
                .FirstOrDefaultAsync(rm => rm.ID == rawMaterialID);
        }

        public async Task<RawMaterialDTO> GetByIdDTOAsync(int rawMaterialID)
        {
            clsRawMaterialFilter filter = new clsRawMaterialFilter();
            filter.ID = rawMaterialID;
            var result =await GetAllDTOAsync(filter);
            return result.FirstOrDefault();
        }

        public async Task<bool> AddAsync(clsRawMaterial rawMaterial)
        {
            try
            {
                _context.RawMaterials.Add(rawMaterial);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddDTOAsync(RawMaterialDTO rawMaterialDTO)
        {
            try
            {
                var rawMaterial = new clsRawMaterial
                {
                    Name = rawMaterialDTO.Name,
                    Description = rawMaterialDTO.Description,
                    PurchasePrice = rawMaterialDTO.PurchasePrice,
                    AvailableQuantity = rawMaterialDTO.AvailableQuantity,
                    UserID = rawMaterialDTO.UserID,
                    UOMID = rawMaterialDTO.UOMID,
                    CurrencyTypeID = rawMaterialDTO.CurrencyTypeID,
                    MaterialSupplier = rawMaterialDTO.MaterialSupplier,

                };

                _context.RawMaterials.Add(rawMaterial);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(clsRawMaterial rawMaterial)
        {
            try
            {
                _context.RawMaterials.Update(rawMaterial);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDTOAsync(RawMaterialDTO rawMaterialDTO)
        {
            try
            {
                var rawMaterial = await _context.RawMaterials.FindAsync(rawMaterialDTO.ID);
                if (rawMaterial != null)
                {
                    rawMaterial.Name = rawMaterialDTO.Name;
                    rawMaterial.Description = rawMaterialDTO.Description;
                    rawMaterial.PurchasePrice = rawMaterialDTO.PurchasePrice;
                    rawMaterial.AvailableQuantity = rawMaterialDTO.AvailableQuantity;
                 //   rawMaterial.ProductionLossQuantity = rawMaterialDTO.ProductionLossQuantity;
                    rawMaterial.UOMID = rawMaterialDTO.UOMID;
                    rawMaterial.CurrencyTypeID = rawMaterialDTO.CurrencyTypeID;
                    rawMaterial.MaterialSupplier = rawMaterialDTO.MaterialSupplier;
                    rawMaterial.ActionType = 2;
                    rawMaterial.ActionDate = DateTime.Now;

                    _context.RawMaterials.Update(rawMaterial);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int rawMaterialID, string UserID)
        {
            try
            {
                var rawMaterial = await _context.RawMaterials.FindAsync(rawMaterialID);
                if (rawMaterial != null)
                {
                    rawMaterial.ActionType = 3; // Soft Delete
                    rawMaterial.UserID = UserID;
                    _context.RawMaterials.Update(rawMaterial);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async  Task<bool> IsAvailable(int  rawMaterialID, float Quantity)
        {
          var Material =   _context.RawMaterials.Where(m => m.ID == rawMaterialID).FirstOrDefault();
            if(Material==null)
                return false;
            if (Quantity <= Material.AvailableQuantity)
                return true;
            else
                return false;
        }
        public async Task<bool> IncreaseByRange(Dictionary<int, float> rawMaterialInfo)
        {
            try
            {
                var MaterialsID = rawMaterialInfo.Select(rm => rm.Key).ToList();
                List<clsRawMaterial> rawMaterials = await _context.RawMaterials.Where(m => MaterialsID.Contains(m.ID)).ToListAsync();
                foreach (var item in rawMaterials)
                {
                    item.AvailableQuantity += rawMaterialInfo[item.ID];
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> IncreaseAsync(int rawMaterialID, float Quantity)
        {
            var Material = await _context.RawMaterials.Where(m => m.ID == rawMaterialID).FirstOrDefaultAsync();
            if (Material == null)
                return false;
            Material.AvailableQuantity += Quantity;
            await Save();
            return true;

        }
        public async Task<bool> DecreaseAsync(int rawMaterialID, float Quantity)
        {
            var Material = await _context.RawMaterials.Where(m => m.ID == rawMaterialID).FirstOrDefaultAsync();
       
            if (Material == null)
                return false;

            if (Material.AvailableQuantity >= Quantity)
            {
                Material.AvailableQuantity -= Quantity;
                _context.Update(Material);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<bool> DecreaseByRange(Dictionary<int , float> rawMaterialInfo)
        {
            try
            {
                var MaterialsID = rawMaterialInfo.Select(rm => rm.Key).ToList();
                List<clsRawMaterial> rawMaterials = await _context.RawMaterials.Where(m => MaterialsID.Contains(m.ID)).ToListAsync();

                foreach (var item in rawMaterials)
                {
                    var Quantity =  rawMaterialInfo[item.ID];
                    if (item.AvailableQuantity >= Quantity)
                        continue;
                    else
                        return false;
                }

                foreach (var item in rawMaterials)
                {
                    item.AvailableQuantity -= rawMaterialInfo[item.ID];
                }
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Save()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddOrderItem(clsRawMaterialOrderItem materialOrderItem)
        {
         
          await _context.RawMaterialOrderItems.AddAsync(materialOrderItem);
            return await Save();
        }

        public async Task<bool> UpdateOrderItem(clsRawMaterialOrderItem materialOrderItem)
        {
             _context.RawMaterialOrderItems.Update(materialOrderItem);
            return await Save();
        }

        public async Task<bool> DeleteOrderItem(clsRawMaterialOrderItem materialOrderItem)
        {
            var item =  _context.RawMaterialOrderItems.FirstOrDefault(moi=>moi.ID== materialOrderItem.ID);
            if(item!=null)
            {
                _context.RawMaterialOrderItems.Remove(item);
                return await Save();
            }
            return false;
        }

        public async Task<List<clsRawMaterialOrderItem>> GetAllOrderItem()
        {
            return await _context.RawMaterialOrderItems
                .Include(moi => moi.Order)
                .Include(moi => moi.RawMaterial)
                .ToListAsync();
        }

        public async Task<List<clsRawMaterialOrderItem>> GetAllOrderItem(int OrderID)
        {
            return await _context.RawMaterialOrderItems
                .Include(moi => moi.Order)
                .Include(moi => moi.RawMaterial)
                .Where(moi => moi.OrderID == OrderID)
                .ToListAsync();
        }

        public async Task<bool> AddImportOrderItem(clsImportRawMaterialItem importRawMaterialItem)
        {
           await _context.ImportRawMaterialItems.AddAsync(importRawMaterialItem);
            return await Save();
        }

        public async Task<bool> UpdateImportOrderItem(clsImportRawMaterialItem importRawMaterialItem)
        {
             _context.ImportRawMaterialItems.Update(importRawMaterialItem);
            return await Save();
        }

        public async Task<bool> DeleteImportOrderItem(clsImportRawMaterialItem importRawMaterialItem)
        {
            var iotem = _context.ImportRawMaterialItems.FirstOrDefault(Iom => Iom.ID == importRawMaterialItem.ID);
            if(iotem != null)
            {
                _context.ImportRawMaterialItems.Remove(iotem);
                return await Save();
            }
            return false;
        }

        public async Task<List<clsImportRawMaterialItem>> GetAllImportOrderItem()
        {
         return await  _context.ImportRawMaterialItems
                .Include(iom => iom.ImportOrder)
                .Include(iom => iom.RawMaterial).ToListAsync();
        }

        public async Task<List<clsImportRawMaterialItem>> GetAllImportOrderItem(int ImportOrderID)
        {
            return await _context.ImportRawMaterialItems
                .Include(iom => iom.ImportOrder)
                .Include(iom => iom.RawMaterial)
                .Where(iom => iom.ImportOrderID == ImportOrderID)
                .ToListAsync();
        }

        public async Task<bool> ReserveQuantity(int MaterialID, float ReserveedQuantity)
        {
            var Material =  _context.RawMaterials.Where(m => m.ID == MaterialID).FirstOrDefault();
            if (Material == null)
                return false;
            float ReservedQuantity = Material.ReservedQuantity ?? 0;
            if ((Material.AvailableQuantity -ReservedQuantity )>= ReserveedQuantity)
            {
                if (Material.ReservedQuantity == null)
                    Material.ReservedQuantity = new float();
                Material.ReservedQuantity += ReserveedQuantity;
               await Save();
                return true;
            }
            return false;

        }

        public async Task<bool> DeReserveQuantity(int MaterialID, float deReserveedQuantity)
        {
           var Material =  _context.RawMaterials.Where(m => m.ID == MaterialID).FirstOrDefault();
            if (Material == null)
                return false;
            if (Material.ReservedQuantity >= deReserveedQuantity)
            {
                Material.ReservedQuantity -= deReserveedQuantity;
               return await Save();
            }
            return true; 
        }

        public async Task<clsRawMaterialOrderItem> GetMaterialItem(int OIMaterial)
        {
            try
            {
                return await _context.RawMaterialOrderItems.AsNoTracking()
                    .FirstOrDefaultAsync(moi => moi.ID == OIMaterial);
            }
            catch (SqlException e)
            {
                return null;
            }
        }
        public async Task<List<clsRawMaterialOrderItem>> GetMaterialItemByOrderID(int OrderID)
        {
            return await _context.RawMaterialOrderItems
             .Include(moi => moi.Order)
             .Include(moi => moi.RawMaterial)
             .Where(moi => moi.OrderID == OrderID).ToListAsync(); 
        }
        public async Task<clsImportRawMaterialItem> GetMaterialImportItem(int IOIMaterial)
        {
            return await _context.ImportRawMaterialItems.AsNoTracking()
                .Include(iom => iom.ImportOrder)
                .Include(iom => iom.RawMaterial)
                .FirstOrDefaultAsync(iom => iom.ID == IOIMaterial);
        }

        public async Task<bool> IsExistByName(string Name, int ID = 0)
        {
            if (ID == 0)
                return await _context.RawMaterials.AnyAsync(m => m.Name == Name);
            else
                return await _context.RawMaterials.AnyAsync(m => m.Name == Name && m.ID != ID);

        }
    }
}
