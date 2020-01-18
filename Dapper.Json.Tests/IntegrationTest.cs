using System.Data.SqlClient;
using Shouldly;
using Xunit;

namespace Dapper.Json.Tests
{
    public class IntegrationTest
    {
        [Fact]
        public void SqlQueryTest()
        {
            using(var conn = new SqlConnection("Server=DESKTOP-SJ0E2JG\\TESTING;Initial Catalog=Dapper.Json;Integrated Security=true;"))
            {
                SqlMapper.AddTypeHandler(new TypeHandler<Parent>());
                
                var sql = JsonSql.Build<Parent>(true);
                var result = conn.QueryFirstOrDefault<Parent>(sql);

                result.ShouldNotBeNull();
                result.Children.Count.ShouldBeGreaterThanOrEqualTo(1);
            }
        }
    }
}