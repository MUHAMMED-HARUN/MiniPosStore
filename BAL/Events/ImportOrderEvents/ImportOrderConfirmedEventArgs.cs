using SharedModels.EF.Models;
using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;

namespace BAL.Events.ImportOrderEvents
{
    public class ImportOrderConfirmedEventArgs : EventArgs
    {
        public clsImportOrder ImportOrder { get; }
        public List<ImportOrderItemUnionDTO> Items { get; }
        
        public ImportOrderConfirmedEventArgs(clsImportOrder importOrder, List<ImportOrderItemUnionDTO> items)
        {
            ImportOrder = importOrder;
            Items = items ?? new List<ImportOrderItemUnionDTO>();
        }
    }
}
