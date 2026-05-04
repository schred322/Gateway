
using System;
using System.IO;

namespace Gateway
{
    public class Services : Gateway
    {
        public static void LogError(dynamic log)
        {
            var d = Path.GetDirectoryName(_job.task.error_log_path.Value);
            if (!Directory.Exists(d)) { Directory.CreateDirectory(d); }
            Logger(_job.task.error_log_path.Value, log);
        }
        public static string AppendTimeStamp(string file, string format)
        {
            try
            {
                return string.Concat(
                    Path.GetFileNameWithoutExtension(file),
                    DateTime.Now.ToString(format),
                    Path.GetExtension(file));
            }
            catch (Exception e) { throw e; }
        }
        public static void Logger(string log_file, dynamic log)
        {
            using StreamWriter writer = new StreamWriter(log_file, append: true);
            writer.WriteLine(string.Empty);
            writer.WriteLine(string.Concat("log_date_time: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            writer.WriteLine(log.InnerException);
        }
    }
}
