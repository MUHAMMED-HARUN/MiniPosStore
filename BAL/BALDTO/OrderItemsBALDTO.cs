using BAL.CustomAttributes;
using SharedModels.EF.DTO;
using System.ComponentModel.DataAnnotations;

namespace BAL.BALDTO
{
    public class OrderItemsBALDTO : OrderItemsDTO
    {
        [IsQuantityAvailable(Message = "������ �������� ��� ������")]
        public new float Quantity
        {
            get => base.Quantity;
            set => base.Quantity = value;
        }
        [IsPriceAdjustmentInRange(Message = "����� ����� ��� ����")]
        public new float? PriceAdjustment
        {
            get => base.PriceAdjustment;
            set => base.PriceAdjustment = value;
        }
    }

}
