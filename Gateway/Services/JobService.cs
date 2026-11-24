using Gateway.Interfaces;
using Gateway.Providers;
using System;
using System.Dynamic;

namespace Gateway.Services
{ 
    public class JobService : IJobService
    {
        ILogService _log; 
        readonly FileServiceProvider _fileService;
        //const string IncomingPath = @"C:\Users\bokbok\source\repos\Gateway\Gateway\Jobs\install-test.json";
        //const string IncomingPath = @"C:\Users\bokbok\source\repos\Gateway\Gateway\Jobs\template_file_operations.json";
        const string IncomingPath = @"C:\Users\bokbok\source\repos\Gateway\Gateway\Jobs\template_db_layout.json";
        public dynamic DbData { get; set; } = new ExpandoObject();
        public dynamic FileInfo { get; set; } = new ExpandoObject();
        public dynamic FileParam { get; set; } = new ExpandoObject();
        public dynamic Task { get; set; } = new ExpandoObject(); 
        public JobService(ILogService log, FileServiceProvider fileService)
        {
            _log = log; 
            _fileService = fileService;

            LoadTask();
        } 

        private void LoadTask()
        {
            var data =  _fileService.LoadJsonFile(IncomingPath);
            if (data == null)
            {
                _log.LogError($"Failed to load job data from {IncomingPath}");
                throw new Exception($"Failed to load job data from {IncomingPath}");
            }
            Task =  data.task; 
        } 
    }
}
