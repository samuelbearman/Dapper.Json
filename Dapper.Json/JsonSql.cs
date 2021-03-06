using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Dapper.Json
{
    public static class JsonSql
    {
        public static string Build<T>(bool isAggregate = false, bool IsPrimitive = false, bool forStoredProc = false)
        {
            string result = "";
            PropertyInfo[] properties = null;

            if (IsPrimitive)
                properties = new PropertyInfo[] { };
            else
                properties = typeof(T).GetProperties();

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
                        bool isPrimitive = IsSimple(childProperty.PropertyType);
                        nestedQuery = Build<T>(false, isPrimitive);
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

                    result = OneToOneQueryBlock(queryObj, nestedQuery, result);
                }

                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    // One To Many
                    var collectionType = property.PropertyType.GetGenericArguments()[0];

                    var collectionTypeProperties = collectionType.GetProperties();
                    string nestedQuery = "";

                    foreach (var childProperty in collectionTypeProperties)
                    {
                        bool isPrimitive = IsSimple(childProperty.PropertyType);
                        nestedQuery = Build<T>(false, isPrimitive);
                    }

                    string parentAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                    string childAlias = string.Concat(property.Name.Remove(property.Name.Length - 2).Where(c => c >= 'A' && c <= 'Z'));

                    var queryObj = new QueryObject()
                    {
                        ChildAliasName = childAlias,
                        ParentAliasName = parentAlias,
                        ChildTableName = property.Name,
                        JoiningColumnName = typeof(T).Name + "Id",
                        ParentPropertyName = property.Name,
                    };

                    result = OneToManyQueryBlock(queryObj, nestedQuery, result);
                }
            }

            if (isAggregate)
            {
                string tableAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                string sqlWhereClause = "";
                if (forStoredProc)
                {
                    var alias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                    sqlWhereClause = $"where {tableAlias}.{typeof(T).Name}Id = @Id";
                }
                
                result = AggregateQueryBlockSingle(typeof(T).Name + "s", tableAlias, result, sqlWhereClause);
            }

            return result;
        }
        public static string BuildCollection<T>(bool isAggregate = false, bool IsPrimitive = false)
        {
            string result = "";
            PropertyInfo[] properties = null;

            if (IsPrimitive)
                properties = new PropertyInfo[] { };
            else
                properties = typeof(T).GetProperties();

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
                        bool isPrimitive = IsSimple(childProperty.PropertyType);
                        nestedQuery = Build<T>(false, isPrimitive);
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

                    result = OneToOneQueryBlock(queryObj, nestedQuery, result);
                }

                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    // One To Many
                    var collectionType = property.PropertyType.GetGenericArguments()[0];

                    var collectionTypeProperties = collectionType.GetProperties();
                    string nestedQuery = "";

                    foreach (var childProperty in collectionTypeProperties)
                    {
                        bool isPrimitive = IsSimple(childProperty.PropertyType);
                        nestedQuery = Build<T>(false, isPrimitive);
                    }

                    string parentAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                    string childAlias = string.Concat(property.Name.Remove(property.Name.Length - 2).Where(c => c >= 'A' && c <= 'Z'));

                    var queryObj = new QueryObject()
                    {
                        ChildAliasName = childAlias,
                        ParentAliasName = parentAlias,
                        ChildTableName = property.Name,
                        JoiningColumnName = typeof(T).Name + "Id",
                        ParentPropertyName = property.Name,
                    };

                    result = OneToManyQueryBlock(queryObj, nestedQuery, result);
                }
            }

            if (isAggregate)
            {
                string tableAlias = string.Concat(typeof(T).Name.Where(c => c >= 'A' && c <= 'Z'));
                result = AggregateQueryBlockMultiple(typeof(T).Name + "s", tableAlias, result);
            }

            return result;
        }
        
        private static bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive ||
                typeInfo.IsEnum ||
                type.Equals(typeof(string)) ||
                type.Equals(typeof(decimal));
        }

        private static string AggregateQueryBlockMultiple(string tableName, string tableAlias, string nestedBlocks)
        {
            string aggregateQuery;

            if (string.IsNullOrWhiteSpace(nestedBlocks))
            {
                aggregateQuery = $@"
                select {tableAlias}.*
                from {tableName} {tableAlias}
			    for json path";
            }
            else
            {
                aggregateQuery = $@"
                select {tableAlias}.*,
			    {nestedBlocks}
                from {tableName} {tableAlias}
			    for json path";
            }

            return aggregateQuery;
        }

        private static string AggregateQueryBlockSingle(string tableName, string tableAlias, string nestedBlocks, string whereClause)
        {
            string aggregateQuery;

            if (string.IsNullOrWhiteSpace(nestedBlocks))
            {
                aggregateQuery = $@"
                select top 1 {tableAlias}.*
                from {tableName} {tableAlias}
                {whereClause}
			    for 
                    json path, without_array_wrapper";
            }
            else
            {
                aggregateQuery = $@"
                select top 1 {tableAlias}.*,
			    {nestedBlocks}
                from {tableName} {tableAlias}
                {whereClause}
			    for 
                    json path, without_array_wrapper";
            }

            return aggregateQuery;
        }

        private static string OneToManyQueryBlock(QueryObject query, string nestedBlock, string curentResult)
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

            if (!string.IsNullOrWhiteSpace(curentResult))
            {
                curentResult += "," + oneToManyQuery;
                return curentResult;
            }

            return oneToManyQuery;
        }

        private static string OneToOneQueryBlock(QueryObject query, string nestedBlock, string curentResult)
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

            if (!string.IsNullOrWhiteSpace(curentResult))
            {
                curentResult += "," + oneToOneQuery;
                return curentResult;
            }

            return oneToOneQuery;
        }
    }
}