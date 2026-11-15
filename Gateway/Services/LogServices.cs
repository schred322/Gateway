using Gateway.interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Gateway.Services
{

    public class LogServices : ILogService
    {
        string _incoming_path = @"C:\Users\bokbok\source\repos\Gateway\Gateway\data\installs\install-test.json";
        dynamic _log = new ExpandoObject();

        public LogServices() 
        { 
            //using FileStream fileStream = File.OpenRead(_incoming_path);
            //JsonNode? _log = JsonSerializer.Deserialize<JsonNode>(fileStream);

            //using StreamReader file_stream = File.OpenText(_incoming_path);
            //_log = new JsonSerializer().Deserialize(file_stream.BaseStream, typeof(object), null);

           //var x = _log.task.error_log_path.Value;

        }

        public void Logger(string log_file, dynamic log)
        {
            using StreamWriter writer = new StreamWriter(log_file, append: true);
            writer.WriteLine(string.Empty);
            writer.WriteLine(string.Concat("log_date_time: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            writer.WriteLine(log.InnerException);
        }
        public void LogError(dynamic log)
        {
            //var d = Path.GetDirectoryName(_log.task.error_log_path.Value);
            //if (!Directory.Exists(d)) { Directory.CreateDirectory(d); }
            //Logger(_log.task.error_log_path.Value, log);
        }
        public void LogInfo(dynamic log)
        {
            //var d = Path.GetDirectoryName(_log.task.info_log_path.Value);
            //if (!Directory.Exists(d)) { Directory.CreateDirectory(d); }
            //Logger(_log.task.info_log_path.Value, log);
        }
    }
}
