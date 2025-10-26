using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsImportOrder
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual clsSupplier Supplier { get; set; }


        public float TotalAmount { get; set; }
        public float PaidAmount { get; set; }

        public DateTime ImportDate { get; set; }
        public byte PaymentStatus { get; set; }


        [ForeignKey("User")]
        public string ActionByUser {  get; set; }
        public virtual clsUser User { get; set; }

            
        public byte ActionType { get; set; }
        public DateTime ActionDate { get; set; }


        public virtual ICollection<clsImportOrderItem> ImportOrderItems { get; set; }
        public virtual ICollection<clsImportRawMaterialItem> ImportRawMaterialItems { get; set; }
    }
}
