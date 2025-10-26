using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class RecipeInfoDTO
    {
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
        public float RequiredMaterialQuantity { get; set; }
        public DateTime ActionDate { get; set; }
        public string UserID { get; set; }
    }
}
