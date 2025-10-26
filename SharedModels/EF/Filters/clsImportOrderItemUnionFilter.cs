using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public class clsImportOrderItemUnionFilter
    {
        public string FilterName = "ufn_GetUnionImportOrderItemsFiltered";
        public int? ImportOrderItemID { get; set; }
        public int? ImportOrderID { get; set; }
        public int? ItemID { get; set; }
        public int? ItemType { get; set; } // 1=Product, 2=RawMaterial
        public string ItemName { get; set; }
        public string Description { get; set; }

        public float? QuantityFrom { get; set; }
        public float? QuantityTo { get; set; }

        public float? SellingPriceFrom { get; set; }
        public float? SellingPriceTo { get; set; }

        public float? WholesalePriceFrom { get; set; }
        public float? WholesalePriceTo { get; set; }
        public float ?AvailableQuantityFrom { get; set; }
        public float? AvailableQuantityTo { get; set; }
        public DateTime? ImportDateFrom { get; set; }
        public DateTime? ImportDateTo { get; set; }

        public string SupplierName { get; set; }
     public   List<SharedModels.EF.DTO.ImportOrderItemUnionDTO> OrderItemUnionDTOs { get; set; } = new List<DTO.ImportOrderItemUnionDTO>();
    }
}