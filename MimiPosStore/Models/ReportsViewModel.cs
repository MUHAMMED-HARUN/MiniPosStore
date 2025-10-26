using System;
using System.ComponentModel.DataAnnotations;

namespace MimiPosStore.Models
{
    /// <summary>
    /// Professional ViewModel for Reports with comprehensive data binding and validation
    /// Provides clean separation between UI and business logic
    /// </summary>
    public class ReportsViewModel
    {
        [Display(Name = "تاريخ البداية")]
        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-30); // Default to last 30 days

        [Display(Name = "تاريخ النهاية")]
        [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today;

        // Order Reports
        [Display(Name = "إجمالي المبيعات")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float OrderSales { get; set; }

        [Display(Name = "صافي ربح الطلبات")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float OrderNetProfit { get; set; }

        [Display(Name = "ديون الطلبات المتبقية")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float RemainingOrderDebt { get; set; }

        // Import Reports
        [Display(Name = "تكلفة طلبات الاستيراد")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float ImportOrderCost { get; set; }

        [Display(Name = "ديون الاستيراد المتبقية")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float RemainingImportDebt { get; set; }

        // Expense Reports
        [Display(Name = "إجمالي المصروفات")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float TotalExpenses { get; set; }

        // Net Profit
        [Display(Name = "صافي ربح المتجر")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public float StoreNetProfit { get; set; }

        // UI State Properties
        public bool IsLoading { get; set; } = false;
        public bool HasData { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        // Calculated Properties for UI
        public string DateRangeDisplay => $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}";
        public int DaysInRange => (int)(EndDate - StartDate).TotalDays + 1;
        
        // Performance Indicators
        public string ProfitabilityStatus
        {
            get
            {
                if (StoreNetProfit > 0) return "مربح";
                if (StoreNetProfit < 0) return "خسارة";
                return "متعادل";
            }
        }

        public string ProfitabilityColor
        {
            get
            {
                if (StoreNetProfit > 0) return "success";
                if (StoreNetProfit < 0) return "danger";
                return "warning";
            }
        }

        // Validation Methods
        public bool IsValidDateRange()
        {
            return StartDate <= EndDate && 
                   StartDate <= DateTime.Today && 
                   EndDate <= DateTime.Today;
        }

        public void ResetToDefaults()
        {
            StartDate = DateTime.Today.AddDays(-30);
            EndDate = DateTime.Today;
            IsLoading = false;
            HasData = false;
            ErrorMessage = string.Empty;
            
            // Reset all financial data
            OrderSales = 0;
            OrderNetProfit = 0;
            RemainingOrderDebt = 0;
            ImportOrderCost = 0;
            RemainingImportDebt = 0;
            TotalExpenses = 0;
            StoreNetProfit = 0;
        }
    }

    /// <summary>
    /// Request model for AJAX calls to get reports data
    /// </summary>
    public class ReportsRequest
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// Response model for AJAX calls returning reports data
    /// </summary>
    public class ReportsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ReportsViewModel Data { get; set; } = new ReportsViewModel();
    }
}
