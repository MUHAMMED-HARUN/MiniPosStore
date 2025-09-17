using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Models
{
    public class clsCustomer
    {
        
        [Key]
        public int ID { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual clsPerson Person { get; set; }
        public virtual ICollection<clsOrder>? Orders { get; set; }
    }
}
