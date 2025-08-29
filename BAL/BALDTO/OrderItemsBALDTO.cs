using BAL.CustomAttributes;
using DAL.EF.DTO;
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
    }

}
