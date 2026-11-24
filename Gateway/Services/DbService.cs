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

        public async Task<List<dynamic>> SqlSource(dynamic db_params, dynamic proc_params)
        {
            await using var connection = _dbServiceProvider.GetSqlConnection(db_params.source_connection_string.Value); 
            return await new DataAccessDbService().SqlSource(connection, db_params, proc_params); 
        }

        public async Task SqlDestination(dynamic data, dynamic db_params, dynamic proc_params)
        { 
            await using var connection = _dbServiceProvider.GetSqlConnection(db_params.destination_connection_string.Value);
            await new DataAccessDbService().SqlDestination(connection, data, db_params.sql_text.Value, proc_params);
        }

        public async Task<List<dynamic>> GetPartsListAll()
        {
            // Implementation for getting parts list
            return new List<dynamic>();
        }
    }
}
