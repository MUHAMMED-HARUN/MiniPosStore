using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsLogRegister
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string NewData { get; set; }
        public string? OldData { get; set; }
       public DateTime ActionDate { get; set; }
        [ForeignKey("User")]
        public string ActoinByUser { get; set; }
        public virtual clsUser User { get; set; }
        public string ActionType { get; set; }
          public int  Version { get; set; }

    }
}
