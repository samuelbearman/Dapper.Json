using Dapper.Json.QueryBuilder;
using System.Collections.Generic;
using Xunit;

namespace Dapper.Json.UnitTests
{
    public class RawSqlTests
    {
        public class Test
        {
            public Test()
            {
                TestId = 1;
                Description = "Test";
                Amount = 200m;
                Details = new List<Detail>()
                {
                    new Detail()
                    {
                        DetailId = 45,
                        Amount = 100m
                    },
                    new Detail()
                    {
                        DetailId = 48,
                        Amount = 100m
                    }
                };
            }

            public int TestId { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public List<Detail> Details { get; set; }
            public int TestTypeId { get; set; }
            public TestType TestType { get; set; }

        }
        public class TestType
        {
            public int TestTypeId { get; set; }
            public string Description { get; set; }
        }

        public class Detail
        {
            public int DetailId { get; set; }
            public decimal Amount { get; set; }
        }

        [Fact]
        public void ShouldReturn()
        {
            Test testClass = new Test();

            var result = RawSql.Build(testClass);
        }
    }
}