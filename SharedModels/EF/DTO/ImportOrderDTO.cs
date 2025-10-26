using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.EF.DTO
{
    public   class ImportOrderDTO
    {
        public int ImportOrderID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierAddress { get; set; }
        public float TotalAmount { get; set; }
        public float PaidAmount { get; set; }
        public DateTime ImportDate { get; set; }
        public byte PaymentStatus { get; set; }
        public string PaymentStatusText { get; set; }
        public string ActionByUser { get; set; }
        public string UserName { get; set; }
        public byte ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public int ItemsCount { get; set; }
        public List<ImportOrderItemDTO> ImportOrderItems { get; set; } = new List<ImportOrderItemDTO>();
        public List<ImportOrderItemUnionDTO> UnionItems { get; set; } = new List<ImportOrderItemUnionDTO>();
    }
}

