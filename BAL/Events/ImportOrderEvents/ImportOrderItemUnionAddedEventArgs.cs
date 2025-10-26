using SharedModels.EF.DTO;
using System;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderItemUnionAddedEventArgs : EventArgs
    {
        public ImportOrderItemUnionDTO ImportOrderItem { get; }
        
        public ImportOrderItemUnionAddedEventArgs(ImportOrderItemUnionDTO importOrderItem)
        {
            ImportOrderItem = importOrderItem ?? throw new ArgumentNullException(nameof(importOrderItem));
        }
    }
}
