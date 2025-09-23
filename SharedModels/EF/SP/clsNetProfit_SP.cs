using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.SP
{
    public class clsNetProfit_SP
    {
       public DateTime? TargetDate { get; set; }
        public int? TargetOrder { get; set; }
        public int? TargetProduct { get; set; }

        public  string SPName = "GetNetProfit";
    }
}
