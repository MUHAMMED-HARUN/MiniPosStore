using SharedModels.EF.Models;
using System;

namespace BAL.Events.OrderEvents
{
    public class MaterialOrderItemAddedEventArgs : EventArgs
    {
        public clsRawMaterialOrderItem OrderItem { get; }

        public MaterialOrderItemAddedEventArgs(clsRawMaterialOrderItem orderItem)
        {
            OrderItem = orderItem;
        }
    }
}


