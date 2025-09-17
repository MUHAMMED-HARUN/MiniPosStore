using BAL.BALDTO;
using SharedModels.EF.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BALFilters
{
    public class clsProductFilterBAL:clsProductFilter
    {
        public List<ProductDTO> ProductDTOList { get; set; } = new List<ProductDTO>();
    }
}
