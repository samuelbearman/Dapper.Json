using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper.Json.Crud
{
    public static class JsonStoredProcedures
    {
        public static string BuildJsonSelectSql<T>()
        {
            string sql = "select * from openjson(@json) with (";

            var properties = typeof(T).GetProperties();

            int count = 1;
            foreach (var property in properties)
            {
                // If is not a collection or another class
                // this is just a regular primitive property
                if (!(property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType)))
                {
                    if (!(property.PropertyType != typeof(string) && property.PropertyType.IsClass))
                    {
                        if (count != properties.Count() || count != 1)
                            sql += ",";
                        string match = SqlTypeMapping.MatchType(property);
                        sql += $" [{property.Name}] {match}";
                    }
                }
                count++;

            }

            sql += ")";

            return sql;
        }
    }
}
