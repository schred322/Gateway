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

        public async Task GetPartsListByVendor(dynamic db_params, dynamic proc_params)
        {  
            var result = await _db.SqlSource(db_params, proc_params);
            
            _job.DbData = result;
        }

        public async Task SetPartByVendor(dynamic db_params, dynamic proc_params)
        {
            var sqlText = db_params.sql_text.Value;

            var x = _job.DbData;

            await _db.SqlDestination(db_params.source_connection_string.Value, sqlText, proc_params); 
        }
    }
}
