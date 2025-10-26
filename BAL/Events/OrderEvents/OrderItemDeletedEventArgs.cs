using SharedModels.EF.Models;
using System;

namespace BAL.Events.OrderEvents
{
    public class OrderItemDeletedEventArgs : EventArgs
    {
        public clsOrderItem OrderItem { get; }

        public OrderItemDeletedEventArgs(clsOrderItem orderItem)
        {
            OrderItem = orderItem;
        }
    }
}

