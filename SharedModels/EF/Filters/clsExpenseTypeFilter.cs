using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.EF.DTO;

namespace SharedModels.EF.Filters
{
    public class clsExpenseTypeFilter
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        public List<ExpenseTypeDTO> expenseTypes { get; set; } = new List<ExpenseTypeDTO>();
    }
}
