using Dapper.Json.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Dapper.Json.Tests
{
    public class RawSqlTests
    {
        public class Parent
        {
            public Parent()
            {
                ParentId = 1;
                Description = "Test";
                Children = new List<Child>()
                {
                    new Child()
                    {
                        ChildId = 45,
                        Description = "This is a test"
                    },
                    new Child()
                    {
                        ChildId = 48,
                        Description = "This is a test"
                    }
                };
                Sibling = new Sibling()
                {
                    SiblingId = 78,
                    Description = "Sibling Test",
                };
            }

            public int ParentId { get; set; }
            public string Description { get; set; }
            public List<Child> Children { get; set; }
            public int SiblingId { get; set; }
            public Sibling Sibling { get; set; }

        }
        public class Sibling
        {
            public int SiblingId { get; set; }
            public string Description { get; set; }
        }

        public class Child
        {
            public int ChildId { get; set; }
            public string Description { get; set; }
        }

        [Fact]
        public void ShouldReturn()
        {
            Parent testClass = new Parent();

            //var intTets = RawSql.Build(2);

            var result = RawSql.Build(testClass, true);

            var test = "";
        }
    }
}
