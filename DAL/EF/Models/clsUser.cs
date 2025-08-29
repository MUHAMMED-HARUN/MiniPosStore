using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.EF.Models
{
    public class clsUser:IdentityUser
    {
        public int Permissions {  get; set; }
        public virtual ICollection<clsImportOrder> ImportOrders { get; set; }
        public virtual ICollection<clsProduct> Products { get; set; }
        public virtual ICollection<clsOrder> Orders { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual clsPerson Person { get; set; }
        public virtual ICollection<clsLogRegister>? LogRegister { get; set; }
    }
}
