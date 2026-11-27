using Dapper;
using Gateway.Interfaces; 

namespace Gateway.Modules
{
    public class DbOperations
    {
        ILogService _log;
        IJobService _job;
        IDbService _db;

        public DbOperations(ILogService log, IJobService job, IDbService db)
        {
            _log = log;
            _job = job;
            _db = db;
        }

        /// <summary>
        /// Get source data with parameters
        /// </summary>
        /// <param name="db_params"></param>
        /// <param name="proc_params"></param>
        /// <returns></returns>

        public async Task GetSourceDbDataListWithParametersAsync(dynamic db_params, dynamic proc_params)
        {  
            var result = await _db.SourceDb_DataListRetrievalAsync(db_params, proc_params);
            
            _job.DbDataList = result;
        }

        public async Task SetDestinationDbDataWithParametersAsync(dynamic db_params, dynamic proc_params)
        { 
            var ret = await _db.DestinationDb_DataListTransferAsync(_job.DbDataList, db_params, proc_params);
            // todo: Handle the result if needed
        }
    }
}
