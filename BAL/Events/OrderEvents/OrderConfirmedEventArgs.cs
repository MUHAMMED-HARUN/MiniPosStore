using SharedModels.EF.Models;
using System;
using System.Collections.Generic;

namespace BAL.Events.OrderEvents
{
    public class OrderConfirmedEventArgs:EventArgs
    {
        public clsOrder Order { get; }
        public List<clsOrderItem> OrderItems { get; }
        public OrderConfirmedEventArgs(clsOrder order, List<clsOrderItem> orderItems)
        {
            Order = order;
            OrderItems = orderItems ?? new List<clsOrderItem>();
        }
    }
}
