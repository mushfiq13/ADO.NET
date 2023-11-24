using System.Data.Common;

namespace ADO.NET.Lib.Services
{
	public interface IAdoDatabase : IDisposable
	{
		Task ExecuteNonQueryAsync(string sql, List<DbParameter>? parameters = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <param name="parameters"></param>
		/// <returns>
		/// List of records. For example, first record will contain a group of
		/// data where each item points to the column name and value of that
		/// field for the corresponding row.
		/// </returns>
		Task<IList<List<(string column, object value)>>> ExecuteQueryAsync(string query, List<DbParameter>? parameters = null);
	}
}