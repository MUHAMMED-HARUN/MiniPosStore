using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public class RawMaterialDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float PurchasePrice { get; set; }
        public float AvailableQuantity { get; set; }
        public float ProductionLossQuantity { get; set; }
        public int UOMID { get; set; }
        public string UOMName { get; set; }
        public int CurrencyTypeID { get; set; }
        public int MaterialSupplier { get; set; }
        public float ReservedQuantity { get; set; }
        public string SupplierName { get; set; }
        public DateTime ActionDate { get; set; }
        public string UserID { get; set; }
    }
}
