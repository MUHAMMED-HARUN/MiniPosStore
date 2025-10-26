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
    public interface IRawMaterialService
    {
        clsGlobal.enSaveMode SaveMode { get; set; }
        clsRawMaterial rawMaterial { get; set; }
        
        Task<bool> AddAsync(clsRawMaterial rawMaterial);
        Task<bool> UpdateAsync(clsRawMaterial rawMaterial);
        Task<bool> DeleteAsync(int rawMaterialID);
        Task<clsRawMaterial> GetByIdAsync(int rawMaterialID);
        Task<List<clsRawMaterial>> GetAllAsync();
        
        // BALDTO Methods
        Task<RawMaterialDTO> GetByIdBALDTOAsync(int rawMaterialID);
        Task<List<RawMaterialDTO>> GetAllBALDTOAsync();
        Task<List<RawMaterialDTO>> GetAllBALDTOAsync(clsRawMaterialFilter filter);
        Task<bool> AddBALDTOAsync(RawMaterialDTO rawMaterialDTO);
        Task<bool> UpdateBALDTOAsync(RawMaterialDTO rawMaterialDTO);
        
        Task<bool> Save();


        Task<bool> AddOrderItem(clsRawMaterialOrderItem materialOrderItem);
        Task<bool> UpdateOrderItem(clsRawMaterialOrderItem materialOrderItem);
        Task<bool> DeleteOrderItem(clsRawMaterialOrderItem materialOrderItem);
        Task<clsRawMaterialOrderItem> GetMaterialItem(int OIMaterial);
        Task<List<clsRawMaterialOrderItem>> GetAllOrderItem();
        Task<List<clsRawMaterialOrderItem>> GetAllOrderItem(int OrderID);
        Task<bool> AddImportOrderItem(clsImportRawMaterialItem importRawMaterialItem);
        Task<bool> UpdateImportOrderItem(clsImportRawMaterialItem importRawMaterialItem);
        Task<bool> DeleteImportOrderItem(clsImportRawMaterialItem importRawMaterialItem);
        Task<clsImportRawMaterialItem> GetMaterialImportItem(int IOIMaterial);
        Task<List<clsRawMaterialOrderItem>> GetMaterialItemByOrderID(int OrderID);
        Task<List<clsImportRawMaterialItem>> GetAllImportOrderItem();
        Task<List<clsImportRawMaterialItem>> GetAllImportOrderItem(int ImportOrderID);

        Task<bool> IncreaseByRange(Dictionary<int, float> rawMaterialInfo);
        Task<bool> IncreaseAsync(int rawMaterialID, float Quantity);
        Task<bool> DecreaseAsync(int rawMaterialID, float Quantity);
        Task<bool> ReserveQuantity(int MaterialID, float ReserveedQuantity);
        Task<bool> DeReserveQuantity(int MaterialID, float deReserveedQuantity);
        Task<bool> IsAvalableQuantity(int MaterialID, float Quantity);
        
        // Import Order handling
        Task HandleImportOrderConfirmed(int importOrderID, List<SharedModels.EF.DTO.ImportOrderItemUnionDTO> rawMaterialItems);
        Task<bool> IsExistByName(string Name, int ID=0);
    }
}
