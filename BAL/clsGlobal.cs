using DAL.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class clsGlobal
    {
        public enum enSaveMode
        {
            Add = 1,
            Update = 2
        }
        public enum enActionType
        {
            Add = 1,
            Update = 2,
            Delete = 3,
            QuantityChange ,
        }
        public enum enPaymentStatus
        {
            Completed = 1,
            Pending = 2,
            PendingForPayment = 3,
            Failed = 4,
            Canceled = 5
        }
        public static SortedDictionary<int, string> GetPaymentStatusList()
        {
            return new SortedDictionary<int, string>
        {
            { (int)enPaymentStatus.Completed, "مكتمل" },
            { (int)enPaymentStatus.Pending, "معلق" },
            { (int)enPaymentStatus.PendingForPayment, "معلق للدفع" },
            { (int)enPaymentStatus.Failed, "فشل" },
            { (int)enPaymentStatus.Canceled, "ملغى" }
        };
        }
        public enum enCurrencyType
        {
            TRY = 1,
            USD,
            EUR
        }
        public static SortedDictionary<int, string> GetCurrencyTypeList()
        {
            return new SortedDictionary<int, string>
        {
            { (int)enCurrencyType.TRY, "TRY" },
            { (int)enCurrencyType.USD, "دولار أمريكي (USD)" },
            { (int)enCurrencyType.EUR, "يورو (EUR)" }
        };
        }
        public static string GetCurrencyTypeString(int CurrecyType)
        {
             if( GetCurrencyTypeList().TryGetValue(CurrecyType,out string Value))
                return Value;
            else
                return string.Empty;
        }
    }

}
