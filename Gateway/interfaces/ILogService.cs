namespace Gateway.interfaces
{
    public interface ILogService
    {
        void LogError(dynamic log);
        void LogInfo(dynamic log);
    }
}