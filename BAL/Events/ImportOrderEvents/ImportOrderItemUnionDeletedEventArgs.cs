using SharedModels.EF.DTO;
using System;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderItemUnionDeletedEventArgs : EventArgs
    {
        public ImportOrderItemUnionDTO ImportOrderItem { get; }
        
        public ImportOrderItemUnionDeletedEventArgs(ImportOrderItemUnionDTO importOrderItem)
        {
            ImportOrderItem = importOrderItem ?? throw new ArgumentNullException(nameof(importOrderItem));
        }
    }
}
