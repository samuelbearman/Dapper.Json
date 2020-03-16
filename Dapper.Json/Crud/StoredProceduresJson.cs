using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Dapper.Json.Crud
{
    public static class StoredProceduresJson<T>
    {
        public static void Init(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                CheckForSchema(conn);
                Get(conn);
                Insert(conn);
            }
        }

        private static void CheckForSchema(SqlConnection conn)
        {
            string sql = $@"IF SCHEMA_ID('web') IS NULL BEGIN	
                            EXECUTE('CREATE SCHEMA [web]')
                            end";

            conn.Execute(sql);
        }

        private static void Get(SqlConnection conn)
        {
            string select = JsonSql.Build<T>(true, forStoredProc:true);
            var entityName = typeof(T).Name;
            string sql = $@"Create or alter procedure Web.Get_{entityName}
                            @Id Int
                            as
                            set nocount on;
                            {select}";

            conn.Execute(sql);
        }

        private static void Insert(SqlConnection conn)
        {
            string jsonSelect = "";
            string insert = "";
            string sourceSelect = "";
            var entityName = typeof(T).Name;
            string sql = $@"Create or alter procedure Web.Put_{entityName}
                            @Json Nvarchar(max)
                            as
                            set no count on;
                            declare @{entityName}Id int = next value for Sequences.{entityName}Id
                            with [source] as
                            (
                                select * from OpenJson(@Json) with (
                                    {jsonSelect}
                                )
                            )
                            insert into {entityName}s
                            (
                                {insert}
                            )
                            select 
                                {sourceSelect}
                            from 
                                [source]
                            ;
                            
                            Exec Web.Get_{entityName}";

            conn.Execute(sql);
        }
    }
}
