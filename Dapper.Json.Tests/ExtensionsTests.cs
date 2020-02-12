using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;
using Dapper.Json.Extensions;
using Shouldly;
using Dapper.Extensions;

namespace Dapper.Json.Tests
{
    public class ExtensionsTests
    {
        private string connString = "Server=localhost;Initial Catalog=Dapper.Json;Integrated Security=true;";
        [Fact]
        public void CustomExtensions_NaiveUpdate_ShouldUpdate()
        {
            using (var conn = new SqlConnection(connString))
            {
                var result = conn.QuerySingleOrDefault<Parent>("select * from Parents where ParentId = 1");

                result.ShouldNotBeNull();

                var originalDescription = result.Description;

                result.Description = "This has been updated";

                conn.NaiveUpdate(result);

                result = conn.QuerySingleOrDefault<Parent>("select * from Parents where ParentId = 1");

                result.Description.ShouldNotBe(originalDescription);
            }
        }
    }
}
