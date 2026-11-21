using Gateway.interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace Gateway.Services
{

    public class LogServices : ILogService
    { 
        private readonly IConfiguration _config;  
        public LogServices() 
        {
            _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                .Build(); 
        }

        /// <summary>
        /// Log to file
        /// </summary>
        /// <param name="log_file"></param>
        /// <param name="log_entry"></param>
        public void Logger(string log_file, string log_entry)
        {
            using StreamWriter writer = new StreamWriter(log_file, append: true);  
            writer.WriteLine(log_entry); 
        }

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="log"></param>
        public void LogError(dynamic log)
        {
            var entry_log =  $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} error-log: {log}";

            Logger(_config.GetValue<string>("LogSettings:ErrorLogPath") ?? string.Empty, entry_log);
             
            LogDebug("error-log", entry_log); 
        }
         
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="log"></param>
        public void LogInfo(dynamic log, bool separator = false)
        { 
            var entry_log = (separator)
                            ? $"{log}" 
                            : $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} info-log: {log}";  

            Logger(_config.GetValue<string>("LogSettings:InfoLogPath") ?? string.Empty, entry_log);

            LogDebug("info-log", entry_log);
        }

        /// <summary>
        /// Set debug to output window, located in appsettings.config
        /// </summary>
        /// <param name="type"></param>
        /// <param name="log_entry"></param>
        public void LogDebug(string type, string log_entry)
        {
            if (_config.GetValue<bool>("LogSettings:OutputDebug"))
            {
                Debug.WriteLine($"{type}: {log_entry}");
            }
        }
    }
}
