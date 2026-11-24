namespace Gateway.Interfaces
{
    public interface IFileServiceProvider
    {
        dynamic LoadJsonFile(string filePath);
    }
}