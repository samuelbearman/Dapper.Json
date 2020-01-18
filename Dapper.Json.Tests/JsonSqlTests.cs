using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Dapper.Json.Tests
{
    public class JsonSqlTests
    {
        [Fact]
        public void ShouldReturnAString()
        {
            var result = JsonSql.Build<Parent>(true);

            result.ShouldNotBeNullOrEmpty();
        }
    }
}