using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class ExpenseTypeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
