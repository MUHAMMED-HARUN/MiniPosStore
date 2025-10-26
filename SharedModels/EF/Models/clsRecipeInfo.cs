using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsRecipeInfo
    {
        [Key]
      public  int ID { get; set; }
        [ForeignKey("Recipe")]
        public int RecipeID { get; set; }
        public virtual clsRecipe Recipe { get; set; }
        [ForeignKey("RawMaterial")]
        public int RawMaterialID { get; set; }
        public virtual clsRawMaterial RawMaterial { get; set; }
        public float RequiredMaterialQuantity { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual clsUser User { get; set; }
        public int ActionType { get; set; }
        public DateTime ActionDate { get; set; }

    }
}
