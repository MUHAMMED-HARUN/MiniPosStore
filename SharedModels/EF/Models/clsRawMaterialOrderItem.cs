using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsRawMaterialOrderItem
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public virtual clsOrder Order { get; set; }
        [ForeignKey("RawMaterial")]
        public int RawMaterialID { get; set; }
        public virtual clsRawMaterial RawMaterial { get; set; }
        public double Quantity { get; set; }
        public double SellingPrice { get; set; }
        public double WholesalePrice { get; set; }
    }
}
