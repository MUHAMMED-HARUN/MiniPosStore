using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsImportRawMaterialItem
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("ImportOrder")]
        public int ImportOrderID { get; set; }
        public virtual clsImportOrder ImportOrder { get; set; }
        [ForeignKey("RawMaterial")]
        public int RawMaterialID { get; set; }
        public virtual clsRawMaterial RawMaterial { get; set; }
        public double Quantity { get; set; }
        public double SellingPrice { get; set; }

    }
}
