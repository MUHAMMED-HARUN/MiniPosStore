using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class clsDALUtil
    {
        public static string GetSqlWhereString<T>()
        {
            var properties = typeof(T).GetProperties();

            var paramList = properties.Select(p =>p.Name);
            var Param=paramList.Select(p => p+"=@"+p);

            return string.Join(", ", Param);
        }
        public static string GetSqlPrameterString<T>()
        {
            var properties = typeof(T).GetProperties();

            var paramList = properties
     .Where(p =>
     {
         var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;

         return type.IsPrimitive               // int, bool, char, float, ...
             || type.IsEnum                    // enum
             || type == typeof(string)         // string
             || type == typeof(DateTime)       // DateTime
             || type == typeof(decimal);       // decimal
     }).Select(p => p.Name);
            var Param = paramList.Select(p => "@" + p);

            return string.Join(",", Param);
        }
        public static List<SqlParameter> GetSqlPrameters<T>(T obj)
        {
            List<SqlParameter> pramList = new List<SqlParameter>();

            var propList = typeof(T).GetProperties();
            var propl = propList.Where(p =>
            {
                var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;

                return type.IsPrimitive               // int, bool, char, float, ...
                    || type.IsEnum                    // enum
                    || type == typeof(string)         // string
                    || type == typeof(DateTime)       // DateTime
                    || type == typeof(decimal);       // decimal
            }).Select(p => p.Name).ToHashSet();

            foreach (var prop in propList)
            {
                if (!propl.Contains(prop.Name))
                    continue;
                var value = prop.GetValue(obj) ?? DBNull.Value;
                SqlParameter parameter = new SqlParameter($"@{prop.Name}", value);
                pramList.Add(parameter);
            }

            return pramList;
        }
        public static string GetSqlFields<t>()
        {
            var properties = typeof(t).GetProperties();
            var fieldList = properties.Select(p => p.Name);
            return string.Join(", ", fieldList);
        }
        public static HashSet<string> GetReaderColumn(DbDataReader reader)
        {
            return Enumerable.Range(0, reader.FieldCount)
                 .Select(i => reader.GetName(i))
                 .ToHashSet();
        }
        public static void MapToClass<T>(DbDataReader reader,ref T obj )where T :class
        {
            if (obj == null)
                throw new NullReferenceException();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            HashSet<string> readerColumns = GetReaderColumn(reader);
            foreach ( var prop in properties)
            {
                if (!prop.CanWrite)
                    continue;

                try
                {
                    
                    if (readerColumns.Contains(prop.Name))
                    {
                        var Val = reader[prop.Name];
                        prop.SetValue(obj, Convert.ChangeType(Val, prop.PropertyType));
                    }
                }
                catch(IndexOutOfRangeException e)
                {
                    continue;
                }
            }
        }
    }
}
