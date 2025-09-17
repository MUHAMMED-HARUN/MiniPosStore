using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public   class ImportOrderItemDTO
    {
        public int ImportOrderID { get; set; }
        public int ImportOrderItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public float Quantity { get; set; }
        public float SellingPrice { get; set; }
        public float TotalItemAmount { get; set; }
        public string CurrencyType { get; set; }
        public string CurrencyName { get; set; }
        public string UOMName { get; set; }
        public string UOMSymbol { get; set; }
        public float ImportedQuantity { get; set; }
    }
}

