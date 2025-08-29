using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Models
{
    public class clsSupplier
    {
        public int ID { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual clsPerson Person { get; set; }

        public string StoreName { get; set; }
        public string StoreAddress { get; set; }

        public virtual ICollection<clsImportOrder> ImportOrders { get; set; }
    }
}
