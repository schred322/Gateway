using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Interfaces
{
    public interface IDbService
    {
        Task<List<dynamic>> SqlSource(dynamic db_params, dynamic proc_params);
        Task SqlDestination(dynamic data, dynamic db_params, dynamic proc_params);
        Task<List<dynamic>> GetPartsListAll();
    }
}
