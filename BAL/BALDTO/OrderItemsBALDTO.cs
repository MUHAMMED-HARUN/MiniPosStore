using BAL.CustomAttributes;
using SharedModels.EF.DTO;
using System.ComponentModel.DataAnnotations;

namespace BAL.BALDTO
{
    public class OrderItemsBALDTO : OrderItemsDTO
    {
        [IsQuantityAvailable(Message = "ÇáßãíÉ ÇáãØáæÈÉ ÛíÑ ãÊæÝÑÉ")]
        public new float Quantity
        {
            get => base.Quantity;
            set => base.Quantity = value;
        }
        [IsPriceAdjustmentInRange(Message = "ÊÚÏíá ÇáÓÚÑ ÛíÑ ÕÇáÍ")]
        public new float? PriceAdjustment
        {
            get => base.PriceAdjustment;
            set => base.PriceAdjustment = value;
        }
    }

}
