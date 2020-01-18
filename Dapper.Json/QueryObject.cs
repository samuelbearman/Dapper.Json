using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Json
{
    public class QueryObject
    {
        public string ChildTableName { get; set; }
        public string ChildAliasName { get; set; }
        public string ParentTableName { get; set; }
        public string ParentAliasName { get; set; }
        public string ParentPropertyName { get; set; }
        public string JoiningColumnName { get; set; }

    }
}
