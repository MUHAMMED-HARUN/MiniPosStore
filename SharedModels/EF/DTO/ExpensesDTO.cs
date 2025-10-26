using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class ExpensesDTO
    {
        public int ID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int ExpenseTypeID { get; set; }
        public string ExpenseTypeName { get; set; }
        public string? Description { get; set; }
        public float Amount { get; set; }
        public string ActionByUser { get; set; }
        public string UserName { get; set; }
        public byte ActionType { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
