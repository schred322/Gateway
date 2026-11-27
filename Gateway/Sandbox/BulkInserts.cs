using System.Data;
using Microsoft.Data.SqlClient;


namespace Gateway.Sandbox
{
    public class BulkInserts
    { 
        public async Task TrueBulkInsertDynamicAsync(
            IEnumerable<IDictionary<string, object>> records,
            string tableName,
            string connectionString)
            {
                var recordList = records.ToList();
                if (!recordList.Any()) return;

                // 1. Build DataTable schema dynamically from keys
                var dataTable = new DataTable();
                var firstRow = recordList.First();

                foreach (var kvp in firstRow)
                {
                    // Infer type from first non-null value, fallback to object/string
                    var sampleVal = recordList.Select(r => r[kvp.Key]).FirstOrDefault(v => v != null);
                    dataTable.Columns.Add(kvp.Key, sampleVal?.GetType() ?? typeof(object));
                }

                // 2. Populate rows
                foreach (var record in recordList)
                {
                    var row = dataTable.NewRow();
                    foreach (var kvp in record)
                    {
                        row[kvp.Key] = kvp.Value ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }

                // 3. Stream directly to SQL Server
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                using var bulkCopy = new SqlBulkCopy(connection)
                {
                    DestinationTableName = tableName
                };

                // Map DataTable columns to DB columns by name
                foreach (DataColumn col in dataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                await bulkCopy.WriteToServerAsync(dataTable);
            }

        }
}
