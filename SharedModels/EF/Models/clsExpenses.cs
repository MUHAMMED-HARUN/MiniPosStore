using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsExpenses
    {
        [Key]
        public int ID { get; set; }
        public DateTime ExpenseDate { get; set; }
        [ForeignKey("ExpenseType")]
        public int ExpenseTypeID { get; set; }
        public virtual clsExpenseType ExpenseType { get; set; }
        public string? Description { get; set; }
        public float Amount { get; set; }
        [ForeignKey("User")]
        public string ActionByUser { get; set; }
        public virtual clsUser User { get; set; }


        public byte ActionType { get; set; }
        public DateTime ActionDate { get; set; }

    }
}
