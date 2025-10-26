using SharedModels.EF.Models;
using System;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderItemAddedEventArgs : EventArgs
    {
        public clsImportOrderItem ImportOrderItem { get; }

        public ImportOrderItemAddedEventArgs(clsImportOrderItem importOrderItem)
        {
            ImportOrderItem = importOrderItem;
        }
    }
}
