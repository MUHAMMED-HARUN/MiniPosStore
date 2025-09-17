using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public   class clsProductFilter
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal? MinRetailPrice { get; set; }
        public decimal? MaxRetailPrice { get; set; }

        public decimal? MinWholesalePrice { get; set; }
        public decimal? MaxWholesalePrice { get; set; }

        public float? MinAvailableQuantity { get; set; }
        public float? MaxAvailableQuantity { get; set; }

        public string? CurrencyType { get; set; }
        public string? UnitOfMeasureName { get; set; }
  public      List <ProductDTO> products { set; get; } = new List<ProductDTO>();

    }
}
