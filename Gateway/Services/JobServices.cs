using Gateway.interfaces;
using Gateway.Providers;
using System;
using System.Dynamic;

namespace Gateway.Services
{ 
    public class JobServices : IJobServices
    {
        ILogService _log; 
        readonly DataServiceProvider _dataServiceProvider;
        const string IncomingPath = @"C:\Users\bokbok\source\repos\Gateway\Gateway\data\installs\install-test.json";
        
        //public dynamic Task { get; set; }
        public dynamic FileInfo { get; set; } = new ExpandoObject();
        public dynamic Task { get; set; } = new ExpandoObject();

        public JobServices(ILogService log, DataServiceProvider dataServiceProvider)
        {
            _log = log; 
            _dataServiceProvider = dataServiceProvider;

            LoadJob();
        } 

        private void LoadJob()
        {
            var data = _dataServiceProvider.LoadJsonFile(IncomingPath);
            if (data == null)
            {
                _log.LogError($"Failed to load job data from {IncomingPath}");
                throw new Exception($"Failed to load job data from {IncomingPath}");
            }
            Task =  data.task; 
        } 
    }
}
