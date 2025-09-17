using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public    class OrderItemsDTO
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "معرف الطلب مطلوب")]
        [Display(Name = "معرف الطلب")]
        public int OrderID { get; set; }
        
        [Required(ErrorMessage = "المنتج مطلوب")]
        [Display(Name = "المنتج")]
        public int ProductID { get; set; }
        
        [Display(Name = "اسم المنتج")]
        public string ProductName { get; set; }
        
        [Display(Name = "سعر البيع للمنتج")]
        public float ProductSaleAmount { get; set; }
        
        [Required(ErrorMessage = "الكمية مطلوبة")]
        [Display(Name = "الكمية")]
        [Range(0.01, double.MaxValue, ErrorMessage = "الكمية يجب أن تكون أكبر من صفر")]
        public float Quantity { get; set; }
        
        [Required(ErrorMessage = "سعر البيع مطلوب")]
        [Display(Name = "سعر البيع")]
        [Range(0, double.MaxValue, ErrorMessage = "سعر البيع يجب أن يكون أكبر من أو يساوي صفر")]
        public float SellingPrice { get; set; }
        [Display(Name = "خصومات او اضافات")]
        public float? PriceAdjustment { get; set; }
        // خصائص محسوبة
        [Display(Name = "المجموع الفرعي")]
        public float SubTotal => Quantity * SellingPrice;
        
        [Display(Name = "المبلغ المخصوم")]
        public float DiscountAmount => PriceAdjustment ?? 0;
        
        [Display(Name = "المبلغ بعد الخصم")]
        public float FinalAmount => SubTotal - DiscountAmount;
        
        [Display(Name ="الكمية في المخزن")]
        public float AvailableQuantity { get; set; }
    }
}
