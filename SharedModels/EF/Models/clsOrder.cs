using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsOrder
    {
        public int  ID { get; set; }
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual clsCustomer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalAmount { get; set; }
        public float PaidAmount { get; set; }
        public byte PaymentStatus { get; set; }



        [ForeignKey("User")]
        public string ActionByUser { get; set; }
        public virtual clsUser User { get; set; }


        public byte ActionType { get; set; }
        public DateTime ActionDate { get; set; }


        public virtual ICollection<clsOrderItem> OrderItems { get; set; }
        public virtual ICollection<clsRawMaterialOrderItem> RawMaterialOrderItems { get; set; }
    }
}
