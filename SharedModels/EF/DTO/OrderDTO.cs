using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
	public   class OrderDTO
	{
		public int ID { get; set; }
		
		[Required(ErrorMessage = "العميل مطلوب")]
		[Display(Name = "العميل")]
		public int CustomerID { get; set; }
		
		// Customer Fields
		[Display(Name = "معرف الشخص")]
		public int PersonID { get; set; }
		
		[Display(Name = "الاسم الأول")]
		public string FirstName { get; set; }
		
		[Display(Name = "الاسم الأخير")]
		public string LastName { get; set; }
		
		[Display(Name = "رقم الهاتف")]
		public string PhoneNumber { get; set; }
		
		// Order Fields
		public int OrderID { get; set; }
		public DateTime OrderDate { get; set; }
		
		[Required(ErrorMessage = "المبلغ الإجمالي مطلوب")]
		[Display(Name = "المبلغ الإجمالي")]
		[Range(0, double.MaxValue, ErrorMessage = "المبلغ الإجمالي يجب أن يكون أكبر من أو يساوي صفر")]
		public float TotalAmount { get; set; }
		
		[Required(ErrorMessage = "المبلغ المدفوع مطلوب")]
		[Display(Name = "المبلغ المدفوع")]
		[Range(0, double.MaxValue, ErrorMessage = "المبلغ المدفوع يجب أن يكون أكبر من أو يساوي صفر")]
		public float PaidAmount { get; set; }
		
		[Required(ErrorMessage = "حالة الدفع مطلوبة")]
		[Display(Name = "حالة الدفع")]
		public byte PaymentStatus { get; set; }
		
		[Display(Name = "المستخدم المسؤول")]
		public string ActionByUser { get; set; }
		
		[Display(Name = "نوع العملية")]
		public byte ActionType { get; set; }
		
		[Display(Name = "تاريخ العملية")]
		public DateTime ActionDate { get; set; }
		
		// Navigation Properties
		[Display(Name = "عناصر الطلب")]
		public List<OrderItemsDTO> OrderItems { get; set; } = new List<OrderItemsDTO>();
	}
}
