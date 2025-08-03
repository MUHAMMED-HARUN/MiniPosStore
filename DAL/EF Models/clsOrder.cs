using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF_Models
{
    public class clsOrder
    {
        public int  ID { get; set; }
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual clsCustomer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public byte PaymentStatus { get; set; }


        public virtual ICollection<clsOrderItem> OrderItems { get; set; }
    }
}
