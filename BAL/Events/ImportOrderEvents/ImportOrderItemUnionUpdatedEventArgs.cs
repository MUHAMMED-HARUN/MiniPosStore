using SharedModels.EF.DTO;
using System;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderItemUnionUpdatedEventArgs : EventArgs
    {
        public ImportOrderItemUnionDTO OldImportOrderItem { get; }
        public ImportOrderItemUnionDTO NewImportOrderItem { get; }
        
        public ImportOrderItemUnionUpdatedEventArgs(ImportOrderItemUnionDTO oldImportOrderItem, ImportOrderItemUnionDTO newImportOrderItem)
        {
            OldImportOrderItem = oldImportOrderItem ?? throw new ArgumentNullException(nameof(oldImportOrderItem));
            NewImportOrderItem = newImportOrderItem ?? throw new ArgumentNullException(nameof(newImportOrderItem));
        }
    }
}
