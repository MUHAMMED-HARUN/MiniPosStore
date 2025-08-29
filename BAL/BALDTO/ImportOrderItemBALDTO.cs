using DAL.EF.DTO;

namespace BAL.BALDTO
{
    public class ImportOrderItemBALDTO : ImportOrderItemDTO
    {
        // خصائص إضافية خاصة بـ BAL
        public string FormattedSellingPrice => $"{SellingPrice:N2} {CurrencyType}";
        public string FormattedTotalItemAmount => $"{TotalItemAmount:N2} {CurrencyType}";
        public string FormattedQuantity => $"{Quantity:N2} {UOMSymbol}";
        public string FormattedImportedQuantity => $"{ImportedQuantity:N2} {UOMSymbol}";
        public string ProductDisplayName => !string.IsNullOrEmpty(ProductName) ? ProductName : "منتج غير محدد";
        public bool IsHighValueItem => TotalItemAmount > 1000; // يمكن تعديل هذا الحد حسب الحاجة
        public string ItemStatus => Quantity > 0 ? "متوفر" : "غير متوفر";
    }
}
