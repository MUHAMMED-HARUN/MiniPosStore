using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public   class clsImportOrderFilter
    {
        public int? @IOID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime? StartImportDate { get; set; }
        public DateTime? EndImportDate { get; set; }

        public decimal? StartTotalAmount { get; set; }
        public decimal? EndTotalAmount { get; set; }

        public decimal? StartPaidAmount { get; set; }
        public decimal? EndPaidAmount { get; set; }

        public string? PaymentStatus { get; set; }

        public int? StartItemsCount { get; set; }
        public int? EndItemsCount { get; set; }
     public   List <ImportOrderDTO> importOrders { set; get; } = new List<ImportOrderDTO>();
    }
}
