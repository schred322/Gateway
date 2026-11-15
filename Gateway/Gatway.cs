
using Gateway.interfaces;
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
        private static readonly string _incoming_path = @"C:\Users\bokbok\source\repos\Gateway\Gateway\data\installs\install-test.json";
        //public static dynamic _job = new ExpandoObject();
        const string gateway_operations = "Gateway.Operations"; 
        //public dynamic _job = new ExpandoObject();
        //public dynamic _instance = new ExpandoObject();
        public dynamic _log = new ExpandoObject();
        public IJobServices _job;
        Common.Extensions _extentions;

        //public ILogService _log;  
        public Gateway(IJobServices job, ILogService log)
        {
            _job = job;
            _log = log;
            _extentions = new Common.Extensions();

            using StreamReader file_stream = File.OpenText(_incoming_path);
            _job.Instance = new JsonSerializer().Deserialize(file_stream, typeof(object));
        }
         
        public async Task RunAsync()
        {
            try
            { 
                foreach (var file in _job.Instance.task.files)
                { 
                    _job.Instance.FilePattern = file.pattern.Value;
                    _job.Instance.FilePath = file.source.Value;

                    _log.LogInfo($"Processing file pattern: {file.pattern.Value} in path: {file.source.Value}");
                    foreach (FileInfo file_info in new DirectoryInfo(file.source.Value).GetFiles(file.pattern.Value))
                    {
                        _job.Instance.FileName = file_info.Name;
                        _job.Instance.CurrentFilePathAndName = file_info.FullName; 

                        _log.LogInfo($"Processing file: {file_info.FullName}");
                        
                        foreach (var route in _job.Instance.task.routes)
                        {
                            _log.LogInfo($"Checking route: {route.name.Value}");
                            if (file.routes.Value.Contains(route.name.Value))
                            {
                                _log.LogInfo($"Route matched: {route.name.Value}");
                                foreach (var op in route.operations)
                                {
                                    _log.LogInfo($"Executing operation: {op.method.Value} with parameters: {JsonConvert.SerializeObject(op.parameters)}");
                                    foreach (var c in Assembly.GetExecutingAssembly().GetTypes().Where(m => m.Name.Contains(typeof(Operations).Name)))
                                    {
                                        _log.LogInfo($"Found class: {c.Name} for operation: {op.method.Value}");
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
            catch (Exception e) { _log.LogError(e); }
        }
    }
}
