using Gateway.interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json;

namespace Gateway.Services
{ 
    public class JobServices : IJobServices
    {
        ILogService _log; 
        string _incoming_path { get; set; }

        dynamic _instance;
        public dynamic Instance 
        {
            get
            {
                if (_instance == null)
                {
                    return new ExpandoObject();
                }
                return _instance;
            }
                
            set
            {
                _instance = value;
            } 
        }

        public JobServices(ILogService log)
        {
            _log = log;
            _instance = Instance;

            LoadJob();
            //string _incoming_path = @"C:\Users\bokbok\source\repos\Gateway\Gateway\data\installs\install-test.json";
            //using StreamReader file_stream = File.OpenText(_incoming_path);
            //Job = JsonSerializer.Deserialize<dynamic>(file_stream.BaseStream);
        }

        public void LoadJob()
        {

            _log.LogInfo("JobServices.LoadJob");

            string _incoming_path = @"C:\Users\bokbok\source\repos\Gateway\Gateway\data\installs\install-test.json";
            using StreamReader file_stream = File.OpenText(_incoming_path);
            Instance = JsonSerializer.Deserialize<dynamic>(file_stream.BaseStream);

        }
    }
}
