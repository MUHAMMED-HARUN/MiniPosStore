using BAL.BALDTO;
using DAL.EF.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BALFilters
{
    public class clsCustomerFilterBAL:clsCustomerFilter
    {
        public List<CustomerBALDTO> customers { set; get; } = new List<CustomerBALDTO>();
    }
}
