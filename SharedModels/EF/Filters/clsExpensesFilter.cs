using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.EF.DTO;

namespace SharedModels.EF.Filters
{
    public class clsExpensesFilter
    {
        public int? ID { get; set; }
        public DateTime? ExpenseDateFrom { get; set; }
        public DateTime? ExpenseDateTo { get; set; }
        public int? ExpenseTypeID { get; set; }
        public string? ExpenseTypeName { get; set; }
        public string? Description { get; set; }
        public float? AmountFrom { get; set; }
        public float? AmountTo { get; set; }
        public string? ActionByUser { get; set; }
        public string? UserName { get; set; }
        public byte? ActionType { get; set; }
        public DateTime? ActionDateFrom { get; set; }
        public DateTime? ActionDateTo { get; set; }
        
        public List<ExpensesDTO> expenses { get; set; } = new List<ExpensesDTO>();
    }
}
