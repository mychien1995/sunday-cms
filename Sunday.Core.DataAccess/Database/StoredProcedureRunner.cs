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
            using (var connection = GetConnection())
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

        public async Task<IEnumerable<T>> ExecuteAsync<T>(string storeName, object parameter = null)
        {
            using (var connection = GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var result = await connection.QueryAsync<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<IEnumerable<object>>> ExecuteMultiple(string storeName, Type[] returnTypes, object parameter = null)
        {
            var finalResult = new List<IEnumerable<object>>();
            using (var connection = GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var queryResult = await connection.QueryMultipleAsync(storeName, parameter, commandType: CommandType.StoredProcedure);
                foreach(var type in returnTypes)
                {
                    var typeResult = await queryResult.ReadAsync(type);
                    finalResult.Add(typeResult);
                }
                return finalResult;
            }
        }

        public IEnumerable<T> Execute<T>(string storeName, object parameter = null)
        {
            using (var connection = GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var result = connection.Query<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        protected virtual SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
