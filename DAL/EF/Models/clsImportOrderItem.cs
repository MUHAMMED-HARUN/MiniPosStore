using SharedModels.EF.Models;
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
    public class clsImportOrderItem
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("ImportOrder")]
        public int ImportOrderID { get; set; }
        public virtual clsImportOrder ImportOrder { get; set; }



        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual clsProduct Product { get; set; }

        public float Quantity { get; set; }
        public float SellingPrice { get; set; }

    }
}
