using System.Data.SqlClient;
using Shouldly;
using Xunit;
using Dapper.Json.Extensions;

namespace Dapper.Json.Tests
{
    public class IntegrationTest
    {
        [Fact]
        public void SqlJsonMapper_UnderTheHoodComponentsTest()
        {
            using(var conn = new SqlConnection("Server=DESKTOP-SJ0E2JG\\TESTING;Initial Catalog=Dapper.Json;Integrated Security=true;"))
            {
                SqlMapper.AddTypeHandler(new JsonTypeHandler<Parent>());
                
                var sql = JsonSql.Build<Parent>(true);
                var result = conn.QueryFirstOrDefault<Parent>(sql);

                result.ShouldNotBeNull();
                result.Children.Count.ShouldBeGreaterThanOrEqualTo(1);

                SqlMapper.ResetTypeHandlers();
            }
        }

        [Fact]
        public void SqlJsonMapper_ExtensionSingleTest()
        {
            using(var conn = new SqlConnection("Server=DESKTOP-SJ0E2JG\\TESTING;Initial Catalog=Dapper.Json;Integrated Security=true;"))
            {
                var result = conn.QuerySingleJson<Parent>();

                result.ShouldNotBeNull();
            }
        }

        [Fact]
        public void SqlJsonMapper_ExtensionMultipleTest()
        {
            using(var conn = new SqlConnection("Server=DESKTOP-SJ0E2JG\\TESTING;Initial Catalog=Dapper.Json;Integrated Security=true;"))
            {
                var result = conn.QueryJson<Parent>();

                result.ShouldNotBeNull();
            }
        }
    }
}