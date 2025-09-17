using BAL.BALDTO;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BALFilters
{
    public class clsImportOrderFilterBAL:clsImportOrderFilter
    {
        public List<BAL.BALDTO.ImportOrderDTO> importOrders { get; set; } = new List<ImportOrderDTO>();
    }
}
