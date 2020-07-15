using Dapper;
using Microsoft.Extensions.Configuration;
using Sunday.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sunday.DataAccess.SqlServer
{
    [ServiceTypeOf(typeof(StoredProcedureRunner), LifetimeScope.Transient)]
    public class StoredProcedureRunner
    {
        private readonly string _connectionString;
        public StoredProcedureRunner(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SundayDB");
        }
        public void Execute(string storeName, params SqlParameter[] parameters)
        {
            using var connection = GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using var cmd = new SqlCommand();
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeName;
            cmd.ExecuteNonQuery();
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(string storeName, object parameter = null)
        {
            await using var connection = GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var result = await connection.QueryAsync<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<List<IEnumerable<object>>> ExecuteMultipleAsync(string storeName, Type[] returnTypes, object parameter = null)
        {
            var finalResult = new List<IEnumerable<object>>();
            await using var connection = GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var queryResult = await connection.QueryMultipleAsync(storeName, parameter, commandType: CommandType.StoredProcedure);
            foreach (var type in returnTypes)
            {
                var typeResult = await queryResult.ReadAsync(type);
                finalResult.Add(typeResult);
            }
            return finalResult;
        }

        public List<IEnumerable<object>> ExecuteMultiple(string storeName, Type[] returnTypes, object parameter = null)
        {
            using var connection = GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var queryResult = connection.QueryMultiple(storeName, parameter, commandType: CommandType.StoredProcedure);
            return returnTypes.Select(type => queryResult.Read(type)).ToList();
        }

        public IEnumerable<T> Execute<T>(string storeName, object parameter = null)
        {
            using var connection = GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var result = connection.Query<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
            return result;
        }

        protected virtual SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
