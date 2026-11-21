
using Gateway.interfaces;
using Gateway.Providers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Dynamic; 
using System.Reflection;

namespace Gateway
{
    public class Gateway
    { 
        const string gateway_operations = "Gateway.Operations";
        public ILogService _log;
        public IJobServices _job;
          
        //public ILogService _log;  
        public Gateway(IJobServices job, ILogService log)
        {
            _job = job; 
            _log = log;
             
            _log.LogInfo($"***************************** Loading Job *****************************", separator: true);
            _log.LogInfo($"Job: {_job.Task.name.Value}");
        }
 
        public async Task RunAsync()
        {
            try
            {
                foreach (var file in _job.Task.files)
                {
                    var directory = new DirectoryInfo(file.source.Value);

                    foreach (var fileInfo in directory.GetFiles(file.pattern.Value))
                    {
                        _job.FileInfo = fileInfo;
                        _log.LogInfo($"FileInfo: {fileInfo}");

                        ProcessRoutes(file);
                    }
                } 
                _log.LogInfo($"Job Completed");
            }
            catch (Exception e) {
                _log.LogInfo($"An error was thrown in Gateway().RunAsync(); Check error log for details.");
                _log.LogError(e); 
            }
        }

        private void ProcessRoutes(dynamic file)
        {
            foreach (dynamic route in _job.Task.routes)
            {
                if (!file.routes.Value.Contains(route.name.Value))
                    continue;

                _log.LogInfo($"Route Match: {route.name.Value}");

                foreach (var operation in route.operations)
                    ExecuteOperation(operation);
            }
        }

        private void ExecuteOperation(dynamic operation)
        {
            _log.LogInfo($"Operation: {operation.method.Value}"); 

            var operations = new Operations(_log, _job);
            var method = typeof(Operations).GetMethod(operation.method.Value);

            if (method == null)
                throw new InvalidOperationException(
                    $"Operation '{operation.method.Value}' was not found.");

            method.Invoke(operations, new[] { operation.parameters[0] });
        }
    }
}
