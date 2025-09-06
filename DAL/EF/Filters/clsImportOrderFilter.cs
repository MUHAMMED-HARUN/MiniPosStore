using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Filters
{
    public class clsImportOrderFilter
    {
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
    }
}
