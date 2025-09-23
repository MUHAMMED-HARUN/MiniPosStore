using DAL;
using DAL.EF.AppDBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.EF.DTO;
using SharedModels.EF.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class clsDALUtil
    {
        public static bool IsSimpleType(Type type)
        {
            // لو النوع Nullable<T> يرجع الـ T
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsPrimitive                // int, bool, char, float, ...
                || type.IsEnum                     // enum
                || type == typeof(string)          // string
                || type == typeof(DateTime)        // DateTime
                || type == typeof(decimal);        // decimal
        }

        public static string GetSqlWhereString<T>()
        {
            var properties = typeof(T).GetProperties();

            var paramList = properties.Select(p => p.Name);
            var Param = paramList.Select(p => p + "=@" + p);

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
            var propl = propList.Where(p => IsSimpleType(p.PropertyType))       
            .Select(p => p.Name).ToHashSet();

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
        public static void MapToClass<T>(DbDataReader reader, ref T obj) where T : class
        {
            if (obj == null)
                throw new NullReferenceException();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            HashSet<string> readerColumns = GetReaderColumn(reader);
            foreach (var prop in properties)
            {
                if (!prop.CanWrite)
                    continue;
                string propName;
                var descrAttr = prop.GetCustomAttribute<DescriptionAttribute>();

                if (descrAttr != null) 
                    propName = descrAttr.Description;
                else
                    propName = prop.Name;
                try
                {

                    if (readerColumns.Contains(propName))
                    {
                        var Val = reader[propName];
                        prop.SetValue(obj, Convert.ChangeType(Val, prop.PropertyType));
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    continue;
                }
            }
        }
        public static async Task<List<T>> ExecuteFilterCommands<T, F>(AppDBContext _Context, F filter, string TVFName) where T : class
        {
            if (filter == null)
                return null;
            {

                string Query = @$"select * from {TVFName} ( {clsDALUtil.GetSqlPrameterString<F>()})";

                using (var connection = _Context.Database.GetDbConnection().CreateCommand())
                {
                    connection.CommandText = Query;
                    connection.CommandType = System.Data.CommandType.Text;

                    if (connection.Connection.State != System.Data.ConnectionState.Open)
                        connection.Connection.Open();

                    var arr = clsDALUtil.GetSqlPrameters<F>(filter).ToArray();
                    connection.Parameters.AddRange(arr);

                    List<T> listResult = new List<T>();
                    try
                    {
                        using (var reader = connection.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Obj = Activator.CreateInstance<T>();
                                clsDALUtil.MapToClass<T>(reader, ref Obj);
                                listResult.Add(Obj);
                            }
                        }
                    }
                    catch (SqlException s)
                    {

                    }

                    return listResult;
                }
            }
        }
        public static async Task<T> ExecuteSPCommands<T, SP>(AppDBContext _Context, SP SPobj, string SPName)
        {
            if (SPobj == null)
                return default;
            {

                string Query = @$"GetNetProfit";

                using (var connection = _Context.Database.GetDbConnection().CreateCommand())
                {
                    connection.CommandText = Query;
                    connection.CommandType = System.Data.CommandType.StoredProcedure;

                    if (connection.Connection.State != System.Data.ConnectionState.Open)
                        connection.Connection.Open();

                    var arr = clsDALUtil.GetSqlPrameters<SP>(SPobj).ToArray();  
                    connection.Parameters.AddRange(arr);

                    var Obj = Activator.CreateInstance<T>();

                    try
                    {
                        object? result = await connection.ExecuteScalarAsync();
                        if (result != DBNull.Value)
                            Obj = (T)result;
                        else
                            Obj = default;
                    }
                    catch (SqlException s)
                    {
                        int ss = 2;
                    }

                    return Obj;
                }
            }
        }




    }
}