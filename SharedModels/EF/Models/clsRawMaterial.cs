using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsRawMaterial
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float PurchasePrice { get; set; }
        public float AvailableQuantity { get; set; }
        [ForeignKey("unitOfMeasure")]
        public int UOMID { get; set; }
        public virtual clsUnitOfMeasure unitOfMeasure { get; set; }
        public int CurrencyTypeID { get; set; }
        [ForeignKey("Supplier")]
        public int MaterialSupplier { get; set; }
        public virtual clsSupplier Supplier { get; set; }
        public float? ReservedQuantity { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual clsUser User { get; set; }
        public int ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public virtual ICollection<clsRecipeInfo>? RecipeInfos { get; set; }
        public virtual ICollection<clsRawMaterialOrderItem>? RawMaterialOrderItems { get; set; }
        public virtual ICollection<clsImportRawMaterialItem>? ImportRawMaterialItems { get; set; }
    }
}
