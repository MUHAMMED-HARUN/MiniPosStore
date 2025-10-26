using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class ImportOrderItemUnionDTO
    {
        public int ImportOrderItemID { get; set; }
        public int ImportOrderID { get; set; }
        public int ItemID { get; set; }
        public int ItemType { get; set; } // 1=Product, 2=RawMaterial
        [DescriptionAttribute("ItemName")]
        public string Name { get; set; }
        public string Description { get; set; }
        public float Quantity { get; set; }
        public float SellingPrice { get; set; }
        public float WholesalePrice { get; set; }
        public int UOMID { get; set; }
        public string UOMName { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencyName { get; set; }
        public float ReservedQuantity { get; set; }
        public float ImportedQuantity { get; set; }
        public DateTime ImportDate { get; set; }
        public string SupplierName { get; set; }
        public string ActionByUser { get; set; }
        public DateTime ActionDate { get; set; }
    }
}