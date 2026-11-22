
using Gateway.interfaces;
using Gateway.Providers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using System.Text.Json;
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
             
            _log.LogInfo($"***************************** Loading Job *****************************", title: true);
            _log.LogInfo($"Job: {_job.Task.name.Value}");
        }
 
        public async Task RunAsync()
        {
            try
            {
                foreach (var file in _job.Task.files)
                {
                    _log.LogInfo($"File pattern: {file.pattern.Value}");
                    _log.LogInfo($"Drop location: {file.source.Value}"); 
                    foreach (FileInfo file_info in new DirectoryInfo(file.source.Value).GetFiles(file.pattern.Value))
                    {
                        _job.FileInfo = file_info; 
                        _log.LogInfo($"Load file: {file_info}"); 
                        foreach (dynamic route in _job.Task.routes)
                        { 
                            if (file.routes.Value.Contains(route.name.Value))
                            {
                                _log.LogInfo($"Route: {route.name.Value}");
                                foreach (var op in route.operations)
                                {
                                    _log.LogInfo($"Operation: {op.method.Value}"); 
                                    foreach (var c in Assembly.GetExecutingAssembly().GetTypes().Where(m => m.Name.Contains(typeof(Operations).Name)))
                                    {  
                                        Type[] parameterTypes = new Type[] { typeof(ILogService), typeof(IJobServices) };
                                        var instance = Type.GetType(gateway_operations).GetConstructor(parameterTypes).Invoke(new object[] { _log, _job }); 
                                        c.GetMethod(op.method.Value, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                            .Invoke(instance, new object[] { op.parameters[0] });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                _log.LogInfo($"An error was thrown in Gateway().RunAsync(); Check error log for details.");
                _log.LogError(e); 
            }
        }
    }
}
