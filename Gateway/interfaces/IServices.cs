namespace Gateway.interfaces
{
    public interface IServices
    { 
        void LogError(dynamic log);
        void Logger(string log_file, dynamic log);
        void LogInfo(dynamic log);
    }
}