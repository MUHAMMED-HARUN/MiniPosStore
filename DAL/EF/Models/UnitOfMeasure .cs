using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Models
{
    public class clsUnitOfMeasure
    {
        public int ID { get; set; }
        [Required]
        public  string Name { get; set; }
        [Required]
        public string Seymbol { get; set; }
    }
}
