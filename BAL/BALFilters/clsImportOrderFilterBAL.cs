using BAL.BALDTO;
using DAL.EF.DTO;
using DAL.EF.Filters;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BALFilters
{
    public class clsImportOrderFilterBAL:clsImportOrderFilter
    {
        public List<BAL.BALDTO.ImportOrderBALDTO> importOrders { get; set; } = new List<ImportOrderBALDTO>();
    }
}
