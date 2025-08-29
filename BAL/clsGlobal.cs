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
       public enum enPaymentStatus
        {
            Completed = 1,
            Pending = 2,
            Failed = 3,
            Canceled = 4
        }
        public static SortedDictionary<int, string> GetPaymentStatusList()
        {
            return new SortedDictionary<int, string>
        {
            { (int)enPaymentStatus.Completed, "مكتمل" },
            { (int)enPaymentStatus.Pending, "معلق" },
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
            { (int)enCurrencyType.TRY, "ليرة تركية (TRY)" },
            { (int)enCurrencyType.USD, "دولار أمريكي (USD)" },
            { (int)enCurrencyType.EUR, "يورو (EUR)" }
        };
        }
    }

}
