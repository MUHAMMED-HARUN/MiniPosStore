using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.CustomAttributes
{
    public class IsPriceAdjustmentInRangeAttribute: ValidationAttribute
    {
        public string Message = "القيمة غير صالحة";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
        
            var f = (OrderItemsDTO)validationContext.ObjectInstance;
            if (value==null ||(float)value <= f.SellingPrice*f.Quantity)
                return ValidationResult.Success;
            else
                return new ValidationResult(Message);
        }
    }
}
