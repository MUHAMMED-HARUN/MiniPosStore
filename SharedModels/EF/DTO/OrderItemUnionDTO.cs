using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class OrderItemUnionDTO
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public float Quantity { get; set; }
        public float SellingPrice { get; set; }
        public float? PriceAdjustment { get; set; }
        public float WholesalePrice { get; set; }
        public int UOMID { get; set; }
        public int CurrencyType { get; set; }
        public float ReservedQuantity { get; set; }
        public int ItemType { get; set; } // "Product" or "Material"
    }
}
