using SharedModels.EF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.Filters
{
    public class clsOrderFilter
    {
        public int? OrderID { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? StartTotalAmount { get; set; }
        public decimal? EndTotalAmount { get; set; }

        public decimal? StartPaidAmount { get; set; }
        public decimal? EndPaidAmount { get; set; }

        public string? PaymentStatus { get; set; }
        public string? ActionByUser { get; set; }
        public List<OrderDTO> orders { set; get; } = new List<OrderDTO>();
    }
}
