using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public class clsRecipeInfoFilter
    {
        public string FilterName = "[dbo].[GetRecipeInfoFiltered]";
        public int? ID { get; set; }
        public string? RecipeName { get; set; }
        public string? MaterialName { get; set; }
        public decimal? RequiredMaterialQuantityFrom { get; set; }
        public decimal? RequiredMaterialQuantityTo { get; set; }
        public DateTime? ActionDateFrom { get; set; }
        public DateTime? ActionDateTo { get; set; }
        public List<RecipeInfoDTO> recipeInfos { set; get; } = new List<RecipeInfoDTO>();
    }
}
