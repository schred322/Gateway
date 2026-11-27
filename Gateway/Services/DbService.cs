using Dapper;
using Gateway.Interfaces;
using Gateway.Services.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Gateway.Services
{
    public class DbService : IDbService
    {
        IDbServiceProvider _dbServiceProvider;

        public DbService(IDbServiceProvider dbServiceProvider)
        {
            _dbServiceProvider = dbServiceProvider; 
        }

        public async Task<List<dynamic>> SourceDb_DataListRetrievalAsync(dynamic db_params, dynamic proc_params)
        {
            await using var connection = _dbServiceProvider.GetSqlConnection(db_params.connection_string.Value); 
            return await new DataAccessDbService().GetSourceDbDataListAsync(connection, db_params, proc_params); 
        }

        public async Task<List<dynamic>> DestinationDb_DataListTransferAsync(dynamic data, dynamic db_params, dynamic proc_params)
        { 
            await using var connection = _dbServiceProvider.GetSqlConnection(db_params.connection_string.Value);
            var x = await new DataAccessDbService().SetDestinationDbDataListAsync(connection, data, db_params, proc_params);
            //todo: return
            return [];
        }
         
    }
}
