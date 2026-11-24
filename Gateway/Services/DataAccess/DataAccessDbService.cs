using Dapper;
using Gateway.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gateway.Services.DataAccess
{
    sealed class DataAccessDbService
    { 
        public async Task<List<dynamic>> SqlSource(SqlConnection connection, dynamic db_params, dynamic proc_params)
        { 
            //todo: add null check for proc_params;
            var p = new DynamicParameters(proc_params);
            var ret = await connection.QueryAsync<dynamic>((string)db_params.sql_text.Value, param: null, commandType: CommandType.StoredProcedure);
            return ret.ToList(); 
        }

        public async Task<List<dynamic>> SqlDestination(SqlConnection connection, dynamic data, dynamic db_params, DynamicParameters proc_params)
        {
            var p = new DynamicParameters(proc_params);
            var ret = await connection.QueryAsync<dynamic>((string)db_params.sql_text.Value, param: p, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }   

        public async Task<List<dynamic>> GetPartsListAll()
        { 
            return new List<dynamic>();
        }
    }
}
