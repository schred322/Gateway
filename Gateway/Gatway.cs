
using Gateway.Common;
using Gateway.Interfaces;
using Gateway.Modules;
using System.Diagnostics;
using GatewayUtilities = Gateway.Common.Utilities;

namespace Gateway
{
    public class Gateway
    {  
        public ILogService _log;
        public IJobService _job;
        public IDbService _db;
        public Gateway(IJobService job, ILogService log, IDbService db)
        {
            _job = job; 
            _log = log;
            _db = db;

            _log.LogInfo($"***************************** Loading Job *****************************", separator: true);
            _log.LogInfo($"Job: {_job.Task.name.Value}");
        } 

        public async Task RunAsync()
        {
            try
            {
                foreach (var routine in _job.Task.routines)
                { 
                    if (routine.module.Value == "DbOperations")
                        await ProcessDbOperations(routine);

                    if (routine.module.Value == "FileOperations")
                        await ProcessFileOperations(routine);
                    
                } 
                _log.LogInfo($"Job Completed");
            }
            catch (Exception e) {
                _log.LogInfo($"An error was thrown in Gateway().RunAsync(); Check error log for details.");
                _log.LogError(e); 
            }
        }

        private async Task ProcessDbOperations(dynamic routine)
        {
            foreach (var operation in routine.operations)
            {
                var method = typeof(DbOperations).GetMethod(operation.method.Value);
                if (method == null)
                    throw new InvalidOperationException($"DbOperations '{operation.method.Value}' was not found.");

                object[] param_group = {
                    GatewayUtilities.ObjectNullCheck(operation.db_params),
                    GatewayUtilities.ObjectNullCheck(operation.proc_params)
                };

                var ops = new DbOperations(_log, _job, _db);
                await (Task)method.Invoke(ops, param_group);
            }
        }

        private async Task ProcessFileOperations(dynamic routine)
        {
            try
            {
                foreach (var operation in routine.operations)
                {
                    var method = typeof(FileOperations).GetMethod(operation.method.Value);
                    if (method == null)
                        throw new InvalidOperationException($"FileOperations '{operation.method.Value}' was not found.");

                    var p = (operation.parameters != null)
                                ? GatewayUtilities.ObjectNullCheck(operation.parameters)
                                : operation;

                    var ops = new FileOperations(_log, _job);
                    method.Invoke(ops, new object[] { p });
                }
            }
            catch(Exception e)
            {
                _log.LogInfo($"An error was thrown in Gateway().ProcessFileOperations(); Check error log for details.");
                _log.LogError($"An error was thrown in Gateway().ProcessFileOperations(); Check error log for details.");
                _log.LogError(e);
            }
        } 
    }
} 