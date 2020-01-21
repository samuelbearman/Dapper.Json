using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dapper.Json.Extensions
{
    public static class SqlJsonMapper
    {
        public static IEnumerable<T> QueryJson<T>(this SqlConnection cnn, string sqlFilter = "")
        {
            SqlMapper.AddTypeHandler(typeof(T), new JsonCollectionTypeHandler<T>());
            // SqlMapper.AddTypeHandler(new TypeHandler<T>());

            var sql = JsonSql.BuildCollection<T>(true, false);
            var result = cnn.Query<T>(sql);

            SqlMapper.ResetTypeHandlers();
            return result;
        }

        public static T QuerySingleJson<T>(this SqlConnection cnn, string sqlFilter = "")
        {
            SqlMapper.AddTypeHandler(new JsonTypeHandler<T>());

            var sql = JsonSql.Build<T>(true, false);
            var result = cnn.QuerySingle<T>(sql);

            SqlMapper.ResetTypeHandlers();
            return result;
        }
    }
}