using SharedModels.EF.DTO;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;

namespace BAL.Events.OrderEvents
{
    public class OrderDeletedEventArgs : EventArgs
    {
        public OrderDTO Order { get; }
        public List<clsOrderItem> OrderItems { get; }

        public OrderDeletedEventArgs(OrderDTO order, List<clsOrderItem> orderItems)
        {
            Order = order;
            OrderItems = orderItems ?? new List<clsOrderItem>();
        }
    }
}

