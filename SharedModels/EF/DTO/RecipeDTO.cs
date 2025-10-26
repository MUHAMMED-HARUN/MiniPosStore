using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class RecipeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public float YieldQuantity { get; set; }
        public DateTime ActionDate { get; set; }
        public string UserID { get; set; }
    }
}
