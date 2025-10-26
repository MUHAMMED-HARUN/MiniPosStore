using SharedModels.EF.Models;
using System;

namespace BAL.Events.OrderEvents
{
    public class OrderItemUpdatedEventArgs : EventArgs
    {
        public clsOrderItem OldOrderItem { get; }
        public clsOrderItem NewOrderItem { get; }

        public OrderItemUpdatedEventArgs(clsOrderItem oldOrderItem, clsOrderItem newOrderItem)
        {
            OldOrderItem = oldOrderItem;
            NewOrderItem = newOrderItem;
        }
    }
}

