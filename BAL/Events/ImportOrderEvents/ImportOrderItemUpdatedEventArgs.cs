using SharedModels.EF.Models;
using System;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderItemUpdatedEventArgs : EventArgs
    {
        public clsImportOrderItem OldImportOrderItem { get; }
        public clsImportOrderItem NewImportOrderItem { get; }

        public ImportOrderItemUpdatedEventArgs(clsImportOrderItem oldImportOrderItem, clsImportOrderItem newImportOrderItem)
        {
            OldImportOrderItem = oldImportOrderItem;
            NewImportOrderItem = newImportOrderItem;
        }
    }
}
