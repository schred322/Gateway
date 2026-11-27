using Dapper;
using Gateway.Interfaces;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Gateway.Services.DataAccess
{
    sealed class DataAccessDbService
    { 
        public async Task<List<dynamic>> GetSourceDbDataListAsync(SqlConnection connection, dynamic db_params, dynamic proc_params)
        {
            try
            { 
                var param_list = ((JObject)proc_params).ToObject<Dictionary<string, object>>();
                var p = new DynamicParameters();
                p.AddDynamicParams(param_list); 
                var ret = await connection.QueryAsync<dynamic>((string)db_params.sql_text.Value,
                                                                param: p,
                                                                commandType: CommandType.StoredProcedure); 
                return ret.ToList();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
        public async Task<List<dynamic>> SetDestinationDbDataListAsync(SqlConnection connection, dynamic data, dynamic db_params, dynamic proc_params)
        {
            var param_list = ((JObject)proc_params).ToObject<Dictionary<string, object>>();
            var p = new DynamicParameters();
            p.AddDynamicParams(param_list);

            var ret = await connection.QueryAsync<dynamic>((string)db_params.sql_text.Value, 
                                                            param: p, 
                                                            commandType: CommandType.StoredProcedure); 
            return ret.ToList();
        }

        public async Task SetDestinationDbDataListBulkInsertAsync(SqlConnection connection, dynamic data, dynamic db_params, dynamic proc_params)
        {
            //todo: logging, error handling, etc.

            //var param_list = ((JObject)proc_params).ToObject<Dictionary<string, object>>();
            //var p = new DynamicParameters();
            //p.AddDynamicParams(param_list);
             
            IEnumerable<IDictionary<string, object>> res = ((IEnumerable<dynamic>)data).Cast<IDictionary<string, object>>();
            await BulkInsertDataListAsync(res, (string)db_params.table_name.Value, connection.ConnectionString);
        }

        /// <summary>
        /// Bulk sql inserts
        /// </summary>
        /// <param name="records"></param>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private async Task BulkInsertDataListAsync(
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
