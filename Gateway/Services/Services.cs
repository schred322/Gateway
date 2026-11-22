using Gateway.Interfaces;
using System;
using System.IO;

namespace Gateway.Services
{
    public class Services : IServices
    {

        ILogService _log;
        IServices _services;

        public Services(ILogService log, IServices services )
        {
            _log = log;
            _services = services;
        }

        public void LogError(dynamic log)
        {
            throw new NotImplementedException();
        }

        public void Logger(string log_file, dynamic log)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(dynamic log)
        {
            throw new NotImplementedException();
        }
    }
}
