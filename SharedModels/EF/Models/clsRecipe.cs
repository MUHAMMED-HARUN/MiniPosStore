using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsRecipe
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual clsProduct Product { get; set; }
        public float YieldQuantity { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual clsUser User { get; set; }
        public int ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public virtual ICollection<clsRecipeInfo>? RecipeInfos { get; set; }
    }
}
