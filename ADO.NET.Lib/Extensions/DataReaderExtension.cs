using System.Data;

namespace ADO.NET.Lib.Extensions;

public static class DataReaderExtension
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="reader"></param>
	/// <returns>
	/// List of records. For example, first record will contain a group of
	/// data where each item points to the column name and value of that
	/// field for the corresponding row.
	/// </returns>
	public static async Task<IList<List<(string column, object value)>>> ReadDataAsync(this IDataReader reader)
	{
		var records = new List<List<(string, object)>>();

		while (reader.Read()) {
			var record = new List<(string, object)>();

			for (var i = 0; i < reader.FieldCount; ++i) {
				record.Add((reader.GetName(i), reader.GetValue(i)));
			}

			records.Add(record);
		}

		return await Task.FromResult(records);
	}
}
