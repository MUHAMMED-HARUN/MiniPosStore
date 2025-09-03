using DAL.EF.DTO;

namespace BAL.BALDTO
{
    public class ImportOrderBALDTO : ImportOrderDTO
    {
        // يمكن إضافة خصائص إضافية خاصة بـ BAL هنا إذا لزم الأمر
        public string FormattedTotalAmount => $"{TotalAmount:N2} {clsGlobal.GetCurrencyTypeString(Convert.ToInt32( GetCurrencySymbol()))}";
        public string FormattedPaidAmount => $"{PaidAmount:N2} {clsGlobal.GetCurrencyTypeString(Convert.ToInt32(GetCurrencySymbol()))}";
        public string FormattedRemainingAmount => $"{(TotalAmount - PaidAmount):N2} {clsGlobal.GetCurrencyTypeString(Convert.ToInt32(GetCurrencySymbol()))}";
        public string FormattedImportDate => ImportDate.ToString("dd/MM/yyyy");
        //public string FormattedActionDate => ActionDate.ToString("dd/MM/yyyy HH:mm");
        public bool IsFullyPaid => PaidAmount >= TotalAmount;
        public bool IsPartiallyPaid => PaidAmount > 0 && PaidAmount < TotalAmount;
        public bool IsNotPaid => PaidAmount == 0;

        private string GetCurrencySymbol()
        {
        
                return ImportOrderItems?.FirstOrDefault()?.CurrencyType ?? "TRY";
        }
    }
}
