using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public class clsRecipeFilter
    {
        public string FilterName = "[dbo].[GetRecipesFiltered]";
        public int? id { get; set; }
        //public int? RecipesID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ProductName { get; set; }
        public decimal? YieldQuantityFrom { get; set; }
        public decimal? YieldQuantityTo { get; set; }
        public DateTime? ActionDateFrom { get; set; }
        public DateTime? ActionDateTo { get; set; }
        public List<RecipeDTO> recipes { set; get; } = new List<RecipeDTO>();
    }
}
