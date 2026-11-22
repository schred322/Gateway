namespace Gateway.Interfaces
{
    public interface ILogService
    { 
        void LogError(dynamic log);
        void LogInfo(dynamic log, bool separator = false);
    }
}