using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.DTO
{
	public class SupplierDTO
	{
		public int SupplierID { get; set; }
		public int PersonID { get; set; }
		public string ShopName { get; set; }
		public string Address { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
	}
}
