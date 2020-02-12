using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    public static class CustomExtensions
    {
        public static int DeleteById<T>(this SqlConnection cnn, T entity)
        {
            var tableName = nameof(T);
            var tableId = tableName + "Id";
            var idValue = typeof(T).GetProperty(tableId);

            if (idValue == null)
                throw new Exception($"Improper Name for primary Id: {idValue}");

            var sql = $@"delete from {tableName} where {tableId} = {idValue}";

            var result = cnn.Execute(sql);

            return result;
        }

        public static async Task<int> DeleteByIdAsync<T>(this SqlConnection cnn, T entity)
        {
            var tableName = nameof(T);
            var tableId = tableName + "Id";
            var idValue = typeof(T).GetProperty(tableId);

            if (idValue == null)
                throw new Exception($"Improper Name for primary Id: {idValue}");

            var sql = $@"delete from {tableName} where {tableId} = {idValue}";

            var result = await cnn.ExecuteAsync(sql);

            return result;
        }


        public static void NaiveUpdate<T>(this SqlConnection cnn, T entity)
        {
            var tableName = typeof(T).Name + "s";
            var tableId = typeof(T).Name + "Id";
            var idValue = typeof(T).GetProperty(tableId).GetValue(entity);

            if (idValue == null)
                throw new Exception($"Improper Name for primary Id: {idValue}");

            var primitiveProperties = new Dictionary<PropertyInfo, string>();

            var properties = typeof(T).GetProperties();

            var parameterObject = new ExpandoObject() as IDictionary<string, object>;

            foreach(var property in properties)
            {
                if(property.PropertyType == typeof(string) || !(typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.PropertyType.IsClass || property.Name == typeof(T).Name + "Id"))
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(entity);
                    parameterObject.Add(propertyName, propertyValue);
                    primitiveProperties.Add(property, $"{propertyName} = @{propertyName}");
                }
            }

            var sql = $@"update {tableName} set ";

            for (int i = 0; i < primitiveProperties.Count(); i++)
            {
                if(i == primitiveProperties.Count() - 1)
                    sql += primitiveProperties.ElementAt(i).Value;
                else
                    sql += primitiveProperties.ElementAt(i).Value + ",";
            }

            sql += $" where {tableId} = {idValue}";

            cnn.Execute(sql, parameterObject);
        }
    }
}
