using SharedModels.EF.Models;
using System;

namespace BAL.Events.OrderEvents
{
    public class MaterialOrderItemUpdatedEventArgs : EventArgs
    {
        public clsRawMaterialOrderItem OldOrderItem { get; }
        public clsRawMaterialOrderItem NewOrderItem { get; }

        public MaterialOrderItemUpdatedEventArgs(clsRawMaterialOrderItem oldOrderItem, clsRawMaterialOrderItem newOrderItem)
        {
            OldOrderItem = oldOrderItem;
            NewOrderItem = newOrderItem;
        }
    }
}


