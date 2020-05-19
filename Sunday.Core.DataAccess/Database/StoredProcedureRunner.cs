using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.DataAccess.Database
{
    [ServiceTypeOf(typeof(StoredProcedureRunner), LifetimeScope.Transient)]
    public class StoredProcedureRunner
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public StoredProcedureRunner(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SundayDB");
        }
        public void Execute(string storeName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (var cmd = new SqlCommand())
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeName;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string storeName, object parameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var result = await connection.QueryAsync<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
