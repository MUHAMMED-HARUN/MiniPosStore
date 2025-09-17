using BAL.BALDTO;
using SharedModels.EF.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BALFilters
{
    public class clsCustomerFilterBAL:clsCustomerFilter
    {
        public List<CustomerDTO> customers { set; get; } = new List<CustomerDTO>();
    }
}
