using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Models
{
    public class clsOrderItem
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public virtual clsOrder Order { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual clsProduct Product { get; set; }

        public float Quantity { get; set; }
        public float SellingPrice { get; set; }
        public float? PriceAdjustment { get; set; }
    }
}
