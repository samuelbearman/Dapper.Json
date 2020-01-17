using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dapper.Json.QueryBuilder
{
    public static class RawSql
    {
        public static string Build<T>(T template, bool isAggregate = false)
        {
            // Assume being passed the Aggreagte 
            List<QueryObject> withOneToMany = new List<QueryObject>();
            List<QueryObject> withOneToOne = new List<QueryObject>();

            string result = "";

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name.Contains("Id") && property.Name != typeof(T).Name + "Id")
                {
                    // One To One Id
                    var childClass = properties.SingleOrDefault(x => x.Name == property.Name.Remove(property.Name.Length - 2) && x.GetType().IsClass);

                    var childProperties = childClass.PropertyType.GetProperties();
                    string nestedQuery = "";

                    foreach (var childProperty in childProperties)
                    {
                        nestedQuery = Build(childProperty);
                    }

                    string parentAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                    string childAlias = string.Concat(property.Name.Remove(property.Name.Length - 2).Where(c => c >= 'A' && c <= 'Z'));

                    var queryObj = new QueryObject()
                    {
                        ChildAliasName = childAlias,
                        ParentAliasName = parentAlias,
                        ChildTableName = property.Name.Remove(property.Name.Length - 2) + "s",
                        JoiningColumnName = property.Name,
                        ParentPropertyName = property.Name.Remove(property.Name.Length - 2),
                    };

                    withOneToOne.Add(queryObj);

                    result = OneToOneQueryBlock(queryObj, nestedQuery);
                }

                //if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                //{
                //    // One To Many Id
                //    var childClass = property.PropertyType.GetGenericArguments()[0];
                //    //var childClass = properties.SingleOrDefault(x => x.Name == property.Name && x.GetType().IsClass);

                //    var childProperties = childClass.GetProperties();
                //    string nestedQuery = "";

                //    foreach (var childProperty in childProperties)
                //    {
                //        nestedQuery = Build(childProperty);
                //    }

                //    string parentAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                //    string childAlias = string.Concat(property.Name.Remove(property.Name.Length - 2).Where(c => c >= 'A' && c <= 'Z'));

                //    var queryObj = new QueryObject()
                //    {
                //        ChildAliasName = childAlias,
                //        ParentAliasName = parentAlias,
                //        ChildTableName = property.Name.Remove(property.Name.Length - 2) + "s",
                //        JoiningColumnName = property.Name,
                //        ParentPropertyName = property.Name.Remove(property.Name.Length - 2),
                //    };

                //    withOneToMany.Add(queryObj);

                //    result = OneToManyQueryBlock(queryObj, nestedQuery);
                //}
            }

            if (isAggregate)
            {
                string tableAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                result = AggregateQueryBlock(typeof(T).Name + "s", tableAlias, result);
            }

            return result;
        }

        private static string AggregateQueryBlock(string tableName, string tableAlias, string nestedBlocks)
        {
            string aggregateQuery;

            if (string.IsNullOrWhiteSpace(nestedBlocks))
            {
                aggregateQuery = $@"
                select {tableAlias}.*
                from {tableName} {tableAlias}
			    for 
                    json path, without_array_wrapper";
            }
            else
            {
                aggregateQuery = $@"
                select {tableAlias}.*,
			    {nestedBlocks}
                from {tableName} {tableAlias}
			    for 
                    json path, without_array_wrapper";
            }

            return aggregateQuery;
        }

        private static string OneToManyQueryBlock(QueryObject query, string nestedBlock)
        {
            string oneToManyQuery;

            if (string.IsNullOrWhiteSpace(nestedBlock))
            {
                oneToManyQuery = $@"json_query((
                select {query.ChildAliasName}.*
			    from {query.ChildTableName} {query.ChildAliasName}
			    where {query.ParentAliasName}.{query.JoiningColumnName} = {query.ChildAliasName}.{query.JoiningColumnName}
			    for json path
                )) as [{query.ParentPropertyName}]";
            }
            else
            {
                oneToManyQuery = $@"json_query((
                select {query.ChildAliasName}.*,
                {nestedBlock}
			    from {query.ChildTableName} {query.ChildAliasName}
			    where {query.ParentAliasName}.{query.JoiningColumnName} = {query.ChildAliasName}.{query.JoiningColumnName}
			    for json path
                )) as [{query.ParentPropertyName}]";
            }

            return oneToManyQuery;
        }

        private static string OneToOneQueryBlock(QueryObject query, string nestedBlock)
        {
            string oneToOneQuery;

            if (string.IsNullOrWhiteSpace(nestedBlock))
            {
                oneToOneQuery = $@"json_query((
				select {query.ChildAliasName}.*
				from {query.ChildTableName} {query.ChildAliasName}
				where {query.ParentAliasName}.{query.JoiningColumnName} = {query.ChildAliasName}.{query.JoiningColumnName}
				for json path, without_array_wrapper
			    )) as [{query.ParentPropertyName}]";
            }
            else
            {
                oneToOneQuery = $@"json_query((
				select {query.ChildAliasName}.*,
                {nestedBlock}
				from {query.ChildTableName} {query.ChildAliasName}
				where {query.ParentAliasName}.{query.JoiningColumnName} = {query.ChildAliasName}.{query.JoiningColumnName}
				for json path, without_array_wrapper
			    )) as [{query.ParentPropertyName}]";
            }

            return oneToOneQuery;
        }
    }
}