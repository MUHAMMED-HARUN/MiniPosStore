using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public class clsRawMaterialFilter
    {
        public string FilterName = "[dbo].[GetRawMaterialsFiltered]";
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? PurchasePriceFrom { get; set; }
        public decimal? PurchasePriceTo { get; set; }
        public decimal? AvailableQuantityFrom { get; set; }
        public decimal? AvailableQuantityTo { get; set; }
        public int? UOMID { get; set; }
        public int? CurrencyTypeID { get; set; }
        public string? SupplierName { get; set; }
        public DateTime? ActionDateFrom { get; set; }
        public DateTime? ActionDateTo { get; set; }
        public float? ReservedQuantityFrom { get; set; }
        public float? ReservedQuantityTo { get; set; }
        public List<RawMaterialDTO> rawMaterials { set; get; } = new List<RawMaterialDTO>();
    }
}
