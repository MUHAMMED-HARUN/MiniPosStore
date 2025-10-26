using SharedModels.EF.Models;
using System;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderItemDeletedEventArgs : EventArgs
    {
        public clsImportOrderItem ImportOrderItem { get; }

        public ImportOrderItemDeletedEventArgs(clsImportOrderItem importOrderItem)
        {
            ImportOrderItem = importOrderItem;
        }
    }
}
