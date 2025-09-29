using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.SP
{
   public  class clsTotalStockValue_SP
    {
        public int ? ProductID { get; set; }    
        public string SPName = "GetTotalStockValue";
    }
}
