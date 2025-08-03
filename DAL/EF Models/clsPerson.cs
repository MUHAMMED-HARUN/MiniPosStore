using DAL.EF_Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.EF_Models
{
    public class clsPerson
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }


        public virtual clsCustomer? Customer { get; set; }
        public virtual ICollection<clsSupplier>? Suppliers { get; set; }// لانه من الممكن ان يكون شخص واحد لديه اكثر من شركة
    }
}
