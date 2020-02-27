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


        public static async Task NaiveUpdateAsync<T>(this SqlConnection cnn, T entity)
        {
            var tableName = typeof(T).Name + "s";
            var tableId = typeof(T).Name + "Id";
            var idValue = typeof(T).GetProperty(tableId).GetValue(entity);

            if (idValue == null)
                throw new Exception($"Improper Name for primary Id: {idValue}");

            var primitiveProperties = new Dictionary<PropertyInfo, string>();

            var properties = typeof(T).GetProperties();

            var parameterObject = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || !(typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.PropertyType.IsClass || property.Name == typeof(T).Name + "Id"))
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
                if (i == primitiveProperties.Count() - 1)
                    sql += primitiveProperties.ElementAt(i).Value;
                else
                    sql += primitiveProperties.ElementAt(i).Value + ",";
            }

            sql += $" where {tableId} = {idValue}";

            await cnn.ExecuteAsync(sql, parameterObject);
        }

        public static async Task<int> InsertAsync<T>(this SqlConnection cnn, T entity)
        {
            var tableName = typeof(T).Name + "s";
            var tableId = typeof(T).Name + "Id";
            var idValue = typeof(T).GetProperty(tableId).GetValue(entity);

            if (!idValue.Equals(0))
                throw new Exception($"Cannot insert with an already assigned Id of {idValue}");

            var primitiveProperties = new Dictionary<PropertyInfo, string>();

            var properties = typeof(T).GetProperties();

            var parameterObject = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || !(typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.PropertyType.IsClass || property.Name == typeof(T).Name + "Id"))
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(entity);
                    parameterObject.Add(propertyName, propertyValue);
                    primitiveProperties.Add(property, $"@{propertyName}");
                }
                else if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    // This is a class collection
                    InsertManyToManyClasses(property, tableId, idValue);
                }
                else
                {

                }
            }

            var sql = $@"insert into {tableName} (";

            for (int i = 0; i < parameterObject.Count(); i++)
            {
                if (i == parameterObject.Count() - 1)
                    sql += parameterObject.ElementAt(i).Key + ")";
                else
                   sql += parameterObject.ElementAt(i).Key + ",";
            }

            sql += " values (";

            for (int i = 0; i < primitiveProperties.Count(); i++)
            {
                if (i == primitiveProperties.Count() - 1)
                    sql += primitiveProperties.ElementAt(i).Value + ")";
                else
                    sql += primitiveProperties.ElementAt(i).Value + ",";
            }

            sql += ";SELECT CAST(SCOPE_IDENTITY() as int)";

            var result = (await cnn.QueryAsync<int>(sql, parameterObject)).Single();
            return result;
        }

        private static string InsertManyToManyClasses(PropertyInfo collectionProperty, string tableId, object parentIdValue)
        {
            var collectionType = collectionProperty.PropertyType.GetGenericArguments()[0];
            var collectionTypeProperties = collectionType.GetProperties();

            // If collection contains the parentId, 
            if(collectionTypeProperties.Where(x => x.Name == tableId).Count() > 0)
            {
                // look at its current value foreach in collection
                //foreach (var childEntity in collectionType)
                //{

                //}

                //var currentParentIdValue = typeof(T).GetProperty(tableId).GetValue(collectionProperty);
            }

            return "";
        }
    }
}
