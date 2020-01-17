using System.Collections;
using System.Collections.Generic;

namespace Dapper.Json.QueryBuilder
{
    public static class RawSql
    {

        public static string Build<T>(T template)
        {
            // Assume being passed the Aggreagte 
            List<string> withOneToMany = new List<string>();
            List<string> withOneToOne = new List<string>();

            var properties = typeof(T).GetProperties();
            foreach(var property in properties)
            {
                if (property.Name.Contains("Id") && property.Name != typeof(T).Name + "Id")
                {

                    withOneToOne.Add(property.Name);
                }

                if(property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    withOneToMany.Add(property.Name);
                }
            }

            return "";
        }

        private static string OneToOneQueryBlock(string name)
        {
            // 0: actual sql table name
            // 1: sql alias of the table
            // 2: parent alias of the joining table
            // 3: joining column name
            // 4: name of the parent property's property 
            string childTableName = "BucketMappings";
            string childTableAlias = "BM";
            string parentTableAlias = "BD";
            string joiningColumnName = "BucketMappingId";
            string parentPropertyName = "BucketMapping";


            var oneToOnequery = $@"json_query((
				select {childTableAlias}.*
				from {childTableName} {childTableAlias}
				where {parentTableAlias}.{joiningColumnName} = {childTableAlias}.{joiningColumnName}
				for json path, without_array_wrapper
			)) as [{parentPropertyName}]";

            return oneToOnequery;
        }

        private static string OneToOneQueryBlock(QueryObject query)
        {
            var oneToOnequery = $@"json_query((
				select {query.ChildTableName}.*
				from {query.ChildTableName} {query.ChildAliasName}
				where {query.ParentAliasName}.{query.JoiningColumnName} = {query.ChildAliasName}.{query.JoiningColumnName}
				for json path, without_array_wrapper
			)) as [{query.ParentPropertyName}]";

            return oneToOnequery;
        }
    }
}