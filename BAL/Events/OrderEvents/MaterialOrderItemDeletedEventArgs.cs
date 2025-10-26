using SharedModels.EF.Models;
using System;

namespace BAL.Events.OrderEvents
{
    public class MaterialOrderItemDeletedEventArgs : EventArgs
    {
        public clsRawMaterialOrderItem OrderItem { get; }

        public MaterialOrderItemDeletedEventArgs(clsRawMaterialOrderItem orderItem)
        {
            OrderItem = orderItem;
        }
    }
}


