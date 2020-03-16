using Dapper.Json.Crud;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Dapper.Json.Tests;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Shouldly;

namespace Dapper.Json.Tests
{
    public class StoredProcedureTests
    {
        private readonly string connString = "Server=localhost;Initial Catalog=Dapper.Json;Integrated Security=true;";

        [Fact]
        public void StoredProcedures_ShouldCreateGetCorrectly()
        {
            StoredProceduresJson<Parent>.Init(connString);

            using (var conn = new SqlConnection(connString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Id", 1);

                var dbResult = conn.ExecuteScalar<string>(
                    sql: "Web.Get_Parent",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                var result = JsonConvert.DeserializeObject<Parent>(dbResult);

                result.ShouldNotBeNull();

            }
        }

        [Fact]
        public void Test()
        {
            var test = JsonStoredProcedures.BuildJsonSelectSql<Parent>();
        }
    }
}
