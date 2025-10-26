using SharedModels.EF.Models;
using System;

namespace BAL.Events.OrderEvents
{
    public class OrderItemAddedEventArgs : EventArgs
    {
        public clsOrderItem OrderItem { get; }

        public OrderItemAddedEventArgs(clsOrderItem orderItem)
        {
            OrderItem = orderItem;
        }
    }
}

