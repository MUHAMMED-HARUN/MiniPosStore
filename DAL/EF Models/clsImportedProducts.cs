using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF_Models
{
    public class clsImportedProducts
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual clsSupplier Supplier { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual clsProduct Product { get; set; }
        public float Quantity { get; set; }
        public float ItemPrice { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
