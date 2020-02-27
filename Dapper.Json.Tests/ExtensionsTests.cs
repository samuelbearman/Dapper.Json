using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;
using Dapper.Json.Extensions;
using Shouldly;
using Dapper.Extensions;
using System.Threading.Tasks;

namespace Dapper.Json.Tests
{
    public class ExtensionsTests
    {
        private readonly string connString = "Server=localhost;Initial Catalog=Dapper.Json;Integrated Security=true;";

        [Fact]
        public async Task CustomExtensions_NaiveUpdateAsync_ShouldUpdateDescription()
        {
            using (var conn = new SqlConnection(connString))
            {
                var result = conn.QuerySingleOrDefault<Parent>("select * from Parents where ParentId = 1");

                result.ShouldNotBeNull();

                var originalDescription = result.Description;

                result.Description = "This has been updated";

                await conn.NaiveUpdateAsync(result);

                result = conn.QuerySingleOrDefault<Parent>("select * from Parents where ParentId = 1");

                result.Description.ShouldNotBe(originalDescription);
            }
        }

        [Fact]
        public async Task CustomExtensions_InsertAsync_ShouldInsertNewRecord()
        {
            using (var conn = new SqlConnection(connString))
            {
                var parent = new Parent()
                {
                    Description = "Hello",
                    SiblingId = 1
                };

                await conn.InsertAsync(parent);
            }
        }
    }
}
