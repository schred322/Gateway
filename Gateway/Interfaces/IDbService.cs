using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Interfaces
{
    public interface IDbService
    {
        Task<List<dynamic>> SourceDb_DataListRetrievalAsync(dynamic db_params, dynamic proc_params);
        Task<List<dynamic>> DestinationDb_DataListTransferAsync(dynamic data, dynamic db_params, dynamic proc_params); 
    }
}
