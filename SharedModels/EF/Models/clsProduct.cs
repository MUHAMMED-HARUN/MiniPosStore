using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsProduct
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public float RetailPrice { get; set; }
        public float WholesalePrice { get; set; }
        public float AvailableQuantity { get; set; }
        public string CurrencyType  { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey("UnitOfMeasure")]
        public int UOMID {  get; set; }
        public virtual clsUnitOfMeasure UnitOfMeasure { get; set; }

        [ForeignKey("User")]
        public string ActionByUser { get; set; }
        public virtual clsUser User { get; set; }


        public byte ActionType { get; set; }
        public DateTime ActionDate { get; set; }


        public virtual ICollection<clsOrderItem> OrderItems { get; set; }
        public virtual  ICollection<clsImportOrder>? ImportedProducts { get; set; }
    }
}
