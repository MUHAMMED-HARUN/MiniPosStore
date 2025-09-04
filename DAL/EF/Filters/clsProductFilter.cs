using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Filters
{
    public class clsProductFilter
    {
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
        
    }
}
