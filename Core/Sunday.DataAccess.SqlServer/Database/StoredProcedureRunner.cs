using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Sunday.Core;

namespace Sunday.DataAccess.SqlServer.Database
{
    [ServiceTypeOf(typeof(StoredProcedureRunner))]
    public class StoredProcedureRunner
    {
        private readonly string _connectionString;
        public StoredProcedureRunner(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SundayDB");
        }
        public async Task Execute(string storeName, params SqlParameter[] parameters)
        {
            await using var connection = GetConnection();
            await using var cmd = new SqlCommand();
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeName;
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(string storeName, object? parameter = null)
        {
            await using var connection = GetConnection();
            var result = await connection.QueryAsync<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task ExecuteAsync(string storeName, object? parameter = null)
        {
            await using var connection = GetConnection();
            await connection.ExecuteAsync(storeName, parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<IEnumerable<object>>> ExecuteMultipleAsync(string storeName, Type[] returnTypes, object? parameter = null)
        {
            var finalResult = new List<IEnumerable<object>>();
            await using var connection = GetConnection();
            var queryResult = await connection.QueryMultipleAsync(storeName, parameter, commandType: CommandType.StoredProcedure);
            foreach (var type in returnTypes)
            {
                var typeResult = await queryResult.ReadAsync(type);
                finalResult.Add(typeResult);
            }
            return finalResult;
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> ExecuteMultipleAsync<T1, T2>(string storeName, object? parameter = null)
        {
            var returnTypes = new[] { typeof(T1), typeof(T2) };
            var queryResult = await ExecuteMultipleAsync(storeName, returnTypes, parameter);
            if (queryResult.Count != 2) throw new InvalidOperationException($"Expect 2 return types, got {queryResult.Count}");
            return (queryResult[0].Select(item => (T1)item), queryResult[1].Select(item => (T2)item));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> ExecuteMultipleAsync<T1, T2, T3>(string storeName, object? parameter = null)
        {
            var returnTypes = new[] { typeof(T1), typeof(T2), typeof(T3) };
            var queryResult = await ExecuteMultipleAsync(storeName, returnTypes, parameter);
            if (queryResult.Count != 3) throw new InvalidOperationException($"Expect 3 return types, got {queryResult.Count}");
            return (queryResult[0].Select(item => (T1)item), queryResult[1].Select(item => (T2)item), queryResult[2].Select(item => (T3)item));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>)> ExecuteMultipleAsync<T1, T2, T3, T4>(string storeName, object? parameter = null)
        {
            var returnTypes = new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
            var queryResult = await ExecuteMultipleAsync(storeName, returnTypes, parameter);
            if (queryResult.Count != 4) throw new InvalidOperationException($"Expect 4 return types, got {queryResult.Count}");
            return (queryResult[0].Select(item => (T1)item), queryResult[1].Select(item => (T2)item)
                , queryResult[2].Select(item => (T3)item), queryResult[3].Select(item => (T4)item));
        }

        public List<IEnumerable<object>> ExecuteMultiple(string storeName, Type[] returnTypes, object? parameter = null)
        {
            using var connection = GetConnection();
            var queryResult = connection.QueryMultiple(storeName, parameter, commandType: CommandType.StoredProcedure);
            return returnTypes.Select(type => queryResult.Read(type)).ToList();
        }

        public IEnumerable<T> Execute<T>(string storeName, object? parameter = null)
        {
            using var connection = GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var result = connection.Query<T>(storeName, parameter, commandType: CommandType.StoredProcedure);
            return result;
        }

        protected virtual SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

    }
}
