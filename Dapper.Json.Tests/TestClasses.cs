using System.Collections.Generic;

namespace Dapper.Json.Tests
{
    public class Parent
    {
        public Parent(bool testData)
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

        public Parent()
        {
            Children = new List<Child>();
            Sibling = new Sibling();
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
}