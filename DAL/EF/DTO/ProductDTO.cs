using System;
using System.ComponentModel.DataAnnotations;


namespace DAL.EF.DTO
{
    public class ProductDTO
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [Display(Name = "اسم المنتج")]
        public string Name { get; set; }
        
        [Display(Name = "الوصف")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "سعر البيع مطلوب")]
        [Display(Name = "سعر البيع")]
        [Range(0, double.MaxValue, ErrorMessage = "سعر البيع يجب أن يكون أكبر من صفر")]
        public float RetailPrice { get; set; }
        
        [Required(ErrorMessage = "سعر الجملة مطلوب")]
        [Display(Name = "سعر الجملة")]
        [Range(0, double.MaxValue, ErrorMessage = "سعر الجملة يجب أن يكون أكبر من صفر")]
        public float WholesalePrice { get; set; }
        
        [Required(ErrorMessage = "الكمية المتاحة مطلوبة")]
        [Display(Name = "الكمية المتاحة")]
        [Range(0, double.MaxValue, ErrorMessage = "الكمية المتاحة يجب أن تكون أكبر من أو تساوي صفر")]
        public float AvailableQuantity { get; set; }
        
        [Required(ErrorMessage = "نوع العملة مطلوب")]
        [Display(Name = "نوع العملة")]
        public string CurrencyType { get; set; }
         [Display(Name = "اسم العملة")]
        public string CurrencyName { get; set; }

        [Display(Name = "مسار الصورة")]
        public string ImagePath { get; set; }
        
        [Required(ErrorMessage = "وحدة القياس مطلوبة")]
        [Display(Name = "وحدة القياس")]
        public int UOMID { get; set; }
        
        [Display(Name = "اسم وحدة القياس")]
        public string UOMName { get; set; }
        
        [Display(Name = "رمز وحدة القياس")]
        public string UOMSymbol { get; set; }
        
        [Display(Name = "المستخدم المسؤول")]
        public string ActionByUser { get; set; }
        
        [Display(Name = "نوع العملية")]
        public byte ActionType { get; set; }
        
        [Display(Name = "تاريخ العملية")]
        public DateTime ActionDate { get; set; }
    }
}
