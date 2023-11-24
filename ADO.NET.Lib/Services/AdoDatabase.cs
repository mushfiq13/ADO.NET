using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using ADO.NET.Lib.Extensions;

namespace ADO.NET.Lib.Services;

public class AdoDatabase : IAdoDatabase
{
	private readonly IDbConnection _connection;
	private readonly IDbTransaction _transaction;

	public AdoDatabase(string connectionString)
	{
		_connection = new SqlConnection(connectionString);
		_transaction = _connection.BeginTransaction();
	}

	public async Task<IList<List<(string column, object value)>>> ExecuteQueryAsync(
		string query,
		List<DbParameter>? parameters = null)
	{
		using var command = await CreateCommandAsync();

		command.CommandText = query;
		if (parameters is not null) {
			parameters.ForEach(item => command.Parameters.Add(item));
		}

		using var reader = command.ExecuteReader();
		var records = await reader.ReadDataAsync();

		_connection.Close();

		return records;
	}

	public async Task ExecuteNonQueryAsync(
		string sql,
		List<DbParameter>? parameters = null)
	{
		using var command = await CreateCommandAsync();

		command.CommandText = sql;
		if (parameters is not null) {
			parameters.ForEach(item => command.Parameters.Add(item));
		}

		command.ExecuteNonQuery();
		_transaction.Commit();
		_connection.Close();
	}

	private async Task<IDbCommand> CreateCommandAsync()
	{
		if (_connection.State == ConnectionState.Open) {
			_connection.Open();
		}

		return await Task.FromResult(_connection.CreateCommand());
	}

	public void Dispose()
	{
		_transaction?.Dispose();
		_connection?.Dispose();
	}
}
