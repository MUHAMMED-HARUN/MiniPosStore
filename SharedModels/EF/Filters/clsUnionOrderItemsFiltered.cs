using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public class clsOrderItemUnionFilter
    {
        public string FilterName = "ufn_GetUnionOrderItemsFiltered";
        public int? OrderItemID { get; set; }
        public int? OrderID { get; set; }
        public int? ItemID { get; set; }
        public int? ItemType { get; set; } // "Product" or "Material"
        public string Name { get; set; }
        public string Description { get; set; }

        public float? QuantityFrom { get; set; }
        public float? QuantityTo { get; set; }

        public float? SellingPriceFrom { get; set; }
        public float? SellingPriceTo { get; set; }

        public float? WholesalePriceFrom { get; set; }
        public float? WholesalePriceTo { get; set; }

        public int? UOMID { get; set; }
        public int? CurrencyType { get; set; }

        public float? ReservedQuantityFrom { get; set; }
        public float? ReservedQuantityTo { get; set; }
        public List<OrderItemUnionDTO> OrderItemUnionDTOs { get; set; }= new List<OrderItemUnionDTO>();
    }
}
